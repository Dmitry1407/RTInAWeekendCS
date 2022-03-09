using RTInAWeekendCS.primitives.hit;
using RTInAWeekendCS.material;
using RTInAWeekendCS.utils;
using System;

namespace RTInAWeekendCS.primitives
{
    class Torus : Primitive
    {
        private Single r1;
        private Single r2;
        private Vector3F vtmp = new Vector3F();
        private Vector3F normal = new Vector3F();
        private BBox bbox;
        private Single kEpsilon = 0.0001F;
        private Single kHugeValue = 1.0E10F;

        Single[] coeffsHit = new Single[5];	// coefficient array for the quartic equation
        Single[] roots = new Single[4];	// solution array for the quartic equ
        Single[] coeffs = new Single[4];
        Single[] coeffs2 = new Single[4];

        Single[] solve2RET = new Single[2];
        Single[] solve3RET = new Single[3];
        Single[] solve4RET = new Single[4];

        public Torus(Vector3F center, Single radius1, Single radius2, Material material)
        {
            this.Center = center;
            this.r1 = radius1;
            this.r2 = radius2;
            this.Material = material;
            Record = new HitRecord();
            Record.Material = Material;
            bbox = new BBox(-r1 - r2, r1 + r2, -r2, r2, -r1 - r2, r1 + r2);
        }

        public override HitRecord Hit(Ray ray, Single tMin, Single tMax)
        {
            if (bbox.Hit(ray, tMin, tMax) == null)
                return null;

            Single x1 = ray.Origin.X; Single y1 = ray.Origin.Y; Single z1 = ray.Origin.Z;
            Single d1 = ray.Direction.X; Single d2 = ray.Direction.Y; Single d3 = ray.Direction.Z;

            // define the coefficients of the quartic equation	
            Single sum_d_sqrd = d1 * d1 + d2 * d2 + d3 * d3;
            Single e = x1 * x1 + y1 * y1 + z1 * z1 - r1 * r1 - r2 * r2;
            Single f = x1 * d1 + y1 * d2 + z1 * d3;
            Single four_a_sqrd = 4F * r1 * r1;

            coeffsHit[0] = e * e - four_a_sqrd * (r2 * r2 - y1 * y1); 	// constant term
            coeffsHit[1] = 4F * f * e + 2F * four_a_sqrd * y1 * d2;
            coeffsHit[2] = 2F * sum_d_sqrd * e + 4F * f * f + four_a_sqrd * d2 * d2;
            coeffsHit[3] = 4F * sum_d_sqrd * f;
            coeffsHit[4] = sum_d_sqrd * sum_d_sqrd;  					// coefficient of t^4

            // find roots of the quartic equation
            int num_real_roots = SolveQuartic(coeffsHit, roots);

            bool intersected = false;
            Single t = kHugeValue;

            if (num_real_roots == 0)  // ray misses the torus
                return (null);

            // find the smallest root greater than kEpsilon, if any
            // the roots array is not sorted

            for (int j = 0; j < num_real_roots; j++)
                if (roots[j] > kEpsilon)
                {
                    intersected = true;
                    if (roots[j] < t)
                        t = roots[j];
                }

            if (!intersected)
                return (null);

            tMin = t;
            ////Record.Position = ray.Origin + t * ray.Direction;
            Vector3F.Mul(vtmp, ray.Direction, t);
            vtmp.Add(ray.Origin);
            Record.Position.CopyFrom(vtmp);
            Record.Normal = compute_normal(Record.Position);

            return (Record);
        }

        private Vector3F compute_normal(Vector3F p)
        {
            Single sum_squared = p.SqrtLength();// x * x + y * y + z * z;
            Single param_squared = r1 * r1 + r2 * r2;

            normal.X = 4F * p.X * (sum_squared - param_squared);
            normal.Y = 4F * p.Y * (sum_squared - param_squared + 2F * r1 * r1);
            normal.Z = 4F * p.Z * (sum_squared - param_squared);
            ////normal.normalize();
            Single lenth = normal.Length();
            normal.Div(lenth);
            return (normal);
        }

