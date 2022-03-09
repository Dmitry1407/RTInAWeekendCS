using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;

using RTInAWeekendCS.material;
using RTInAWeekendCS.material.light;
using RTInAWeekendCS.material.texture;
using RTInAWeekendCS.primitives;
using RTInAWeekendCS.primitives.hit;
using RTInAWeekendCS.primitives.plane;
using RTInAWeekendCS.utils;
using RTInAWeekendCS.primitives.polygon;

namespace RTInAWeekendCS
{
    public class MainClass
    {
        private static Random random = new Random();
        private static Ray scattered = new Ray();
        private static FColor tmpColor = new FColor();
        private static FColor traceColor = new FColor();
        private static FColor attenuation = new FColor();
        private static FColor black = new FColor(0F, 0F, 0F);
        private static FColor white = new FColor(1F, 1F, 1F);
        private static FColor gray = new FColor(0.5F, 0.5F, 0.5F);
        private static Vector3F unitDirection = new Vector3F();

        public static FColor Trace(Ray ray, Hitable scene, Int32 depth)
        {
            HitRecord record = scene.Hit(ray, 0.001F, Single.MaxValue);
            if (record != null)
            {
                Single r, g, b;
                FColor emitted = record.Material.Emitted(record.U, record.V, record.Position);
                if (depth < 50 && record.Material.Scatter(ray, record, attenuation, scattered))
                {
                    r = attenuation.R; g = attenuation.G; b = attenuation.B;
                    tmpColor.CopyFrom(Trace(scattered, scene, depth + 1));
                    traceColor.R = emitted.R + r * tmpColor.R;
                    traceColor.G = emitted.G + g * tmpColor.G;
                    traceColor.B = emitted.B + b * tmpColor.B;
                    return traceColor;
                }
                else { return emitted; }
            }
            else
            {
                Vector3F.Div(unitDirection, ray.Direction, ray.Direction.Length());
                Single t = 0.5F * (unitDirection.Y + 1F);
                traceColor.R = 1F - t + 0.5F * t;
                traceColor.G = 1F - t + 0.7F * t;
                traceColor.B = 1F;
                return traceColor;
                //return black;
            }
        }

        public static void Raster(HitList scene, int width, int height)
        {
            BColor bwhite = new BColor(160, 160, 160);
            BColor bblack = new BColor(60, 60, 60);
            ////DrawLine(13, 20, 80, 40, bwhite);
            ////DrawLine(20, 13, 40, 80, bwhite);

            foreach (Hitable obj in scene.GetList())
            {
                if (obj is Model)
                {
                    Model model = (Model)obj;
                    foreach (Triangle triangle in model.Triangles)
                    {
                        //bwhite.R = (Byte)(random.NextDouble() * 255.99);
                        //bwhite.G = (Byte)(random.NextDouble() * 255.99);
                        //bwhite.B = (Byte)(random.NextDouble() * 255.99);
                        //bwhite.R = bwhite.G = bwhite.B = (Byte)(random.NextDouble() * 255.99);

                        float intensity = (float)Math.Abs(Math.Pow(triangle.Cross.Z, 2));
                        bwhite.R = bwhite.G = bwhite.B = (Byte)(255.99 * intensity);

                        DrawTriangleFace(width, height, triangle, bwhite);
                        ////DrawTriangleWire(width, height, triangle, bblack);
                    }
                }
            }
        }

        private static void DrawTriangleWire(int width, int height, Triangle triangle, BColor bcolor)
        {
            DrawLine(width, height, triangle.Vertices[0], triangle.Vertices[1], bcolor);
            DrawLine(width, height, triangle.Vertices[1], triangle.Vertices[2], bcolor);
            DrawLine(width, height, triangle.Vertices[2], triangle.Vertices[0], bcolor);
        }

