using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MillingMachineGeometryParserDll
{
    /// <summary>
    /// Face: This class represents the index of vertices for a triangle
    /// </summary>
    public class Face
    {
        public int firstIndex;
        public int secondIndex;
        public int thirdIndex;

        public string Serialize()
        {
            return String.Format("Face:({0}, {1}, {2})", firstIndex, secondIndex, thirdIndex);
        }
    }
}