        private int SolveQuartic(Single[] c, Single[] s)
        {
            Single z, u, v, sub;
            Single A, B, C, D;
            Single sq_A, p, q, r;
            int i, num;

            /* normal form: x^4 + Ax^3 + Bx^2 + Cx + D = 0 */
            A = c[3] / c[4];
            B = c[2] / c[4];
            C = c[1] / c[4];
            D = c[0] / c[4];

            /*  substitute x = y - A/4 to eliminate cubic term:
            x^4 + px^2 + qx + r = 0 */
            sq_A = A * A;
            p = -3F / 8 * sq_A + B;
            q = 1F / 8 * sq_A * A - 1F / 2 * A * B + C;
            r = -3F / 256 * sq_A * sq_A + 1F / 16 * sq_A * B - 1F / 4 * A * C + D;

            if (IsZero(r))
            {
                /* no absolute term: y(y^3 + py + q) = 0 */
                coeffs[0] = q;
                coeffs[1] = p;
                coeffs[2] = 0;
                coeffs[3] = 1;
                num = SolveCubic(coeffs, s);
                s[num++] = 0;
            }
            else
            {
                /* solve the resolvent cubic ... */
                coeffs[0] = 1F / 2 * r * p - 1F / 8 * q * q;
                coeffs[1] = -r;
                coeffs[2] = -1F / 2 * p;
                coeffs[3] = 1;
                SolveCubic(coeffs, s);

                /* ... and take the one real solution ... */
                z = s[0];

                /* ... to build two quadric equations */
                u = z * z - r;
                v = 2 * z - p;

                if (IsZero(u))
                    u = 0;
                else if (u > 0)
                    u = (Single)Math.Sqrt(u);
                else
                    return 0;

                if (IsZero(v))
                    v = 0;
                else if (v > 0)
                    v = (Single)Math.Sqrt(v);
                else
                    return 0;

                coeffs[0] = z - u;
                coeffs[1] = q < 0 ? -v : v;
                coeffs[2] = 1;
                num = SolveQuadric(coeffs, s);

                coeffs[0] = z + u;
                coeffs[1] = q < 0 ? v : -v;
                coeffs[2] = 1;

                coeffs2[0] = s[num];
                coeffs2[1] = s[num + 1];
                num += SolveQuadric(coeffs, coeffs2);
            }

            /* resubstitute */
            sub = 1F / 4 * A;

            for (i = 0; i < num; ++i)
                s[i] -= sub;

            return num;
        }

        private int SolveCubic(Single[] c, Single[] s)
        {
            int i, num;
            Single sub;
            Single A, B, C;
            Single sq_A, p, q;
            Single cb_p, D;

            /* normal form: x^3 + Ax^2 + Bx + C = 0 */
            A = c[2] / c[3];
            B = c[1] / c[3];
            C = c[0] / c[3];

            /*  substitute x = y - A/3 to eliminate quadric term:
            x^3 +px + q = 0 */
            sq_A = A * A;
            p = 1F / 3 * (-1F / 3 * sq_A + B);
            q = 1F / 2 * (2F / 27 * A * sq_A - 1F / 3 * A * B + C);

            /* use Cardano's formula */
            cb_p = p * p * p;
            D = q * q + cb_p;

            if (IsZero(D))
            {
                if (IsZero(q))
                { /* one triple solution */
                    s[0] = 0;
                    num = 1;
                }
                else
                { /* one single and one double solution */
                    Single u = cbrt(-q);
                    s[0] = 2 * u;
                    s[1] = -u;
                    num = 2;
                }
            }
            else if (D < 0)
            { /* Casus irreducibilis: three real solutions */
                Single phi = 1F / 3 * (Single)Math.Acos(-q / (Single)Math.Sqrt(-cb_p));
                Single t = 2 * (Single)Math.Sqrt(-p);

                s[0] = t * (Single)Math.Cos(phi);
                s[1] = -t * (Single)Math.Cos(phi + Math.PI / 3);
                s[2] = -t * (Single)Math.Cos(phi - Math.PI / 3);
                num = 3;
            }
            else
            { /* one real solution */
                Single sqrt_D = (Single)Math.Sqrt(D);
                Single u = cbrt(sqrt_D - q);
                Single v = -cbrt(sqrt_D + q);

                s[0] = u + v;
                num = 1;
            }

            /* resubstitute */
            sub = 1F / 3 * A;
            for (i = 0; i < num; ++i)
                s[i] -= sub;

            return num;
        }

        private int SolveQuadric(Single[] c, Single[] s)
        {
            Single p, q, D;
            /* normal form: x^2 + px + q = 0 */
            p = c[1] / (2 * c[2]);
            q = c[0] / c[2];
            D = p * p - q;

            if (IsZero(D))
            {
                s[0] = -p;
                return 1;
            }
            else if (D > 0)
            {
                Single sqrt_D = (Single)Math.Sqrt(D);
                s[0] = sqrt_D - p;
                s[1] = -sqrt_D - p;
                return 2;
            }
            return 0;
        }


