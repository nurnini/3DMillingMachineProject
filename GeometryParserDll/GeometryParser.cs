using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace MillingMachineGeometryParserDll
{
    /// <summary>
    /// GeometryParser: This is the main parser which parse the 3d geometry (Currently it supports only Wavefront's obj model)
    /// </summary>
    public class GeometryParser
    {

        public List<Vertex> VertexList { get; set; }
        public List<Face> FaceList { get; set; }

        public GeometryParser()
        {
            VertexList = new List<Vertex>();
            FaceList = new List<Face>();
        }

        public void Parse3DGeometry(string folderdir)
        {

            string line;

            double firstV, secondV, thirdV;
            int firstF, secondF, thirdF;
            StreamReader sr = new StreamReader(folderdir);
            while (sr.Peek() > -1)
            {
                line = sr.ReadLine();
                if (line[0] == 'v')
                {
                    string[] parsedVertices = line.Split(' ');
                    firstV = Convert.ToDouble(parsedVertices[1]);
                    secondV = Convert.ToDouble(parsedVertices[2]);
                    thirdV = Convert.ToDouble(parsedVertices[3]);

                    Vertex myVertex = new Vertex { x = firstV, y = secondV, z = thirdV };
                    VertexList.Add(myVertex);

                }
                else if (line[0] == 'f')
                {
                    string[] parsedFaces = line.Split(' ');
                    firstF = Convert.ToInt32(parsedFaces[1]);
                    secondF = Convert.ToInt32(parsedFaces[2]);
                    thirdF = Convert.ToInt32(parsedFaces[3]);


                    Face myFace = new Face { firstIndex = firstF, secondIndex = secondF, thirdIndex = thirdF };
                    FaceList.Add(myFace);

                }
            }
            sr.Close();
        }
    }
}