        private static void DrawTriangleFace(int width, int height, Triangle triangle, BColor bcolor)
        {
            Int32 x0 = (Int32)((triangle.Vertices[0].X + 1F) * width / 2F); //x0 = Math.Max(x0, 0); x0 = Math.Min(x0, width - 1);
            Int32 y0 = (Int32)((triangle.Vertices[0].Y + 1F) * height / 2F); //y0 = Math.Max(y0, 0); y0 = Math.Min(y0, height - 1);
            Int32 z0 = (Int32)((triangle.Vertices[0].Z + 1F) * DEPTH / 2F); //z0 = Math.Max(z0, 0); z0 = Math.Min(z0, DEPTH);

            Int32 x1 = (Int32)((triangle.Vertices[1].X + 1F) * width / 2F); //x1 = Math.Max(x1, 0); x1 = Math.Min(x1, width - 1);
            Int32 y1 = (Int32)((triangle.Vertices[1].Y + 1F) * height / 2F); //y1 = Math.Max(y1, 0); y1 = Math.Min(y1, height - 1);
            Int32 z1 = (Int32)((triangle.Vertices[1].Z + 1F) * DEPTH / 2F); //z1 = Math.Max(z1, 0); z1 = Math.Min(z1, DEPTH);

            Int32 x2 = (Int32)((triangle.Vertices[2].X + 1F) * width / 2F); //x2 = Math.Max(x2, 0); x2 = Math.Min(x2, width - 1);
            Int32 y2 = (Int32)((triangle.Vertices[2].Y + 1F) * height / 2F); //y2 = Math.Max(y2, 0); y2 = Math.Min(y2, height - 1);
            Int32 z2 = (Int32)((triangle.Vertices[2].Z + 1F) * DEPTH / 2F); //z2 = Math.Max(z2, 0); z2 = Math.Min(z2, DEPTH);

            v0.SetXYZ(x0, y0, z0);
            v1.SetXYZ(x1, y1, z1);
            v2.SetXYZ(x2, y2, z2);
            DrawTriangleFace(v0, v1, v2, bcolor);
        }

        private static Vector3I v0 = new Vector3I();
        private static Vector3I v1 = new Vector3I();
        private static Vector3I v2 = new Vector3I();
        private static Vector3I A = new Vector3I();
        private static Vector3I B = new Vector3I();
        private static Vector3I P = new Vector3I();

        private static void DrawTriangleFace(Vector3I t0, Vector3I t1, Vector3I t2, BColor bcolor)
        {
            Vector3I C;
            ////v0.CopyFrom(t0); v1.CopyFrom(t1); v2.CopyFrom(t2);

            // I dont care about degenerate triangles
            // sort the vertices, t0, t1, t2 lower-to-upper (bubblesort yay!)
            if (v0.Y == v1.Y && v0.Y == v2.Y) return;

            if (v0.Y > v1.Y) { C = v0; v0 = v1; v1 = C; }////std::swap(t0, t1);             
            if (v0.Y > v2.Y) { C = v0; v0 = v2; v2 = C; }////std::swap(t0, t2); 
            if (v1.Y > v2.Y) { C = v1; v1 = v2; v2 = C; }////std::swap(t1, t2); 

            Single total_height = v2.Y - v0.Y;
            for (Int32 i = 0; i < total_height; i++)
            {
                bool second_half = i > v1.Y - v0.Y || v1.Y == v0.Y;
                float segment_height = second_half ? v2.Y - v1.Y : v1.Y - v0.Y;

                Single alpha = (float)i / total_height;
                // be careful: with above conditions no division by zero here
                Single beta = (float)(i - (second_half ? v1.Y - v0.Y : 0)) / segment_height;

                ////A = alpha * (v2 - v0) + v0;
                Vector3I.Sub(A, v2, v0); A.Mul(alpha); A.Add(v0);
                ////B = second_half ? beta * (v2 - v1) + v1 : beta * (v1 - v0) + v0;
                if (second_half) { Vector3I.Sub(B, v2, v1); B.Mul(beta); B.Add(v1); }
                else { Vector3I.Sub(B, v1, v0); B.Mul(beta); B.Add(v0); }

                ////std::swap(A, B);
                if (A.X > B.X) { C = A; ; A = B; B = C; }

                for (Int32 j = A.X; j <= B.X; j++)
                {   // attention, due to int casts t0.y+i != A.y
                    //imgBuff.SetPX(j, v0.Y + i, bcolor);

                    Single phi = B.X == A.X ? 1F : (float)(j - A.X) / (float)(B.X - A.X);
                    ////P = A + phi * (B - A);
                    Vector3I.Sub(P, B, A); P.Mul(phi); P.Add(A);
                    Int32 idx = P.X + P.Y * imgBuff.Width;
                    if (zBuff[idx] < P.Z)
                    {
                        zBuff[idx] = P.Z;
                        imgBuff.SetPX(P.X, P.Y, bcolor);
                    }
                }
            }
        }