        private Single cbrt(Single x)
        {
            return x > 0F ? (Single)Math.Pow(x, 1F / 3F) :
                (x < 0F ? -(Single)Math.Pow(-x, 1F / 3F) : 0F);
        }

        private Boolean IsZero(Single x)
        {
            return (x > -1e-90F && x < 1e-90F);
        }





        private Single[] solve2(Single[] c)
        {
            /* normal form: x^2 + px + q = 0 */
            Single p = c[1] / (2 * c[2]);
            Single q = c[0] / c[2];
            Single D = p * p - q;

            if (D < 0)
            {
                solve2RET[0] = Single.NaN;
                solve2RET[1] = Single.NaN;
            }
            else if (IsZero(D))
            {
                solve2RET[0] = -p;
                solve2RET[1] = Single.NaN;
            }
            else /* if (D > 0) */
            {
                Single sqrt_D = (Single)Math.Sqrt(D);
                solve2RET[0] = sqrt_D - p;
                solve2RET[1] = -sqrt_D - p;
            }
            return solve2RET;
        }

        private Single[] solve3(Single[] c)
        {
            int num;
            /* normal form: x^3 + Ax^2 + Bx + C = 0 */
            Single A = c[2] / c[3];
            Single B = c[1] / c[3];
            Single C = c[0] / c[3];

            /*  substitute x = y - A/3 to eliminate quadric term:
            x^3 +px + q = 0 */
            Single sq_A = A * A;
            Single p = 1F / 3 * (-1F / 3 * sq_A + B);
            Single q = 1F / 2 * (2F / 27 * A * sq_A - 1F / 3 * A * B + C);

            /* use Cardano's formula */
            Single cb_p = p * p * p;
            Single D = q * q + cb_p;

            ////Single s = null;
            if (IsZero(D))
            {
                if (IsZero(q)) /* one triple solution */
                {
                    solve3RET[0] = 0;
                    solve3RET[1] = Single.NaN;
                    solve3RET[2] = Single.NaN;
                    num = 1;
                }
                else /* one single and one double solution */
                {
                    Single u = cbrt(-q);
                    solve3RET[0] = 2 * u;
                    solve3RET[1] = -u;
                    solve3RET[2] = Single.NaN;
                    num = 2;
                }
            }
            else if (D < 0) /* Casus irreducibilis: three real solutions */
            {
                Single phi = 1F / 3 * (Single)Math.Acos(-q / Math.Sqrt(-cb_p));
                Single t = 2 * (Single)Math.Sqrt(-p);
                solve3RET[0] = t * (Single)Math.Cos(phi);
                solve3RET[1] = -t * (Single)Math.Cos(phi + Math.PI / 3);
                solve3RET[2] = -t * (Single)Math.Cos(phi - Math.PI / 3);
                num = 3;

            }
            else /* one real solution */
            {
                Single sqrt_D = (Single)Math.Sqrt(D);
                Single u = cbrt(sqrt_D - q);
                Single v = -cbrt(sqrt_D + q);
                solve3RET[0] = u + v;
                solve3RET[1] = Single.NaN;
                solve3RET[2] = Single.NaN;
                num = 1;
            }

            /* resubstitute */
            Single sub = 1F / 3 * A;
            for (int i = 0; i < num; ++i)
            {
                solve3RET[i] -= sub;
            }

            return solve3RET;
        }

        ///**
        // *  Solves equation:
        // *  c[0] + c[1]*x + c[2]*x^2 + c[3]*x^3 + c[4]*x^4 = 0
        // */
        //private Single[]  solve4(Single[]c) {
        //    /* normal form: x^4 + Ax^3 + Bx^2 + Cx + D = 0 */
        //    Single A = c[3] / c[4];
        //    Single B = c[2] / c[4];
        //    Single C = c[1] / c[4];
        //    Single D = c[0] / c[4];

        //    /*  substitute x = y - A/4 to eliminate cubic term:
        //    x^4 + px^2 + qx + r = 0 */
        //    Single sq_A = A * A;
        //    Single p = - 3F / 8 * sq_A + B;
        //    Single q = 1F / 8 * sq_A * A - 1F / 2 * A * B + C;
        //    Single r = - 3F / 256 * sq_A * sq_A + 1F / 16 * sq_A * B - 1F / 4 * A * C + D;
        //    ////let s = null;

