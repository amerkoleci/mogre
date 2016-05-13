namespace Mogre
{
    public static class MathHelper
    {
        public enum AngleUnit
        {
            AU_DEGREE = 0,
            AU_RADIAN = 1,
            Degree = 0,
            Radian = 1
        }

        public const float POS_INFINITY = float.PositiveInfinity;

        public const float NEG_INFINITY = float.NegativeInfinity;

        public const float PI = 3.14159274f;

        public const float TWO_PI = 6.28318548f;

        public const float HALF_PI = 1.57079637f;

        public const float fDeg2Rad = 0.0174532924f;

        public const float fRad2Deg = 57.2957764f;

        public static AngleUnit CurrentAngleUnit = AngleUnit.Degree;

        public static float DegreesToRadians(float degrees)
        {
            return degrees * fDeg2Rad;
        }

        public static float RadiansToDegrees(float radians)
        {
            return radians * fRad2Deg;
        }

        public static float RadiansToAngleUnits(float radians)
        {
            if (CurrentAngleUnit == AngleUnit.Degree)
                return radians * fRad2Deg;

            return radians;
        }

        public static float DegreesToAngleUnits(float degrees)
        {
            if (CurrentAngleUnit == AngleUnit.Radian)
                return degrees * fDeg2Rad;

            return degrees;
        }

        public static float AngleUnitsToRadians(float angleunits)
        {
            if (CurrentAngleUnit == AngleUnit.Degree)
                return angleunits * fDeg2Rad;

            return angleunits;
        }

        public static float AngleUnitsToDegrees(float angleunits)
        {
            if (CurrentAngleUnit == AngleUnit.Radian)
                return angleunits * fRad2Deg;

            return angleunits;
        }
    }
}
