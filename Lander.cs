using SharpNeat.Phenomes;
using System;
using System.Drawing;

namespace SharpNeatLander
{
    public class Lander : INeatUnit
    {
        //public const double StartingAltitude = 2400;
        public const double ViewWidth = 1000;
        public const double StartingFuel = 1000;
        public const double MaxThrust = 6;

        //90 deg = up, CCW = positive rotation
        public const double MinRot = 30;
        public const double MaxRot = 120;
        public const double RotationRate = 30; // deg/second


        public readonly Vector2 Gravity = new Vector2(0, -3.711);

        public const double TerminalVel = -200;
        public const double CrashSpeed = -20;

        public readonly Vector2 StartPos = new Vector2(100, 100);
        public readonly Vector2 TargetPos = new Vector2(900, 150);
        public readonly Vector2 ObstaclePos = new Vector2(500, 50);
        public const double ObstacleRadius = 200;

        private bool HadCollision;
        //private int _numCollisions;
        private bool WentOutOfBounds;
        private bool OutOfFuel;

        public Vector2 Position = new Vector2(0, 0);
        public double DesiredRotation { get; set; }
        public double Rotation { get; set; }
        //public double Altitude { get; private set; }
        public Vector2 Velocity = new Vector2(0, 0);
        public double Fuel { get; private set; }
        public double Thrust { get; private set; }


        public bool Crashed => OutOfFuel || WentOutOfBounds || HadCollision;
        public bool Landed => (Vector2.Distance(TargetPos, Position) < 5) && !OutOfFuel && !WentOutOfBounds && !HadCollision;

        

        public void Start()
        {
            Position = StartPos;
            Fuel = StartingFuel;
            Rotation = 90; //up

            //CalcFitness(40, 0, 200, 20, 100);
        }
        public void Update(double deltaTime)
        {
            if (Fuel <= 0.01)
            {
                OutOfFuel = true;
                Thrust = 0;
            }

            Fuel -= Thrust * deltaTime;

            Rotation = Mathf.Lerp(Rotation, DesiredRotation, RotationRate * deltaTime);
            //Rotation = DesiredRotation;
            Rotation = Mathf.Clamp(Rotation, MinRot, MaxRot);



            Vector2 rot = Vector2.FromAngle(Rotation);

            Velocity += rot * Thrust * deltaTime;
            Velocity += Gravity * deltaTime;

            //if (Velocity.Y < TerminalVel)
            //    Velocity.Y = TerminalVel;

            Position += Velocity * deltaTime;

            if (!HadCollision)
                HadCollision = (Vector2.Distance(Position, ObstaclePos) <= ObstacleRadius);
            //if ((Vector2.Distance(Position, ObstaclePos) <= ObstacleRadius))
            //    _numCollisions++;

            if (!WentOutOfBounds)
                WentOutOfBounds = (Position.X < 0 || Position.X > ViewWidth || Position.Y < 0);

            //if (Position.Y < 0)
            //    Position.Y = 0;

        }

        public void Compute(IBlackBox box)
        {
            ISignalArray inputArr = box.InputSignalArray;
            ISignalArray outputArr = box.OutputSignalArray;

            //set inputs
            //inputArr[0] = Position.X / ViewWidth;
            //inputArr[1] = Position.Y / StartPos.Y;
            //inputArr[2] = Velocity.X / TerminalVel;
            //inputArr[3] = Velocity.Y / TerminalVel;
            //inputArr[4] = Fuel / StartingFuel;
            inputArr[0] = Vector2.Distance(Position, TargetPos) / ViewWidth;
            inputArr[1] = Vector2.Distance(Position, ObstaclePos) / ViewWidth;


            box.Activate();

            //if (outputArr[0] > 0.5)
            //    ship.Thrust = 10;
            Thrust = outputArr[0] * MaxThrust;//Math.Round(outputArr[0] * 5); //Math.Floor(outputArr[0] * 4.0);
            DesiredRotation = outputArr[1] * 360.0;
        }



        public static double StandardizedRange(double x, double origRangeA, double origRangeB, double desiredRangeA, double desiredRangeB)
        {
            //A,B = original range
            //r1,r2 = desired range
            //y = 1 + (x-A)*(r2-r1)/(B-A)
            var fitness = 1 + (x - origRangeA) * (desiredRangeB - desiredRangeA) / (origRangeB - origRangeA);
            return fitness;
        }
        /// <summary>
        /// Calculate a fitness by normalizing and scaling the value by a "weight", centered around a desired "best" value
        /// </summary>
        /// <param name="x">incoming value</param>
        /// <param name="min">value range mininum</param>
        /// <param name="max">value range maximum</param>
        /// <param name="best">best value for maximum fitness</param>
        /// <param name="weight">importance of this value compared to others</param>
        /// <returns></returns>
        public static double NormalizeFitness(double x, double min, double max, double best, double weight)
        {

            if (x < min || x > max)
                return 0;

            var offset = Math.Abs(best - x);
            var range = max - min;
            //var v = (1 + offset) * (1 / range);
            var v = 1 - (offset / range);
            var fit = v * weight;
            return fit;
        }
        /// <summary>
        /// Sigmoid function
        /// </summary>
        /// <param name="x"></param>
        /// <returns>0 to 1</returns>
        public static double Sigmoid2(double x)
        {
            return 1 / (1 + Math.Exp(-x));
        }
        public static double SigmoidDerivative2(double x)
        {
            double s = Sigmoid(x);
            return s * (1 - s);
        }
        /// <summary>
        /// Sigmoid function
        /// </summary>
        /// <param name="x"></param>
        /// <returns>-1 to 1</returns>
        public static double Sigmoid(double x)
        {
            return 2 / (1 + Math.Exp(-2 * x)) - 1;
        }
        public static double SigmoidDerivative(double x)
        {
            double s = Sigmoid(x);
            return 1 - (Math.Pow(s, 2));
        }
        public static double CalcFitness(double x, double best, double weight)
        {
            double diff = Math.Abs(best - x);
            double r = 1 - (diff / best);
            return r * weight;
        }