        //    if (IsZero(r)) {
        //        /* no absolute term: y(y^3 + py + q) = 0 */
        //        let coeffs = [q, p, 0, 1];
        //        s = solve3(coeffs);
        //        s.push(0);
        //    }
        //    else {
        //        /* solve the resolvent cubic ... */
        //        let coeffs = [
        //            1F / 2 * r * p - 1F / 8 * q * q,
        //            - r,
        //            - 1F / 2 * p,
        //            1];

        //        s = solve3(coeffs);

        //        /* ... and take the one real solution ... */
        //        Single z = s[0];

        //        /* ... to build two quadric equations */
        //        Single u = z * z - r;
        //        Single v = 2 * z - p;

        //        if (IsZero(u))
        //            u = 0;
        //        else if (u > 0)
        //            u = (Single)Math.Sqrt(u);
        //        else
        //            return 0;

        //        if (IsZero(v))
        //            v = 0;
        //        else if (v > 0)
        //            v = (Single)Math.Sqrt(v);
        //        else
        //            return 0;

        //        coeffs = [
        //            z - u,
        //            q < 0 ? -v : v,
        //            1];

        //        s = solve2(coeffs);
        //        coeffs = [z + u,
        //            q < 0 ? v : -v,
        //            1];

        //        s = s.concat(solve2(coeffs));
        //    }

        //    /* resubstitute */
        //    Single sub = 1F / 4 * A;

        //    for (let i = 0; i < s.length; ++i)
        //        s[i] -= sub;

        //    return s;
        //}



        //    hit(ray) {
        //    let tfRay = this.transformation.transformRay(ray);

        //    let t = this.findIntersection(tfRay);
        //    if (t === null)
        //        return null;

        //    let localHitPoint = tfRay.pointAtDistance(t);
        //    let localNormal = this.computeNormalAtPoint(localHitPoint);

        //    return {
        //        tmin: t,
        //        hitPoint: ray.pointAtDistance(t),
        //        normal: this.transformation.transformNormal(localNormal),
        //        objectColor: this.color
        //    };
        //}

        //findIntersection(ray) {
        //    //if (!boundingBox.isIntersecting(ray))
        //    //    return null

        //    let ox = ray.origin.x;
        //    let oy = ray.origin.y;
        //    let oz = ray.origin.z;

        //    let dx = ray.direction.x;
        //    let dy = ray.direction.y;
        //    let dz = ray.direction.z;

        //    // define the coefficients of the quartic equation
        //    let sum_d_sqrd 	= dx * dx + dy * dy + dz * dz;
        //    let e			= ox * ox + oy * oy + oz * oz - 
        //                        this.sweptRadius*this.sweptRadius - this.tubeRadius*this.tubeRadius;
        //    let f			= ox * dx + oy * dy + oz * dz;
        //    let four_a_sqrd	= 4.0 * this.sweptRadius * this.sweptRadius;

        //    let coeffs = [
        //        e * e - four_a_sqrd * (this.tubeRadius*this.tubeRadius - oy * oy),
        //        4.0 * f * e + 2.0 * four_a_sqrd * oy * dy,
        //        2.0 * sum_d_sqrd * e + 4.0 * f * f + four_a_sqrd * dy * dy,
        //        4.0 * sum_d_sqrd * f,
        //        sum_d_sqrd * sum_d_sqrd];

        //    let solution = solve4(coeffs);

        //    // ray misses the torus
        //    if (!solution.length)
        //        return null;

        //    // find the smallest root greater than kEpsilon, if any
        //    // the roots array is not sorted
        //    let mint = Number.POSITIVE_INFINITY;
        //    for (let t of solution) {
        //        if ((t > K_EPSILON) && (t < mint)) {
        //            mint = t;
        //        }
        //    }

        //    return Number.isFinite(mint) ? mint : null;
        //}

        //computeNormalAtPoint(point) {
        //    let paramSquared = this.sweptRadius*this.sweptRadius + this.tubeRadius*this.tubeRadius;

        //    let x = point.x;
        //    let y = point.y;
        //    let z = point.z;
        //    let sumSquared = x * x + y * y + z * z;

        //    let tmp = new Vec3D(
        //        4.0 * x * (sumSquared - paramSquared),
        //        4.0 * y * (sumSquared - paramSquared + 2.0*this.sweptRadius*this.sweptRadius),
        //        4.0 * z * (sumSquared - paramSquared));

        //    return tmp.norm();
        //}

    }
}