        private static void DrawLine(int width, int height, Vector3F p0, Vector3F p1, BColor bcolor)
        {
            //int x0 = (int)((p0.X + 1F) * width / 6F); x0 = Math.Max(x0, 0); x0 = Math.Min(x0, width - 1);
            //int y0 = (int)((p0.Y + 3F) * height / 4F); y0 = Math.Max(y0, 0); y0 = Math.Min(y0, height - 1);
            //int x1 = (int)((p1.X + 1F) * width / 6F); x1 = Math.Max(x1, 0); x1 = Math.Min(x1, width - 1);
            //int y1 = (int)((p1.Y + 3F) * height / 4F); y1 = Math.Max(y1, 0); y1 = Math.Min(y1, height - 1);

            int x0 = (int)((p0.X + 1F) * width / 2F); x0 = Math.Max(x0, 0); x0 = Math.Min(x0, width - 1);
            int y0 = (int)((p0.Y + 1F) * height / 2F); y0 = Math.Max(y0, 0); y0 = Math.Min(y0, height - 1);
            int x1 = (int)((p1.X + 1F) * width / 2F); x1 = Math.Max(x1, 0); x1 = Math.Min(x1, width - 1);
            int y1 = (int)((p1.Y + 1F) * height / 2F); y1 = Math.Max(y1, 0); y1 = Math.Min(y1, height - 1);

            DrawLine(x0, y0, x1, y1, bcolor);
        }

        private static void DrawLine(Int32 x0, Int32 y0, Int32 x1, Int32 y1, BColor bcolor)
        {
            Int32 swap;
            bool steep = false;
            if (Math.Abs(x0 - x1) < Math.Abs(y0 - y1))
            {
                swap = x0; x0 = y0; y0 = swap; ////Swap(x0, y0);
                swap = x1; x1 = y1; y1 = swap; ////Swap(x1, y1);
                steep = true;
            }
            if (x0 > x1)
            {
                swap = x0; x0 = x1; x1 = swap; ////Swap(x0, x1);
                swap = y0; y0 = y1; y1 = swap; ////Swap(y0, y1);
            }
            for (Int32 x = x0; x <= x1; x++)
            {
                float t = (x - x0) / (float)(x1 - x0);
                Int32 y = (Int32)(y0 * (1F - t) + y1 * t);

                if (steep) { imgBuff.SetPX(y, x, bcolor); }
                else { imgBuff.SetPX(x, y, bcolor); }
            }
        }

        private static Hitable RandomScene()
        {
            Int32 i = 0;
            Int32 n = 90;
            Hitable[] list = new Hitable[n + 1];

            // Ground
            list[i++] = new Sphere(new Vector3F(0F, -1000F, 0F), 1000F, new Lambertian(new TDiffuse(new FColor(0.5F, 0.5F, 0.5F))));
            ////Texture checker = new TChecker(new TDiffuse(new Color(0.2F, 0.3F, 0.1F)), new TDiffuse(new Color(0.9F, 0.9F, 0.9F)));
            ////list[i++] = new Sphere(new Vector(0F, -1000F, 0F), 1000F, new Lambertian(checker));

            // Random spheres
            //Vector center;
            //Vector correct = new Vector(4F, 0.2F, 0F);
            //Vector result = new Vector(0F, 0F, 0F);
            //for (Int32 a = -4; a < 4; a++)
            //{
            //    for (Int32 b = -4; b < 4; b++)
            //    {
            //        Single chooseMat = (Single)random.NextDouble();
            //        center = new Vector(2 * a + 1.9F * (Single)random.NextDouble(), 0.2F, 2 * b + 1.9F * (Single)random.NextDouble());
            //        Vector.Sub(result, center, correct);
            //        ////if ((center - new Vector(4F, 0.2F, 0F)).length() > 0.9F)
            //        if (result.Length() > 0.9F)
            //        {
            //            if (chooseMat < 0.6)
            //            {   // diffuse
            //                list[i++] = new Sphere(center, 0.2F, new Lambertian(new Color((Single)random.NextDouble(), (Single)random.NextDouble(), (Single)random.NextDouble())));
            //            }
            //            else if (chooseMat < 0.9)
            //            {   // metal
            //                center.Y = 0.3F;
            //                list[i++] = new Sphere(center, 0.3F, new Metal(new Color((Single)random.NextDouble(), (Single)random.NextDouble(), (Single)random.NextDouble()), (Single)random.NextDouble()));
            //            }
            //            else
            //            {   // glass
            //                center.Y = 0.4F;
            //                list[i++] = new Sphere(center, 0.4F, new Glass(1.5F));
            //            }
            //        }
            //    }
            //}

            // Central spheres
            list[i++] = new Sphere(new Vector3F(-4F, 1F, 0F), 1F, new Lambertian(new TDiffuse(new FColor(0.4F, 0.2F, 0.1F))));
            list[i++] = new Sphere(new Vector3F(0F, 1F, 0F), 1F, new Glass(1.5F));
            list[i++] = new Sphere(new Vector3F(4F, 1F, 0F), 1F, new Metal(new FColor(0.7F, 0.6F, 0.5F), 0F));

            return new HitList(list, i);
        }

