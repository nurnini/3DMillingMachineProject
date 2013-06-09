using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MillingMachineGeometryParserDll;
using MillingMachineCoreDll.Utilities;
using System.IO;


namespace MillingMachineCoreDll
{
    public class MillingMachineManager
    {
        public GeometryParser ObjectGeomertyParser { get; set; }

        public string GeometryPath { get; set; }

        public int MillingMachineResolution { get; set; }

        public MillingMachineManager(string geometryPath)
        {
            GeometryPath = geometryPath;
            ObjectGeomertyParser = new GeometryParser();
            ObjectGeomertyParser.Parse3DGeometry(geometryPath);

        }


        public List<Vertex> GetNormalisedVertexList()
        {

            List<Vertex> normalisedList = ObjectGeomertyParser.VertexList;


            double maxXValue = Utility.FindMaxValue(normalisedList, m => m.x);
            double minXValue = Utility.FindMinValue(normalisedList, m => m.x);

            double maxYValue = Utility.FindMaxValue(normalisedList, m => m.y);
            double minYValue = Utility.FindMinValue(normalisedList, m => m.y);

            double maxZValue = Utility.FindMaxValue(normalisedList, m => m.z);
            double minZValue = Utility.FindMinValue(normalisedList, m => m.z);

            double xValueSpan = Math.Abs(maxXValue - minXValue);
            double yValueSpan = Math.Abs(maxYValue - minYValue);
            double zValueSpan = Math.Abs(maxZValue - minZValue);

            double percentXIndex = 0.4;
            double percentYIndex = 0.6; // Motor A
            double percentZIndex = 0.4;

            foreach (Vertex item in normalisedList)
            {
                item.x = ((item.x - minXValue) / xValueSpan) * percentXIndex;
            }

            foreach (Vertex item in normalisedList)
            {
                item.y =  ((item.y - minYValue) / yValueSpan) * percentYIndex;
            }

            foreach (Vertex item in normalisedList)
            {
                item.z = ((item.z - minZValue) / zValueSpan) * percentZIndex;
            }


            return normalisedList;
        }


        public List<Vertex> GetVertexListForMachineCoordinates()
        {

            List<Vertex> machineCoordinateList = ObjectGeomertyParser.VertexList;


            double maxXValue = Utility.FindMaxValue(machineCoordinateList, m => m.x);
            double minXValue = Utility.FindMinValue(machineCoordinateList, m => m.x);

            double maxYValue = Utility.FindMaxValue(machineCoordinateList, m => m.y);
            double minYValue = Utility.FindMinValue(machineCoordinateList, m => m.y);

            double maxZValue = Utility.FindMaxValue(machineCoordinateList, m => m.z);
            double minZValue = Utility.FindMinValue(machineCoordinateList, m => m.z);

            double xValueSpan = Math.Abs(maxXValue - minXValue);
            double yValueSpan = Math.Abs(maxYValue - minYValue);
            double zValueSpan = Math.Abs(maxZValue - minZValue);

            double percentXIndex = 160;
            double percentYIndex = 160; // Motor A
            double percentZIndex = 160;

            foreach (Vertex item in machineCoordinateList)
            {
                item.x = Math.Round(((item.x - minXValue) / xValueSpan) * percentXIndex);
            }

            foreach (Vertex item in machineCoordinateList)
            {
                item.y =  160 - Math.Round(((item.y - minYValue) / yValueSpan) * percentYIndex);
            }

            foreach (Vertex item in machineCoordinateList)
            {
                item.z = Math.Round(((item.z - minZValue) / zValueSpan) * percentZIndex);
            }


            return machineCoordinateList;
        }

      
        public List<Displacement> GetVertexCalibrationListForMachineCoordinatesAsDisplacement()
        {

            List<Displacement> displacementList = new List<Displacement>();
            displacementList.Add(new Displacement { x = 20, y = 120, z = 20});
            displacementList.Add(new Displacement { x = 20, y = 100, z = 20});
            displacementList.Add(new Displacement { x = 20, y = 40, z = 20});
            displacementList.Add(new Displacement { x = 20, y = 60, z = 20});
            displacementList.Add(new Displacement { x = 20, y = 60, z = 20});
            displacementList.Add(new Displacement { x = 20, y = 30, z = 20});

            displacementList.Add(new Displacement { x = -20, y = 30, z = -20});
            displacementList.Add(new Displacement { x = -20, y = 40, z = -20});
            displacementList.Add(new Displacement { x = -20, y = 50, z = -20});
            displacementList.Add(new Displacement { x = -20, y = 50, z = -20});
            displacementList.Add(new Displacement { x = -20, y = 60, z = -20});
            displacementList.Add(new Displacement { x = -20, y = 90, z = -20});


            //displacementList.Add(new Displacement { x = -180, y = -180, z = -180 });
            //displacementList.Add(new Displacement { x = -120, y = -120, z = -120 });
            //displacementList.Add(new Displacement { x = 100, y = 100, z = 100 });
            //displacementList.Add(new Displacement { x = 200, y = 200, z = 200 });
            //displacementList.Add(new Displacement { x = -300, y = -300, z = -300 });


            return displacementList;
        }