        public double GetFitness()
        {


            //double f = 0;//NormalizeFitness(Fuel, 0, StartingFuel, StartingFuel, 10);
            //double r = NormalizeFitness(Rotation, 0, 360, 90, 100);
            //double vx = 0;//NormalizeFitness(Velocity.X, -20, 20, 0, 10);
            //double vy = NormalizeFitness(Velocity.Y, TerminalVel, 0, CrashSpeed + 1, 100);
            //double x =  NormalizeFitness(Position.X, 0, ViewWidth, TargetPos.X, 10);

            //check out of bounds
            
            double fitness = 0;
            //fitness += 1 - (Vector2.Distance(TargetPos, Position) / 2000);
            double td = Vector2.Distance(TargetPos, Position);
            fitness += NormalizeFitness(td, 0, 2000, 0, 1);
            // fitness -= _numCollisions * 0.002;

            //fitness += NormalizeFitness(Velocity.Magnitude, 0, 100, 20, 10);
            //fitness += NormalizeFitness(Fuel, 0, StartingFuel, StartingFuel, 10);
            ;
            //double od = Vector2.Distance(Position, ObstaclePos);
            //fitness += NormalizeFitness(od, 0, 2000, ObstacleRadius + 50, 8);


            // double fitness = f + vx + vy + x + r;
            //if (d < 2)// && Math.Abs(Position.X - TargetPos.X) < 5 && Velocity.Y > CrashSpeed)// && Math.Abs(Velocity.X) < 2 && Rotation > 85 && Rotation < 95) //safe landing?
            //{

            //fitness += 100;
            //fitness += NormalizeFitness(Velocity.Magnitude, 0, 100, 1, 10);
            //big bonus for fuel savings
            //fitness += NormalizeFitness(Fuel, 0, StartingFuel, StartingFuel, 100); //StandardizedRange(Fuel, 1, StartingFuel, 1, 100);
            //bonus for soft landing
            //v = CalcFitness(Velocity, CrashSpeed + 1, -1, -2, 50);  // StandardizedRange(Velocity, CrashSpeed, -1, 1, 20);
            //fitness += f;// + v;
            //fitness += 100;
            //}
            //double nFuel = Fuel / StartingFuel;
            //double nVelocity = Velocity / TerminalVel;
            //double nAltitude = Altitude / StartingAltitude;

            //double fitness = 0;
            ////fitness = (1 - nAltitude);
            //fitness += (CrashSpeed / Velocity) * 10;
            ////fitness += nFuel * 5;
            //if (Altitude <= 2) //landed
            //{
            //    //small reward for lower velocity
            //    //fitness += (1 - nVelocity) * 5;
            //    if (Velocity > CrashSpeed) //no crash
            //    {

            //        //big reward for safe landing
            //        fitness += 100;
            //        //fuel savings bonus
            //        fitness += nFuel * 10;

            //    }

            //}
            //Console.WriteLine($"{Altitude,8:F1}{Velocity,8:F1}{Fuel,8:F1}{Thrust,8:F1}");
            return fitness > 0 ? fitness : 0;
        }


        public void Render(Graphics g)
        {
            //draw a triangle to represent lander for now


            Point[] lines = new Point[4]
            {
                FrmMain.Instance.WorldToView(new Vector2(Position.X,Position.Y + 50)),
                FrmMain.Instance.WorldToView(new Vector2(Position.X -20,Position.Y)),
                FrmMain.Instance.WorldToView(new Vector2(Position.X+20,Position.Y )),
                FrmMain.Instance.WorldToView(new Vector2(Position.X,Position.Y + 50 )),

            };
            g.DrawLines(new Pen(Color.White, 2), lines);

            Vector2 rot = Vector2.FromAngle(Rotation) * Thrust * 10;
            Point[] rotLines = new Point[2]
           {
                FrmMain.Instance.WorldToView(new Vector2(Position.X,Position.Y)),
                FrmMain.Instance.WorldToView(new Vector2(Position.X - rot.X,Position.Y-rot.Y)),

           };

            //draw a thrust line
            g.DrawLines(new Pen(Color.Gold, 2), rotLines);

            Point start = FrmMain.Instance.WorldToView(StartPos);
            Point targ = FrmMain.Instance.WorldToView(TargetPos);

            g.DrawRectangle(new Pen(Color.White), new Rectangle(start.X - 10, start.Y, 20, 5));
            g.DrawRectangle(new Pen(Color.White), new Rectangle(targ.X - 10, targ.Y, 20, 5));

            //draw the obstacle
            Point obsPoint = FrmMain.Instance.WorldToView(ObstaclePos);
            int obsRadius = (int)(ObstacleRadius * FrmMain.ViewScale);
            g.DrawEllipse(new Pen(Color.Red), new Rectangle(obsPoint.X - obsRadius, obsPoint.Y - obsRadius, obsRadius * 2, obsRadius * 2));

        }
    }
}
