namespace OpenGlSharp.MathLib;

/// <summary>
/// 4x4矩阵计算
/// </summary>
public static class Matrix44Extension
{
    public static float[] One = [1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1];

    /// <summary>
    /// 矩阵转置
    /// </summary>
    /// <param name="matrix"></param>
    /// <returns></returns>
    public static float[] T(this float[] matrix)
    {
        if (matrix.Length != 16)
            throw new ArgumentException("must be a 4 x 4 matrix");

        return [
            matrix[0], matrix[4], matrix[8], matrix[12],
            matrix[1], matrix[5], matrix[9], matrix[13],
            matrix[2], matrix[6], matrix[10], matrix[14],
            matrix[3], matrix[7], matrix[11], matrix[15]];
    }

    /// <summary>
    /// a . b 点乘
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public static float[] DotProduct(this float[] a, float[] b)
    {
        var result = new float[16];

        var ta = a.T();
        var tb = b.T();

        DotProductBySpan(tb.AsSpan(), ta.AsSpan(), result.AsSpan());

        return result.T();
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