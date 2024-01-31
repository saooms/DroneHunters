using System;
using UnityEngine;
using static System.Math;

namespace Logic
{
    
    [Serializable]
    public class CoordinateVector
    {
        public double x;
        public double y;
        public double z;

        public CoordinateVector(double x = 0, double y = 0, double z = 0)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public static CoordinateVector operator *(int scalar, CoordinateVector position) => new CoordinateVector(position.x * scalar, position.y * scalar, position.z * scalar);
        public static CoordinateVector operator *(CoordinateVector position, int scalar) => scalar * position;
        public static CoordinateVector operator -(CoordinateVector positionA, CoordinateVector positionB) => new CoordinateVector(positionA.x - positionB.x, positionA.y - positionB.y, positionA.z - positionB.z);

        public static implicit operator CoordinateVector(Vector3 position) => new CoordinateVector(position.x, position.y, position.z);
        public static explicit operator Vector3(CoordinateVector position) => new Vector3((float)position.x, (float)position.y, (float)position.z);

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            CoordinateVector other = (CoordinateVector)obj;
            return IsWithinErrorMargin(x, other.x) && IsWithinErrorMargin(y, other.y) && IsWithinErrorMargin(z, other.z);
        }

        private bool IsWithinErrorMargin(double a, double b)
        {
            return Abs(a - b) < 0.000000000000001;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(x, y, z);
        }

        public override string ToString()
        {
            return $"{Round(x, 13)}, {Round(z, 13)}";
        }
    }
}
