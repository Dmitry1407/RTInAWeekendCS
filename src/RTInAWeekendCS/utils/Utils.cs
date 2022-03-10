using System;
using System.Linq;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

using RTInAWeekendCS.primitives;
using RTInAWeekendCS.primitives.polygon;
using RTInAWeekendCS.material;

namespace RTInAWeekendCS.utils
{
    public class Utils
    {
        public static Model ReadObjFile(String filePath, Material material)
        {
            var vertices = new List<Vector3F>();
            var indexes = new List<int>();
            var normals = new List<Vector3F>();
            var normalIndexes = new List<int>();
            var reader = new StreamReader(filePath);
            while (!reader.EndOfStream)
            {
                var currentLine = reader.ReadLine();
                if (currentLine != "")
                {
                    if (currentLine[0] == 'v')
                    {
                        ReadVertex(currentLine, vertices, normals);
                    }
                    else if (currentLine[0] == 'f')
                    {
                        ReadPolygon(currentLine, indexes, normalIndexes);
                    }
                }
            }
            Model model = new Model(
                vertices.ToArray(),
                indexes.ToArray(),
                normals.ToArray(),
                normalIndexes.ToArray(),
                material);
            return model;
        }

        static void ReadVertex(String line, List<Vector3F> vertices, List<Vector3F> normals)
        {
            var nums = line.Split(' ').Skip(1)
                .Where(n => n != "")
                .Select(n => float.Parse(n, CultureInfo.InvariantCulture))
                .ToArray();

            switch (line[1])
            {
                case 't':
                    ////texCoords.Add(new Vector2(nums[0], nums[1]));
                    break;
                case 'n':
                    ////normals.Add(Vector.Normalize(new Vector(nums[0], nums[1], nums[2])));
                    Vector3F vertice = new Vector3F(nums[0], nums[1], nums[2]);
                    vertice.Normalize();
                    normals.Add(vertice);
                    break;
                default:
                    vertices.Add(new Vector3F(nums[0], nums[1], nums[2]));
                    break;
            }
        }

        static void ReadPolygon(String line, List<Int32> indexes, List<Int32> normalIndexes)
        {
            var index = line.Split(' ').Skip(1)
                .Where(l => l != "")
                .Select(l => l.Split('/').Select(n => n == "" ? 0 : Int32.Parse(n)).ToArray())
                .ToArray();

            for (Int32 i = 1; i < index.Length - 1; i++)
            {
                indexes.Add(index[0][0] - 1);
                indexes.Add(index[i][0] - 1);
                indexes.Add(index[i + 1][0] - 1);

                if (index[0].Length > 2)
                {
                    normalIndexes.Add(index[0][2] - 1);
                    normalIndexes.Add(index[i][2] - 1);
                    normalIndexes.Add(index[i + 1][2] - 1);
                }
            }
        }
    }
}
