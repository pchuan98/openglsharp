using System.Numerics;

// note 由于opengl是列优先，所以所有的数据也是列优先的

namespace OpenGlSharp.MathLib;

/// <summary>
/// 模型变换
///
/// 默认右手系,所有数据默认4x4矩阵
/// </summary>
public static class ModelTransformation
{
    internal const double RADIAN_SCALE = 0.01745329251994329576923690768489f;

    /// <summary>
    /// 平移变换
    /// </summary>
    /// <param name="tx"></param>
    /// <param name="ty"></param>
    /// <param name="tz"></param>
    /// <returns></returns>
    public static float[] Translation(float tx = 0, float ty = 0, float tz = 0)
        => [1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, tx, ty, tz, 1];

    ///// <summary>
    ///// 
    ///// </summary>
    ///// <param name="data"></param>
    ///// <param name="tx"></param>
    ///// <param name="ty"></param>
    ///// <param name="tz"></param>
    ///// <returns></returns>
    //public static float[] Translation(this float[] data, float tx = 0, float ty = 0, float tz = 0)
    //    => [1, 0, 0, tx, 0, 1, 0, ty, 0, 0, 1, tz, 0, 0, 0, 1];

    /// <summary>
    /// 绕x轴顺时针旋转
    /// </summary>
    /// <param name="theta"></param>
    /// <returns></returns>
    public static float[] RotateX(float theta = 0)
    {
        var radian = theta * RADIAN_SCALE;

        var sinx = (float)Math.Sin(radian);
        var cosx = (float)Math.Cos(radian);

        return [1, 0, 0, 0, 0, cosx, -sinx, 0, 0, sinx, cosx, 0, 0, 0, 0, 1];
    }

    /// <summary>
    /// 绕y轴顺时针旋转
    /// </summary>
    /// <param name="theta"></param>
    /// <returns></returns>
    public static float[] RotateY(float theta = 0)
    {
        var radian = theta * RADIAN_SCALE;

        var sinx = (float)Math.Sin(radian);
        var cosx = (float)Math.Cos(radian);

        return [cosx, 0, sinx, 0, 0, 1, 0, 0, -sinx, 0, cosx, 0, 0, 0, 0, 1];
    }

    /// <summary>
    /// 绕z轴顺时针旋转
    /// </summary>
    /// <param name="theta"></param>
    /// <returns></returns>
    public static float[] RotateZ(float theta = 0)
    {
        var radian = theta * RADIAN_SCALE;

        var sinx = (float)Math.Sin(radian);
        var cosx = (float)Math.Cos(radian);

        return [cosx, -sinx, 0, 0, sinx, cosx, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1];
    }

    /// <summary>
    /// 绕任意轴旋转
    /// </summary>
    /// <param name="u"></param>
    /// <param name="theta"></param>
    /// <returns></returns>
    public static float[] Rotate(float[] u, float theta = 0)
    {
        var radian = theta * RADIAN_SCALE;

        var cosx = (float)Math.Cos(radian);
        var cosx1 = 1 - cosx;

        var sinx = (float)Math.Sin(radian);

        var norm = (float)Math.Sqrt(u[0] * u[0] + u[1] * u[1] + u[2] * u[2]);

        var ux = u[0] / norm;
        var uy = u[1] / norm;
        var uz = u[2] / norm;

        var uxy = ux * uy;
        var uxz = ux * uz;
        var uyz = uy * uz;

        var n11 = cosx + ux * ux * cosx1;
        var n12 = uxy * cosx1 - uz * sinx;
        var n13 = uxz * cosx1 + uy * sinx;

        var n21 = uxy * cosx1 + uz * sinx;
        var n22 = cosx + uy * uy * cosx1;
        var n23 = uyz * cosx1 - ux * sinx;

        var n31 = uxz * cosx1 - uy * sinx;
        var n32 = uyz * cosx1 + ux * sinx;
        var n33 = cosx + uz * uz * cosx1;

        float[] result = [n11, n12, n13, 0, n21, n22, n23, 0, n31, n32, n33, 0, 0, 0, 0, 1];

        return result.T();
    }

    /// <summary>
    /// 沿着主轴缩放
    /// </summary>
    /// <param name="tx"></param>
    /// <param name="ty"></param>
    /// <param name="tz"></param>
    /// <returns></returns>
    public static float[] Scale(float tx = 1, float ty = 1, float tz = 1)
        => [tx, 0, 0, 0, 0, ty, 0, 0, 0, 0, tz, 0, 0, 0, 0, 1];

    /// <summary>
    /// 
    /// </summary>
    /// <param name="u">缩放的方向</param>
    /// <param name="k"></param>
    /// <returns></returns>
    public static float[] Scale(float[] u, float k)
    {
        var scale = k - 1;

        var norm = (float)Math.Sqrt(u[0] * u[0] + u[1] * u[1] + u[2] * u[2]);

        var ux = u[0] / norm;
        var uy = u[1] / norm;
        var uz = u[2] / norm;

        var uxy = ux * uy;
        var uxz = ux * uz;
        var uyz = uy * uz;

        var n11 = 1 + scale * ux * ux;
        var n12 = scale * uxy;
        var n13 = scale * uxz;

        var n21 = scale * uxy;
        var n22 = 1 + scale * uy * uy;
        var n23 = scale * uyz;

        var n31 = scale * uxz;
        var n32 = scale * uyz;
        var n33 = 1 + scale * uz * uz;

        return [n11, n12, n13, 0, n21, n22, n23, 0, n31, n32, n33, 0, 0, 0, 0, 1];
    }
}