        private static Hitable TwoSpheres()
        {
            Hitable[] list = new Hitable[2];
            Texture checker = new TChecker(new TDiffuse(new FColor(0.2F, 0.3F, 0.1F)), new TDiffuse(new FColor(0.9F, 0.9F, 0.9F)));
            list[0] = new Sphere(new Vector3F(0F, -10F, 0F), 10F, new Lambertian(checker));
            list[1] = new Sphere(new Vector3F(0F, 10F, 0F), 10F, new Lambertian(checker));
            return new HitList(list, 2);
        }

        private static Hitable Cylinder()
        {
            Hitable[] list = new Hitable[2];
            ////Texture checker = new TChecker(new TDiffuse(new Color(0.2F, 0.3F, 0.1F)), new TDiffuse(new Color(0.9F, 0.9F, 0.9F)));
            list[0] = new Sphere(new Vector3F(0F, -1000F, 0F), 1000F, new Lambertian(new TDiffuse(new FColor(0.5F, 0.5F, 0.5F))));
            list[1] = new Cylinder(0F, 1F, 1F, new Lambertian(new TDiffuse(new FColor(0.4F, 0.2F, 0.1F))));
            ////list[1] = new Cylinder(0F, 1F, 1F, new Glass(1.5F));
            ////list[1] = new Cylinder(0F, 1F, 1F, new Metal(new Color(0.7F, 0.6F, 0.5F), 0F));
            return new HitList(list, 2);
        }

        private static Hitable Torus()
        {
            Hitable[] list = new Hitable[2];
            ////Texture checker = new TChecker(new TDiffuse(new Color(0.2F, 0.3F, 0.1F)), new TDiffuse(new Color(0.9F, 0.9F, 0.9F)));
            list[0] = new Sphere(new Vector3F(0F, -1000F, 0F), 1000F, new Lambertian(new TDiffuse(new FColor(0.5F, 0.5F, 0.5F))));
            list[1] = new Torus(new Vector3F(0F, 1F, 0F), 2F, 0.4F, new Lambertian(new TDiffuse(new FColor(0.4F, 0.2F, 0.1F))));
            ////list[1] = new Torus(new Vector(0F, 1F, 0F), 2F, 0.3F, new Glass(1.5F));
            ////list[1] = new Torus(new Vector(0F, 1F, 0F), 2F, 0.3F, new Metal(new Color(0.7F, 0.6F, 0.5F), 0F));
            return new HitList(list, 2);
        }

        private static Hitable Duck()
        {
            Hitable[] list = new Hitable[2];
            list[0] = new Sphere(new Vector3F(0F, -1005F, 0F), 1000F, new Lambertian(new TDiffuse(new FColor(0.5F, 0.5F, 0.5F))));
            ////list[1] = Utils.ReadObjFile("duck.obj", new Lambertian(new TDiffuse(new FColor(0.4F, 0.2F, 0.1F))));
            ////list[1] = Utils.ReadObjFile("MyDiamond.obj", new Lambertian(new TDiffuse(new FColor(1F, 1F, 1F))));
            list[1] = Utils.ReadObjFile("african_head.obj", new Lambertian(new TDiffuse(new FColor(1F, 1F, 1F))));
            ////list[1] = Utils.ReadObjFile("MyDiamond.obj", new Glass(1.5F));
            ////list[1] = Utils.ReadObjFile("MyDiamond.obj", new Metal(new FColor(0.7F, 0.6F, 0.5F), 0F));
            return new HitList(list, 2);
        }

