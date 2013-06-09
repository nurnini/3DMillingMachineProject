using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MillingMachineGeometryParserDll
{
    /// <summary>
    /// Vertex : This contains the Vertex coordinates as 3 domensional cartesian coordinate
    /// </summary>
    public class Vertex
    {
        public double x { get; set; }
        public double y { get; set; }
        public double z { get; set; }

        public string Serialize()
        {
            return String.Format("Vertex:({0}, {1}, {2})", x, y, z);
        }

        public Vertex Clone()
        {
            return new Vertex { x = this.x, y = this.y, z = this.z};
        }
    }
}
