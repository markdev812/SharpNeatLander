using System;

namespace SharpNeatLander
{
    public class Vector2
    {
        public readonly static Vector2 Zero = new Vector2(0, 0);
        public readonly static Vector2 Up = new Vector2(0, 1);
        public readonly static Vector2 Right = new Vector2(1, 0);
        public readonly static Vector2 One = new Vector2(1, 1);


        public double X { get; set; }
        public double Y { get; set; }

        public double Magnatude { get { return Length; } }

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
                return Math.Atan2(Y, X);
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
            return (X == other.X) && (Y == other.Y);
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

        public void Rotate(double angle)
        {
            double x = X * Math.Cos(angle) - Y * Math.Sin(angle);
            double y = X * Math.Sin(angle) + Y * Math.Cos(angle);
            X = x;
            Y = y;
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