        private static Hitable SimpleLight()
        {
            Hitable[] list = new Hitable[3];
            Texture checker = new TChecker(new TDiffuse(new FColor(0.2F, 0.3F, 0.1F)), new TDiffuse(new FColor(0.9F, 0.9F, 0.9F)));
            list[0] = new Sphere(new Vector3F(0F, -1000F, 0F), 1000F, new Lambertian(checker));
            ////list[1] = new Sphere(new Vector(0F, 0.5F, 0F), 0.5F, new Lambertian(new TDiffuse(new Color(0.4F, 0.2F, 0.1F))));
            list[1] = new Sphere(new Vector3F(0F, 0.5F, 0F), 0.5F, new Glass(1.5F));
            list[2] = new Sphere(new Vector3F(-1F, 2F, -1F), 0.5F, new Light(new TDiffuse(new FColor(4F, 4F, 4F))));
            return new HitList(list, 3);
        }

        private static Hitable CornellBox()
        {
            Hitable[] list = new Hitable[8];
            int i = 0;
            Material red = new Lambertian(new TDiffuse(new FColor(0.65F, 0.05F, 0.05F)));
            Material white = new Lambertian(new TDiffuse(new FColor(0.73F, 0.73F, 0.73F)));
            Material green = new Lambertian(new TDiffuse(new FColor(0.12F, 0.45F, 0.15F)));
            Material light = new Light(new TDiffuse(new FColor(15F, 15F, 15F)));

            Plane plane1 = new YZPlane(0, 555, 0, 555, 555, green);
            plane1.FlipNormales = true;
            list[i++] = plane1;
            list[i++] = new YZPlane(0, 555, 0, 555, 0, red);
            list[i++] = new XZPlane(213, 343, 227, 332, 554, light);
            Plane plane2 = new XZPlane(0, 555, 0, 555, 555, white);
            plane2.FlipNormales = true;
            list[i++] = plane2;
            list[i++] = new XZPlane(0, 555, 0, 555, 0, white);
            Plane plane3 = new XYPlane(0, 555, 0, 555, 555, white);
            plane3.FlipNormales = true;
            list[i++] = plane3;

            ////list[i++] = new Box(new Vector(130, 0, 65), new Vector(295, 165, 230), white);
            ////list[i++] = new Box(new Vector(265, 0, 295), new Vector(430, 330, 460), white);
            list[i++] = new Box(130, 295, 0, 165, 65, 230, white);
            list[i++] = new Box(265, 430, 0, 330, 295, 460, white);

            return new HitList(list, i);
        }

        private const Int32 DEPTH = 255;
        private static ImgBuff imgBuff;
        private static Int32[] zBuff;

        public static void Main(string[] args)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            Int32 numX = 640;
            Int32 numY = 480;
            Int32 numSamples = 8;
            ////Byte[] imgBuff;
            ////Int32 buffCounter = 0;

            ////// MultiThread
            ////Int32 th_cnt = Environment.ProcessorCount;// Запуск потоков
            ////var threads = new Thread[th_cnt];
            ////for (Int32 i = 0; i < threads.Length; i++)
            ////{
            ////    threads[i] = new Thread(new ThreadStart(new Calc().CalcPart));
            ////    threads[i].Start();
            ////}
            ////// Ожидание завершения
            ////for (Int32 i = 0; i < threads.Length; i++)
            ////{
            ////    threads[i].Join();
            ////}

            // Create scene
            ////Hitable scene = RandomScene();
            ////Hitable scene = Cylinder();
            ////Hitable scene = Torus();
            Hitable scene = Duck();
            ////Hitable scene = TwoSpheres();
            ////Hitable scene = SimpleLight();
            ////Hitable scene = CornellBox();

            ////Create camera for RandomScene
            //Vector lookfrom = new Vector(13F, 2F, 3F);
            //Vector lookat = new Vector(0F, 0F, 0F);
            //Single distToFocus = 10F;
            //Single aperture = 0.1F;
            //Single vfov = 20F;

            // Create camera for CornellBox
            //Vector lookfrom = new Vector(278F, 278F, -800F);
            //Vector lookat = new Vector(278F, 278F, 0F);
            //Single distToFocus = 10F;
            //Single aperture = 0.01F;
            //Single vfov = 40F;

