using System.Net.Http.Headers;

namespace OpenGlSharp.MathLib;

public static class Vector4Extension
{
    /// <summary>
    /// 向量a叉乘b（三维向量）
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public static float[] Cross3(this float[] a, float[] b)
    {
        var x = a[1] * b[2] - a[2] * b[1];
        var y = a[2] * b[0] - a[0] * b[2];
        var z = a[0] * b[1] - a[1] * b[0];

        return [x, y, z];
    }

    /// <summary>
    /// 向量a叉乘b（三维向量）
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public static float Dot3(this float[] a, float[] b)
        => a[0] * b[0] + a[1] * b[1] + a[2] * b[2];

    /// <summary>
    /// 任意向量变成单位向量
    /// </summary>
    /// <param name="vector"></param>
    /// <returns></returns>
    public static float[] ToUnitVector(this float[] vector)
    {
        var sum = (float)Math.Sqrt(vector.Sum(i => i * i));

        return vector.Select(item => item / sum).ToArray();
    }

}