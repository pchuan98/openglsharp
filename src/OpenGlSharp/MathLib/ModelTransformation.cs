namespace OpenGlSharp.MathLib;

/// <summary>
/// 模型变换
///
/// 默认右手系,所有数据默认4x4矩阵
/// </summary>
public static class ModelTransformation
{
    internal const double RADIAN_SCALE = Math.PI / 180.0;

    /// <summary>
    /// 平移变换
    /// </summary>
    /// <param name="tx"></param>
    /// <param name="ty"></param>
    /// <param name="tz"></param>
    /// <returns></returns>
    public static float[] Translation(float tx = 0, float ty = 0, float tz = 0)
        => [1, 0, 0, tx, 0, 1, 0, ty, 0, 0, 1, tz, 0, 0, 0, 1];

    public static float[] Translation(this float[] data, float tx = 0, float ty = 0, float tz = 0)
        => [1, 0, 0, tx, 0, 1, 0, ty, 0, 0, 1, tz, 0, 0, 0, 1];

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

        return [1, 0, 0, 0, 0, cosx, sinx, 0, 0, -sinx, cosx, 0, 0, 0, 0, 1];
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="theta"></param>
    /// <returns></returns>
    public static float[] RotateY(float theta = 0)
    {
        var radian = theta * RADIAN_SCALE;

        var sinx = (float)Math.Sin(radian);
        var cosx = (float)Math.Cos(radian);

        return [cosx, 0, -sinx, 0, 0, 1, 0, 0, sinx, 0, cosx, 0, 0, 0, 0, 1];
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="theta"></param>
    /// <returns></returns>
    public static float[] RotateZ(float theta = 0)
    {
        var radian = theta * RADIAN_SCALE;

        var sinx = (float)Math.Sin(radian);
        var cosx = (float)Math.Cos(radian);

        return [cosx, sinx, 0, 0, -sinx, cosx, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1];
    }
}