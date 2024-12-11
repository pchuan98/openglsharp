namespace OpenGlSharp.Extensions.Math;

public static class FloatExtension
{
    /// <summary>
    /// 角度转弧度系数
    /// </summary>
    internal const float RADIAN_SCALE = 0.01745329251994329576923690768489f;

    /// <summary>
    /// 弧度转角度系数
    /// </summary>
    internal const float DEGREE_SCALE = 57.295779513082320876798154814105f;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="degree"></param>
    /// <returns></returns>
    public static float AsRadian(this float degree)
        => degree * RADIAN_SCALE;

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public static float AsDegree(this float radian)
        => radian * DEGREE_SCALE;
}