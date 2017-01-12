using System;

namespace SharpNeatLander
{
    public class Vector2
    {
        public readonly static Vector2 Zero = new Vector2(0, 0);
        public readonly static Vector2 Up = new Vector2(0, 1);
        public readonly static Vector2 Down = new Vector2(0, -1);
        public readonly static Vector2 Left = new Vector2(-1, 0);
        public readonly static Vector2 Right = new Vector2(1, 0);
        public readonly static Vector2 One = new Vector2(1, 1);

        public const double Deg2Rad = (Math.PI * 2.0) / 360.0;
        public const double Rad2Deg = 360.0 / (Math.PI * 2.0);


        public double X { get; set; }
        public double Y { get; set; }

        public double Magnitude { get { return Length; } }

        public double Length { get { return Math.Sqrt(SquaredLength); } }

        public double SquaredLength
        {
            get
            {
                return X * X + Y * Y;
            }
        }

        public double Angle
        {
            get
            {
                return Math.Atan2(Y, X) * Rad2Deg;
                
            }
        }
        public Vector2(double x, double y)
        {
            X = x;
            Y = y;
        }
        public override bool Equals(object obj)
        {
            if (Object.ReferenceEquals(obj, null))
            {
                return false;
            }

            if (Object.ReferenceEquals(this, obj))
            {
                return true;
            }

            if (this.GetType() != obj.GetType())
                return false;

            Vector2 other = ((Vector2)obj);

            return (X.AlmostEquals(other.X) && Y.AlmostEquals(other.Y));
        }
        public override int GetHashCode()
        {
            return X.GetHashCode() ^ Y.GetHashCode();
        }
        public override string ToString()
        {
            return "(" + X + ", " + Y + ")";
        }

        public void Normalize()
        {
            double len = Length;
            X = X / len;
            Y = Y / len;
        }

        public static Vector2 FromAngle(double degrees)
        {
            degrees *= Deg2Rad;
            return new Vector2(Math.Cos(degrees), Math.Sin(degrees));

            //double r = degrees * Deg2Rad;
            //double x = X * Math.Cos(r) - Y * Math.Sin(r);
            //double y = X * Math.Sin(r) + Y * Math.Cos(r);
            //X = x;
            //Y = y;
        }
        /*----------------------- Operator overloading below ------------------------------*/
        public static bool operator ==(Vector2 v1, Vector2 v2)
        {
            if (object.ReferenceEquals(v1, null))
            {
                return object.ReferenceEquals(v2, null);
            }
            return v1.Equals(v2);
        }

        public static bool operator !=(Vector2 v1, Vector2 v2)
        {
            return !(v1 == v2);
        }

        public static Vector2 operator +(Vector2 a, Vector2 b)
        {
            return new Vector2(a.X + b.X, a.Y + b.Y);
        }

        public static Vector2 operator -(Vector2 a, Vector2 b)
        {
            return new Vector2(a.X - b.X, a.Y - b.Y);
        }

        public static Vector2 operator *(Vector2 a, Vector2 b)
        {
            return new Vector2(a.X * b.X, a.Y * b.Y);
        }

        public static Vector2 operator *(Vector2 a, double b)
        {
            return new Vector2(a.X * b, a.Y * b);
        }

        public static Vector2 operator *(double a, Vector2 b)
        {
            return new Vector2(b.X * a, b.Y * a);
        }

        public static Vector2 operator /(Vector2 a, Vector2 b)
        {
            return new Vector2(a.X / b.X, a.Y / b.Y);
        }

        public static Vector2 operator /(Vector2 a, double b)
        {
            return new Vector2(a.X / b, a.Y / b);
        }

        public static Vector2 operator -(Vector2 a)
        {
            return new Vector2(-a.X, -a.Y);
        }
    }
}
