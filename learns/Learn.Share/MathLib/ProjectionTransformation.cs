using System.Numerics;
using OpenGlSharp.Helper;

namespace OpenGlSharp.MathLib;

public static class ProjectionTransformation
{
    /// <summary>
    /// 正交投影 (结果变成-1到1)
    /// </summary>
    /// <param name="l"></param>
    /// <param name="r"></param>
    /// <param name="b"></param>
    /// <param name="t"></param>
    /// <param name="f"></param>
    /// <param name="n"></param>
    /// <returns></returns>
    public static float[] Orghographic(
        float l, float r, float b, float t, float f, float n)
    {
        // center cuboid by tranlating
        var trans = ModelTransformation.Translation(-(l + r) / 2, -(t + b) / 2, -(f + n) / 2);

        // scale into canonical cube
        var scale = ModelTransformation.Scale(2 / (r - l), 2 / (t - b), 2 / (n - f));

        // dot cal
        var result = scale.DotProduct(trans);

        Console.WriteLine(result.AsString());

        return result;
    }

    public static float[] Orghographic(float range)
        => Orghographic(-range, range, -range, range, -range, range);

    /**
 * 关于透视投影
 * 1. 近平面不变
 * 2. 中心点不变
 * 3. 近平面(n)和远平面(f)的z不变，中间的z会有所变化
 * 4. f > n > 0
 *
 * 求解方法是 求解某个矩阵，将frustum（视锥体）压缩成为一个长方体，然后把这个长方体进行正交变换
 *
 * 通过相似三角形的关系，n的值，对任意z，有， (我们所期望的长方体应该是压缩到近平面的比例下)
 * zx or zy = (nx or ny) * nz / zz
 *  ==>
 * | n    0    0    0 |  | x |      | xn |
 * | 0    n    0    0 |  | y |  ==  | yn |
 * | A    B    C    D |  | z |  ==  | ?  |
 * | 0    0    0    z |  | 1 |      | z  |
 * (ABCD)为未知待定系数
 *
 * 加入特值点
 * * 在near上 任一点(i,j,n)变换结果不变
 * * 在far上，任一点(k,l,f)变换结果中，z恒定f
 *
 * Ai + Bj + Cn + D = n*n
 * Ak + Bl + Cf + D = f*f
 *
 * 使用特值法，当nf=0的时候 + ijkl为任意一点 => A = B = 0
 *
 * Cn + D = n*n
 * Cf + D = f*f
 * ==> C = n + f ,  D = -nf
 */

    /// <summary>
    /// 
    /// </summary>
    /// <param name="l"></param>
    /// <param name="r"></param>
    /// <param name="b"></param>
    /// <param name="t"></param>
    /// <param name="f"></param>
    /// <param name="n"></param>
    /// <returns></returns>
    public static float[] Perspective(
        float l, float r, float b, float t, float f, float n)
    {
        //float[] trans = [
        //    n, 0, 0, 0,
        //    0, n, 0, 0,
        //    0, 0, n + f, -n * f,
        //    0, 0, 0, n];

        //var ortho = CreateOrghographicProjection(l, r, b, t, f, n);
        //var per = ortho.Dot(trans);


        float[] per = [
            (2 * n) / (r - l), 0, -(r + l) / (r - l), 0,
            0, 2 * n / (t - b), -(t + b) / (t - b), 0,
            0, 0, (n + f) / (n - f), -2 * n * f / (n - f),
            0, 0, 1, 0];
        //return per;
        var matrix = Matrix4x4.CreatePerspectiveFieldOfView(l, r, n, f);

        return [matrix.M11, matrix.M12, matrix.M13, matrix.M14,
            matrix.M21, matrix.M22, matrix.M23, matrix.M24,
            matrix.M31, matrix.M32, matrix.M33, matrix.M34,
            matrix.M41, matrix.M42, matrix.M43, matrix.M44];
    }

    public static float[] Perspective(float ratio)
    {
        var matrix = Matrix4x4.CreatePerspectiveFieldOfView(45f * 0.01745329251994329576923690768489f, 1f, 0.1f, 100f);

        //matrix.M43 *= 2;

        Console.WriteLine($"{matrix.M11:F5} {matrix.M12:F5} {matrix.M13:F5} {matrix.M14:F5}");
        Console.WriteLine($"{matrix.M21:F5} {matrix.M22:F5} {matrix.M23:F5} {matrix.M24:F5}");
        Console.WriteLine($"{matrix.M31:F5} {matrix.M32:F5} {matrix.M33:F5} {matrix.M34:F5}");
        Console.WriteLine($"{matrix.M41:F5} {matrix.M42:F5} {matrix.M43:F5} {matrix.M44:F5}");

        return [matrix.M11, matrix.M12, matrix.M13, matrix.M14,
            matrix.M21, matrix.M22, matrix.M23, matrix.M24,
            matrix.M31, matrix.M32, matrix.M33, matrix.M34,
            matrix.M41, matrix.M42, matrix.M43, matrix.M44];
    }
}
