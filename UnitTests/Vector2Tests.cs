using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharpNeatLander;
using System.Diagnostics;

namespace UnitTests
{
    [TestClass]
    public class Vector2Tests
    {

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

            Assert.IsTrue(dir.Magnitude.AlmostEquals(1.0));
            Assert.IsTrue(dir.X.AlmostEquals(0.70710678118654752440084436210485));
            Assert.IsTrue(dir.Y.AlmostEquals(-0.70710678118654752440084436210485));

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
            Assert.IsTrue(Lander.NormalizeFitness(0, 0, 1000, 500, 200).AlmostEquals(100));
            Assert.IsTrue(Lander.NormalizeFitness(1000, 0, 1000, 500, 200).AlmostEquals(100));
            Assert.IsTrue(Lander.NormalizeFitness(500, 0, 1000, 500, 200).AlmostEquals(200));
        }
        [TestMethod]
        public void CalcFitnessTest()
        {
            Assert.IsTrue(Lander.CalcFitness(50, 100, 200).AlmostEquals(100));
            Assert.IsTrue(Lander.CalcFitness(50, -100, 200).AlmostEquals(100));


        }
        [TestMethod]
        public void AngleTest()
        {
            Assert.IsTrue(Vector2.Right.Angle.AlmostEquals(0));
            Assert.IsTrue(Vector2.Up.Angle.AlmostEquals(90));
            Assert.IsTrue(Vector2.Left.Angle.AlmostEquals(180));
            Assert.IsTrue(Vector2.Down.Angle.AlmostEquals(-90));
        }

        [TestMethod]
        public void DistanceTest()
        {
            double d = Vector2.Distance(new Vector2(1, 1), new Vector2(2, 2));
            Assert.IsTrue(d.AlmostEquals(1.414213562373095));
            d = Vector2.Distance(new Vector2(-1, -1), new Vector2(-2, -2));
            Assert.IsTrue(d.AlmostEquals(1.414213562373095));
            d = Vector2.Distance(new Vector2(-1, -1), new Vector2(1, 1));
            Assert.IsTrue(d.AlmostEquals(2.82842712474619));
        }
        [TestMethod]
        public void LerpTest()
        {
            Assert.IsTrue(Mathf.Lerp(0, 100, 0.5).AlmostEquals(50));
            Assert.IsTrue(Mathf.Lerp(100, 0, 0.5).AlmostEquals(50));
            double v = Mathf.Lerp(100, 0, 0.25);
            Assert.IsTrue(v.AlmostEquals(75));
            Assert.IsTrue(Mathf.Lerp(0, 100, 0.25).AlmostEquals(25));
        }

        [TestMethod]
        public void ClampTest()
        {
            Assert.IsTrue(Mathf.Clamp(0, 1, 10).AlmostEquals(1));
            Assert.IsTrue(Mathf.Clamp(11, 1, 10).AlmostEquals(10));
            Assert.IsTrue(Mathf.Clamp(5, 1, 10).AlmostEquals(5));
        }
    }
}
