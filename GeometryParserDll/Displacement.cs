using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MillingMachineGeometryParserDll
{
    public class Displacement 
    {
        public double x { get; set; }
        public double y { get; set; }
        public double z { get; set; }
        public Vertex SourceVertex { get; set; }
        public Vertex TargetVertex { get; set; }

        public Displacement()
        {

        }

        public string Serialize()
        {
            return String.Format("Dsp:({0}, {1}, {2})", x, y, z);
        }

        public Displacement(Vertex sourceVertex, Vertex targetVertex)
        {
            x = targetVertex.x - sourceVertex.x;
            y = targetVertex.y - sourceVertex.y;
            z = targetVertex.z - sourceVertex.z;

            SourceVertex = sourceVertex;
            TargetVertex = targetVertex;
        }
    }
}
