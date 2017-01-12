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
        public void RotateLeft90Test()
        {
            Vector2 dir = Vector2.FromAngle(90);

            Assert.IsTrue(dir == Vector2.Up);

        }
        [TestMethod]
        public void RotateRight90Test()
        {
            Vector2 dir = Vector2.FromAngle(-90);

            Assert.IsTrue(dir == Vector2.Down);

        }
        [TestMethod]
        public void RotateRight45Test()
        {
            Vector2 dir = Vector2.FromAngle(-45); //right 45 deg

            Assert.IsTrue(DoubleEquals(dir.Magnitude, 1.0));
            Assert.IsTrue(DoubleEquals(dir.X, 0.70710678118654752440084436210485));
            Assert.IsTrue(DoubleEquals(dir.Y, -0.70710678118654752440084436210485));

        }

        [TestMethod]
        public void VelocityTest()
        {
            Vector2 g = Vector2.Up * -3.711;

            Vector2 rot = Vector2.FromAngle(90);

            Vector2 vel = rot * 4;
            vel += g;

            Assert.IsTrue(vel == new Vector2(0, 0.289));
        }
        [TestMethod]
        public void NormalizeFitnessTest()
        {
            Assert.IsTrue(DoubleEquals(100, Lander.NormalizeFitness(0, 0, 1000, 500, 200)));
            Assert.IsTrue(DoubleEquals(100, Lander.NormalizeFitness(1000, 0, 1000, 500, 200)));
            Assert.IsTrue(DoubleEquals(200, Lander.NormalizeFitness(500, 0, 1000, 500, 200)));
        }
    }
}
