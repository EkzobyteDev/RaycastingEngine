using System;
using SFML.System;

namespace RaycastingEngine
{
    // AVector = advanced vector
    public struct AVector2f
    {
        public static AVector2f zero = new AVector2f(0, 0);
        public static AVector2f one = new AVector2f(1, 1);
        public static AVector2f right = new AVector2f(1, 0);
        public static AVector2f left = new AVector2f(-1, 0);
        public static AVector2f up = new AVector2f(0, 1);
        public static AVector2f down = new AVector2f(0, -1);

        public static AVector2f operator +(AVector2f a, AVector2f b) => new AVector2f(a.x + b.x, a.y + b.y);
        public static AVector2f operator -(AVector2f a, AVector2f b) => new AVector2f(a.x - b.x, a.y - b.y);
        public static AVector2f operator *(AVector2f a, float b) => new AVector2f(a.x * b, a.y * b);
        public static AVector2f operator /(AVector2f a, float b) => new AVector2f(a.x / b, a.y / b);

        public float x;
        public float y;

        public AVector2f(float x, float y)
        {
            this.x = x;
            this.y = y;
        }
        public Vector2f toSFMLVec
        {
            get
            {
                return new Vector2f(x, y);
            }
        }

        public float length
        {
            get
            {
                return (float)Math.Sqrt(x * x + y * y);
            }
        }
        public float sqrLength
        {
            get
            {
                return x * x + y * y;
            }
        }
        public AVector2f normalized
        {
            get
            {
                if (x == 0 && y == 0) return this;
                return new AVector2f(x / this.length, y / this.length);
            }
        }
    }

    // MathV = vector math
    public static class MathV
    {
        public static AVector2f Rotate(AVector2f a, float angle)
        {
            AVector2f b = new AVector2f();
            angle *= (float)(Math.PI / 180);
            b.x = (float)(a.x * Math.Cos(angle) - a.y * Math.Sin(angle));
            b.y = (float)(a.y * Math.Cos(angle) + a.x * Math.Sin(angle));
            return b;
        }
        public static float Cos(AVector2f a, AVector2f b)
        {
            return (a.x * b.x + a.y * b.y) / a.length * b.length;
        }
    }
}
