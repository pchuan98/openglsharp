using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace OpenGlSharp.MathLib;

/// <summary>
/// 4x4矩阵计算
/// </summary>
public static class Matrix44Extension
{
    /// <summary>
    /// a . b 点乘
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public static float[] DotProduct(this float[] a, float[] b)
    {
        var result = new float[16];
        DotProductBySpan(a.AsSpan(), b.AsSpan(), result.AsSpan());
        return result;
    }

    /// <summary>
    /// a . b 点乘
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <param name="result"></param>
    /// <exception cref="ArgumentException"></exception>
    public static void DotProductBySpan(ReadOnlySpan<float> a, ReadOnlySpan<float> b, Span<float> result)
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