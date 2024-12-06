using System.ComponentModel.DataAnnotations;
using System.Numerics;
using MathNet.Numerics.LinearAlgebra;

namespace OpenGlSharp.Helper;

public static partial class MathHelper
{
    /// <summary>
    /// 角度转弧度
    /// </summary>
    internal const double RADIAN_SCALE = Math.PI / 180.0;

    /// <summary>
    /// 
    /// </summary>
    internal static Vector3 OriginalPos = new(0, 0, 0);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="data"></param>
    /// <param name="n"></param>
    /// <returns></returns>
    /// <exception cref="NotSupportedException"></exception>
    [Obsolete("目前没有验证正确性")]
    public static float[,] To2D(this float[] data, int n) => Matrix<float>.Build.Dense(n, n, data).Transpose().ToArray();

    /// <summary>
    /// 
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    [Obsolete("目前没有验证正确性")]
    public static float[] To1D(this float[,] data) => Matrix<float>.Build.DenseOfArray(data).ToRowMajorArray();

    /// <summary>
    /// 去除四维矩阵的第四列
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public static float[] To33(this float[] data)
    {
        throw new NotSupportedException();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="array"></param>
    /// <returns></returns>
    public static string AsString(this float[] array)
    {
        var length = (int)Math.Sqrt(array.Length);
        var str = "";

        for (var i = 0; i < length; i++)
        {
            for (var j = 0; j < length; j++)
                str += $"{array[i * length + j]:F4}\t";
            str += Environment.NewLine;
        }

        return str;
    }

    public static string AsString(this float[,] array)
    {
        var length = (int)Math.Sqrt(array.Length);
        var str = "";

        for (var i = 0; i < length; i++)
        {
            for (var j = 0; j < length; j++)
                str += $"{array[i, j]:F4}\t";
            str += Environment.NewLine;
        }

        return str;
    }
}

#region Model Transformation

// cal
partial class MathHelper
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    [Obsolete]
    public static float[] DotProduct(this float[] a, float[] b)
    {
        // 默认列读取数据
        var ma = Matrix<float>.Build.Dense(4, 4, a).Transpose();
        var mb = Matrix<float>.Build.Dense(4, 4, b).Transpose();

        return (ma * mb).ToRowMajorArray();
    }

    /// <summary>
    /// 点乘
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public static float[] Dot(this float[] a, float[] b)
    {
        var result = new float[16];
        DotProductBySpan(a.AsSpan(), b.AsSpan(), result.AsSpan());
        return result;
    }

    private static void DotProductBySpan(ReadOnlySpan<float> a, ReadOnlySpan<float> b, Span<float> result)
    {
        if (a.Length != 16 || b.Length != 16 || result.Length != 16)
            throw new ArgumentException("must be a 4 x 4 matrix");

        for (var row = 0; row < 4; row++)
        {
            for (var col = 0; col < 4; col++)
            {
                float sum = 0;
                for (var k = 0; k < 4; k++)
                {
                    // a[row,k] * b[k,col]
                    sum += a[row * 4 + k] * b[k * 4 + col];
                }
                result[row * 4 + col] = sum;
            }
        }
    }
}

// translation
partial class MathHelper
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="tx"></param>
    /// <param name="ty"></param>
    /// <param name="tz"></param>
    /// <returns></returns>
    public static float[] CreateTranslation(float tx = 0, float ty = 0, float tz = 0)
        => [1, 0, 0, tx, 0, 1, 0, ty, 0, 0, 1, tz, 0, 0, 0, 1];
}

// rotate
// note: 旋转矩阵正交，满足ATA = AAT = I ==> A = A(-1)
partial class MathHelper
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="theta"></param>
    /// <returns></returns>
    public static float[] CreateRotateX(float theta = 0)
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
    public static float[] CreateRotateY(float theta = 0)
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
    public static float[] CreateRotateZ(float theta = 0)
    {
        var radian = theta * RADIAN_SCALE;

        var sinx = (float)Math.Sin(radian);
        var cosx = (float)Math.Cos(radian);

        return [cosx, sinx, 0, 0, -sinx, cosx, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1];
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="u"></param>
    /// <param name="theta"></param>
    /// <returns></returns>
    /// <exception cref="ValidationException"></exception>
    public static float[] CreateRotate(float[] u, float theta = 0)
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

        return [n11, n12, n13, 0, n21, n22, n23, 0, n31, n32, n33, 0, 0, 0, 0, 1];
    }
}

// scale
partial class MathHelper
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="tx"></param>
    /// <param name="ty"></param>
    /// <param name="tz"></param>
    /// <returns></returns>
    public static float[] CreateScaleMain(float tx = 1, float ty = 1, float tz = 1)
        => [tx, 0, 0, 0, 0, ty, 0, 0, 0, 0, tz, 0, 0, 0, 0, 1];
}

// scale
partial class MathHelper
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="u"></param>
    /// <param name="k"></param>
    /// <returns></returns>
    /// <exception cref="ValidationException"></exception>
    public static float[] CreateScale(float[] u, float k)
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

#endregion

#region View / Camera Transformation

// todo 

#endregion

#region Projection Transformation

// todo 

#endregion