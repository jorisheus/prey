namespace Prey.Domain
{
    public struct Point
    {
        public bool Equals(Point other)
        {
            return X == other.X && Y == other.Y;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Point && Equals((Point) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (X*397) ^ Y;
            }
        }

        public int X { get; set; }

        public int Y { get; set; }

        public Point(int x, int y) : this()
        {
            X = x;
            Y = y;
        }

        public static Point operator +(Point p1, Point p2)
        {
            p1.X += p2.X;
            p1.Y += p2.Y;
            return p1;
        }

        public static bool operator !=(Point p1, Point p2)
        {
            return p1.X != p2.X || p1.Y != p2.Y;
        }

        public static bool operator ==(Point p1, Point p2)
        {
            return p1.X == p2.X && p1.Y == p2.Y;
        }

        public double DistanceFromZero()
        {
            return Math.Sqrt(X * X + Y * Y);
        }

        public static double Distance(Point p1, Point p2)
        {
            return Math.Sqrt((p1.X - p2.X) * (p1.X - p2.X) + (p1.Y - p2.Y) * (p1.Y - p2.Y));
        }

        public Point Clone()
        {
            return new Point(X, Y);
        }

        public override string ToString()
        {
            return String.Format("{0:000}.{1:000}", X, Y);
        }
    }
}