        public List<Displacement> GetVertexListForMachineCoordinatesAsDisplacement()
        {

            List<Vertex> machineCoordinateList = ObjectGeomertyParser.VertexList;


            double maxXValue = Utility.FindMaxValue(machineCoordinateList, m => m.x);
            double minXValue = Utility.FindMinValue(machineCoordinateList, m => m.x);

            double maxYValue = Utility.FindMaxValue(machineCoordinateList, m => m.y);
            double minYValue = Utility.FindMinValue(machineCoordinateList, m => m.y);

            double maxZValue = Utility.FindMaxValue(machineCoordinateList, m => m.z);
            double minZValue = Utility.FindMinValue(machineCoordinateList, m => m.z);

            double xValueSpan = Math.Abs(maxXValue - minXValue);
            double yValueSpan = Math.Abs(maxYValue - minYValue);
            double zValueSpan = Math.Abs(maxZValue - minZValue);

            double percentXIndex = 160;
            double percentYIndex = 160; // Motor A
            double percentZIndex = 160;

            int step = 5;

            WriteVertexToFile(machineCoordinateList, "original_vertext2.txt");

            foreach (Vertex item in machineCoordinateList)
            {
                item.x = Math.Round(((item.x - minXValue) / xValueSpan) * percentXIndex);
               // item.x = Utility.GetDiscreteValue(item.x, step);
            }

            foreach (Vertex item in machineCoordinateList)
            {
                item.z = Math.Round(((item.z - minZValue) / zValueSpan) * percentZIndex);
                //item.z = Utility.GetDiscreteValue(item.z, step);
            }

            // Now further discretize x and z ccordinates, then order y accordingly as displacement

            foreach (Vertex item in machineCoordinateList)
            {
                item.y = 160 - Math.Round(((item.y - minYValue) / yValueSpan) * percentYIndex);
                //item.y = Utility.GetDiscreteValue(item.y, step);
            }

            WriteVertexToFile(machineCoordinateList, "normalised_vertext2.txt");

           // machineCoordinateList = machineCoordinateList.OrderBy(e => e.x).ThenBy(e => e.z).ThenBy(e => e.y).ToList<Vertex>();


           // WriteVertexToFile(machineCoordinateList, "ordered_vertext.txt");

            Vertex previousVertex = new Vertex() {x = 0, y = 0, z = 0};

            List<Displacement> displacementList = new List<Displacement>();

            foreach (Vertex vertex in machineCoordinateList)
            {
                Displacement currentDisplacement = new Displacement(previousVertex, vertex);
                previousVertex = vertex;
                displacementList.Add(currentDisplacement);   
            }

          /* var orderedVertexList =  from m in machineCoordinateList
            orderby m.x, m.z descending, m.y
            select m;

            machineCoordinateList = orderedVertexList.ToList<Vertex>(); */

            StreamWriter sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + @"\displacement1.txt");

            foreach (Displacement d in displacementList)
            {
                sw.WriteLine(d.Serialize());
            }

            sw.Close();

            return displacementList;
        }


        private void WriteVertexToFile(List<Vertex> vList, string fileName)
        {
            StreamWriter sw = new StreamWriter(AppDomain.CurrentDomain.BaseDirectory + fileName);

            foreach (Vertex d in vList)
            {
                sw.WriteLine(d.Serialize());
            }

            sw.Close();

        }


        public List<Vertex> GetNormalisedAndReducedVertexList()
        {
            List<Vertex> normalisedList = GetNormalisedVertexList().Where(p => (double)p.x < 0.5).ToList();

            return normalisedList;
        }


        public List<Face> GetReducedVertexList()
        {
            List<Vertex> normalisedVertexList = GetNormalisedVertexList();
            List<Face> reducedFaceList = ObjectGeomertyParser.FaceList.Where(f => normalisedVertexList[f.firstIndex - 1].x < 0.5).ToList();

            return reducedFaceList;
        }

        
    }
}
