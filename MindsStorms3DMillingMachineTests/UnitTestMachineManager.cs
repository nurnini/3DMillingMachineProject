using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MillingMachineCoreDll.Utilities;
using MillingMachineGeometryParserDll;

namespace MillingMachineTests
{
    [TestClass]
    public class UnitTestMachineManager
    {
        [TestMethod]
        public void FindMaximumAndMiniumValuesTest()
        {

            // Arrange
            List<Vertex> sampleVertexList = new List<Vertex>();
            sampleVertexList.Add(new Vertex { x = 3, y = 3, z = 91 });
            sampleVertexList.Add(new Vertex { x = 9, y = 4, z = 12 });
            sampleVertexList.Add(new Vertex { x = 2, y = 12, z = -3 });
            sampleVertexList.Add(new Vertex { x = -8, y = -5, z = 8 });
            sampleVertexList.Add(new Vertex { x = 4, y = -4, z = 12 });
            sampleVertexList.Add(new Vertex { x = 2, y = 0, z = 7 });


            // Act
            double maxXValue = Utility.FindMaxValue(sampleVertexList, m => m.x);
            double minXValue = Utility.FindMinValue(sampleVertexList, m => m.x);

            double maxYValue = Utility.FindMaxValue(sampleVertexList, m => m.y);
            double minYValue = Utility.FindMinValue(sampleVertexList, m => m.y);

            double maxZValue = Utility.FindMaxValue(sampleVertexList, m => m.z);
            double minZValue = Utility.FindMinValue(sampleVertexList, m => m.z);


            //Assert

            Assert.AreEqual(maxXValue, 9);
            Assert.AreEqual(minXValue, -8);
            Assert.AreEqual(maxYValue, 12);
            Assert.AreEqual(minYValue, -5);
            Assert.AreEqual(maxZValue, 91);
            Assert.AreEqual(minZValue, -3);
        }

        [TestMethod]
        public void DiscreteValueTest()
        {
            // Arrange
             
            //Act
            int value40 = Utility.GetDiscreteValue(45, 20);
            int value60 = Utility.GetDiscreteValue(51, 20);
            int value80 = Utility.GetDiscreteValue(80, 20);
            int value140 = Utility.GetDiscreteValue(133, 20);
            int value180 = Utility.GetDiscreteValue(190, 20);
            //Assert

            Assert.AreEqual(value40, 40);
            Assert.AreEqual(value60, 60);
            Assert.AreEqual(value80, 80);
            Assert.AreEqual(value140, 140);
            Assert.AreEqual(value180, 180);
        }
    }
}
