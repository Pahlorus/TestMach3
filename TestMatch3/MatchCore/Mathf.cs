//Taken from an external source: structs Mathf, MathfInternal of UnityEngine.dll

using System;

namespace MatchCore
{
    public struct Mathf
    {
        /// <summary>
        ///   <para>A tiny floating point value (Read Only).</para>
        /// </summary>
        public static readonly float Epsilon = MathfInternal.IsFlushToZeroEnabled ? MathfInternal.FloatMinNormal : MathfInternal.FloatMinDenormal;

        /// <summary>
        ///   <para>Returns the absolute value of f.</para>
        /// </summary>
        /// <param name="f"></param>
        public static float Abs(float f) => Math.Abs(f);

        /// <summary>
        ///   <para>Compares two floating point values and returns true if they are similar.</para>
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        public static bool Approximately(float a, float b) => (double)Mathf.Abs(b - a) < (double)Mathf.Max(1E-06f * Mathf.Max(Mathf.Abs(a), Mathf.Abs(b)), Mathf.Epsilon * 8f);

        /// <summary>
        ///   <para>Returns largest of two or more values.</para>
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="values"></param>
        public static float Max(float a, float b) => (double)a > (double)b ? a : b;

        /// <summary>
        ///   <para>Returns f rounded to the nearest integer.</para>
        /// </summary>
        /// <param name="f"></param>
        public static int RoundToInt(float f) => (int)Math.Round((double)f);
    }

    public struct MathfInternal
    {
        public static volatile float FloatMinNormal = 1.175494E-38f;
        public static volatile float FloatMinDenormal = float.Epsilon;
        public static bool IsFlushToZeroEnabled = (double)MathfInternal.FloatMinDenormal == 0.0;
    }
}