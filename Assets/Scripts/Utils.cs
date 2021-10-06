using UnityEngine;

namespace SaltyTank
{
    public static class Utils
    {
        public static float NormalizeAngle(float a)
        {
            return a - 180f * Mathf.Floor((a + 180f) / 180f);
        }

        public static float UnwrapAngle(float angle)
        {
            if (angle >= 0)
                return angle;

            angle = -angle % 360;

            return 360 - angle;
        }
    }
}