            // Create camera for Duck
            ////Vector lookfrom = new Vector(0F, 12F, -20F);
            ////Vector lookat = new Vector(6F, -6F, -6F);
            Vector3F lookfrom = new Vector3F(0F, 0.4F, 6F);
            Vector3F lookat = new Vector3F(0F, 0F, 0F);
            Single distToFocus = 6.1F;
            Single aperture = 0.1F;
            Single vfov = 20F;

            Camera camera = new Camera(lookfrom, lookat, new Vector3F(0F, 1F, 0F), vfov, (Single)numX / (Single)numY, aperture, distToFocus);

            if (args.Length == 3)
            {
                numX = Int32.Parse(args[0]);
                numY = Int32.Parse(args[1]);
                numSamples = Int32.Parse(args[2]);
            }
            ////imgBuff = new Byte[numX * numY * 3];
            imgBuff = new ImgBuff(numX, numY);

            // Save to file in current directory
            String path = System.Reflection.Assembly.GetExecutingAssembly().Location;
            String directory = System.IO.Path.GetDirectoryName(path);
            String filePath = directory + String.Format(@"\[{0}x{1}x{2}].ppm", numX, numY, numSamples);

            using (FileStream writer = new FileStream(filePath, FileMode.Create))
            {
                // Write file Header
                String header = String.Format("P6\n{0} {1}\n255\n", numX, numY);
                foreach (Char ch in header.ToCharArray()) { writer.WriteByte((Byte)ch); }

                // Write file Content
                FColor color;
                Single r, g, b;
                Int32 yRound = numY / 20;
                Console.WriteLine("");

                //for (Int32 y = numY - 1; y >= 0; y--)
                //{
                //    for (Int32 x = 0; x < numX; x++)
                //    {
                //        r = 0F; g = 0F; b = 0F;
                //        if (numSamples == 0)
                //        {
                //            // No AntiAliasing
                //            Single u = (Single)x / (Single)numX;
                //            Single v = (Single)y / (Single)numY;
                //            Ray ray = camera.GetSingleRay(u, v);
                //            color = Trace(ray, scene, 0);
                //            r = (Single)Math.Sqrt(color.R);
                //            g = (Single)Math.Sqrt(color.G);
                //            b = (Single)Math.Sqrt(color.B);
                //        }
                //        else
                //        {
                //            // AntiAliasing ON
                //            for (Int32 sample = 0; sample < numSamples; sample++)
                //            {
                //                Single u = (x + (Single)random.NextDouble()) / (Single)numX;
                //                Single v = (y + (Single)random.NextDouble()) / (Single)numY;
                //                Ray ray = camera.GetAARay(u, v);
                //                color = Trace(ray, scene, 0);
                //                r += color.R; g += color.G; b += color.B;
                //            }
                //            r = (Single)Math.Sqrt(r / (Single)numSamples);
                //            g = (Single)Math.Sqrt(g / (Single)numSamples);
                //            b = (Single)Math.Sqrt(b / (Single)numSamples);
                //        }
                //        ////imgBuff[buffCounter++] = r > 1F ? Byte.MaxValue : (Byte)(r * 255.99F);
                //        ////imgBuff[buffCounter++] = g > 1F ? Byte.MaxValue : (Byte)(g * 255.99F);
                //        ////imgBuff[buffCounter++] = b > 1F ? Byte.MaxValue : (Byte)(b * 255.99F);
                //        imgBuff.SetPX(x, y, r, g, b);
                //    }
                //    // Print TimeLine in console
                //    if (y % yRound == 0) { Console.Write(':'); }
                //}

                zBuff = new Int32[numX * numY];
                for (Int32 i = 0; i < numX * numY; i++) { zBuff[i] = Int32.MinValue; }
                Raster((HitList)scene, numX, numY);

                imgBuff.FlipVertically();
                writer.Write(imgBuff.GetBuff(), 0, imgBuff.GetBuff().Length);
            }
            sw.Stop();
            Console.WriteLine("\n\nTime: {0}h {1}m {2}.{3}s", sw.Elapsed.Hours, sw.Elapsed.Minutes, sw.Elapsed.Seconds, sw.Elapsed.Milliseconds);
        }
    }
}
