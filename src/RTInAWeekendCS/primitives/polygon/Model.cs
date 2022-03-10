using System;
using RTInAWeekendCS.material;
using RTInAWeekendCS.primitives.hit;
using RTInAWeekendCS.utils;

namespace RTInAWeekendCS.primitives.polygon
{
    public class Model : Primitive
    {
        private static readonly Single EPSILON = 1e-5F;

        public Vector3F[] Vertices { get; protected set; }
        public Int32[] Indexes { get; protected set; }
        public Vector3F[] Normals { get; protected set; }
        public Int32[] NormalIndexes { get; protected set; }
        public Triangle[] Triangles { get; protected set; }

        private BBox bbox;

        private Vector3F position = new Vector3F();
        private Vector3F edge1 = new Vector3F();
        private Vector3F edge2 = new Vector3F();
        private Vector3F pvec = new Vector3F();
        private Vector3F tvec = new Vector3F();
        private Vector3F qvec = new Vector3F();

        public Model(Vector3F[] vertices, Int32[] indexes, Vector3F[] normals, Int32[] normalIndexes, Material material)
        {
            Vertices = vertices;
            Indexes = indexes;
            Normals = normals;
            NormalIndexes = normalIndexes;
            InitBBox();

            this.Material = material;
            Record = new HitRecord();
            Record.Material = Material;
            InitTriangles();
        }

        private void InitBBox()
        {
            Single x, y, z;
            Single minX = Single.MaxValue, minY = Single.MaxValue, minZ = Single.MaxValue;
            Single maxX = Single.MinValue, maxY = Single.MinValue, maxZ = Single.MinValue;
            for (int i = 0; i < Vertices.Length; i++)
            {
                Vector3F vertice = Vertices[i];
                x = vertice.X; y = vertice.Y; z = vertice.Z;

                if (x < minX) minX = x;
                if (y < minY) minY = y;
                if (z < minZ) minZ = z;

                if (x > maxX) maxX = x;
                if (y > maxY) maxY = y;
                if (z > maxZ) maxZ = z;
            }
            bbox = new BBox(minX, maxX, minY, maxY, minZ, maxZ);
        }

        private void InitTriangles()
        {
            Int32 numT = Indexes.Length / 3;
            Triangles = new Triangle[numT];
            for (Int32 i = 0; i < numT; i++)
            {
                Int32 i3 = i * 3;
                Int32 index1 = Indexes[i3];
                Int32 index2 = Indexes[i3 + 1];
                Int32 index3 = Indexes[i3 + 2];
                Vector3F v1 = Vertices[index1];
                Vector3F v2 = Vertices[index2];
                Vector3F v3 = Vertices[index3];
                Triangles[i] = new Triangle(v1, v2, v3, index1, index2, index3, Material);
            }
        }

        public override HitRecord Hit(Ray ray, Single tMin, Single tMax)
        {
            if (bbox.Hit(ray, tMin, tMax) == null) return null;

            float duck_dist = Single.MaxValue;
            for (Int32 i = 0; i < Triangles.Length; i++)
            {
                Single dist = 0;
                Triangle triangle = Triangles[i];
                if (ray_triangle_intersect(triangle, ray.Origin, ray.Direction, ref dist) && dist < duck_dist /*&& dist<spheres_dist*/)
                {
                    duck_dist = dist;
                    Record.T = dist;
                    ////hit = ray.Origin + dir * dist;
                    Record.Position.CopyFrom(ray.PointAtParameter(dist));

                    ////N = cross(v1 - v0, v2 - v0).normalize();
                    Record.Normal.CopyFrom(triangle.Cross);
                    ////Vector.Mul(Record.Normal, triangle.Cross, -1F);
                    return Record;
                }
            }
            return null;
        }

        private bool ray_triangle_intersect(Triangle triangle, Vector3F orig, Vector3F dir, ref Single tnear)
        {
            ////Vector.Sub(edge1, point(vert(fi, 1)), point(vert(fi, 0)));
            ////Vector.Sub(edge2, point(vert(fi, 2)), point(vert(fi, 0)));
            edge1 = triangle.Edge01;
            edge2 = triangle.Edge02;
            Vector3F.Cross(pvec, dir, edge2);
            Single det = Vector3F.Dot(edge1, pvec);
            if (det < EPSILON) return false;

            ////Vector.Sub(tvec, orig, point(vert(fi, 0)));
            Vector3F.Sub(tvec, orig, triangle.Vertices[0]);
            Single u = Vector3F.Dot(tvec, pvec);
            if (u < 0 || u > det) return false;

            Vector3F.Cross(qvec, tvec, edge1);
            Single v = Vector3F.Dot(dir, qvec);
            if (v < 0 || u + v > det) return false;

            tnear = Vector3F.Dot(edge2, qvec) * (1F / det);
            return tnear > EPSILON;
        }

        Vector3F point(Int32 i)
        {
            ////assert(i >= 0 && i < nverts());
            return Vertices[i];
        }

        int vert(Int32 fi, Int32 li)
        {
            ////assert(fi >= 0 && fi < nfaces() && li >= 0 && li < 3);
            ////return faces[fi][li];
            return Indexes[fi * 3 + li];
        }
    }
}
