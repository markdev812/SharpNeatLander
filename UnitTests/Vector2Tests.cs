using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharpNeatLander;

namespace UnitTests
{
    [TestClass]
    public class Vector2Tests
    {
        public bool DoubleEquals(double d1, double d2)
        {
            double diff = d2 - d1;
            return (diff < 0.000001 && diff > -0.000001);
        }
        [TestMethod]
        public void RotateCheckAngle()
        {
            Vector2 dir = Vector2.Up;
            dir.Rotate(1); Assert.IsTrue(DoubleEquals(dir.Angle, 1));
            dir.Rotate(89); Assert.IsTrue(DoubleEquals(dir.Angle, 89));
            dir.Rotate(91); Assert.IsTrue(DoubleEquals(dir.Angle, 91));
            dir.Rotate(179); Assert.IsTrue(DoubleEquals(dir.Angle, 179));
            dir.Rotate(181); Assert.IsTrue(DoubleEquals(dir.Angle, 181));

        }
        [TestMethod]
        public void RotateLeft90Test()
        {
            Vector2 dir = Vector2.Up;
            dir.Rotate(90);

            Assert.IsTrue(DoubleEquals(dir.X, -1.0));
            Assert.IsTrue(DoubleEquals(dir.Y, 0.0));

        }
        public void RotateRight90Test()
        {
            Vector2 dir = Vector2.Up;
            dir.Rotate(-90);

            Assert.IsTrue(DoubleEquals(dir.X, 1.0));
            Assert.IsTrue(DoubleEquals(dir.Y, 0.0));

        }
        [TestMethod]
        public void RotateRight45Test()
        {
            Vector2 dir = Vector2.Up;
            dir.Rotate(-45); //right 45 deg

            Assert.IsTrue(DoubleEquals(dir.Magnitude, 1.0));
            Assert.IsTrue(DoubleEquals(dir.X, 0.70710678118654752440084436210485));
            Assert.IsTrue(DoubleEquals(dir.Y, 0.70710678118654752440084436210485));

        }
    }
}
