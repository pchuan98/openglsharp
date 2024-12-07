// ReSharper disable once InvalidXmlDocComment

/**
 * 比较MathNet和使用Span的计算性能差距
 *
 * 结果是Span直接计算性能远高于MathNet
 */

using MathNet.Numerics.LinearAlgebra;
using BenchmarkDotNet.Attributes;
using System.Numerics;

namespace OpenGlSharp.Benchmark.Others;

file static class MatrixCalMethod
{
    #region Dot

    /// <summary>
    /// 
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public static float[] DotProductByMathNet(float[] a, float[] b)
    {
        // 默认列读取数据
        var ma = Matrix<float>.Build.Dense(4, 4, a).Transpose();
        var mb = Matrix<float>.Build.Dense(4, 4, b).Transpose();

        return (ma * mb).ToRowMajorArray();
    }

    public static float[] DotProductBySpan(float[] a, float[] b)
    {
        var result = new float[16];
        DotProductBySpan(a.AsSpan(), b.AsSpan(), result.AsSpan());
        return result;
    }

    private static void DotProductBySpan(ReadOnlySpan<float> a, ReadOnlySpan<float> b, Span<float> result)
    {
        if (a.Length != 16 || b.Length != 16 || result.Length != 16)
            throw new ArgumentException("矩阵大小必须是4x4");

        for (int row = 0; row < 4; row++)
        {
            for (int col = 0; col < 4; col++)
            {
                float sum = 0;
                for (int k = 0; k < 4; k++)
                {
                    // a[row,k] * b[k,col]
                    sum += a[row * 4 + k] * b[k * 4 + col];
                }
                result[row * 4 + col] = sum;
            }
        }
    }

    public static float[] DotProductByMatrix4x4(float[] a, float[] b)
    {
        var ma = new Matrix4x4(
            a[0], a[1], a[2], a[3],
            a[4], a[5], a[6], a[7],
            a[8], a[9], a[10], a[11],
            a[12], a[13], a[14], a[15]
        );
        
        var mb = new Matrix4x4(
            b[0], b[1], b[2], b[3],
            b[4], b[5], b[6], b[7],
            b[8], b[9], b[10], b[11],
            b[12], b[13], b[14], b[15]
        );
        
        var result = Matrix4x4.Multiply(ma, mb);
        return new[]
        {
            result.M11, result.M12, result.M13, result.M14,
            result.M21, result.M22, result.M23, result.M24,
            result.M31, result.M32, result.M33, result.M34,
            result.M41, result.M42, result.M43, result.M44
        };
    }

    #endregion
}

public class MatrixCalBenchmark
{
    private readonly List<(float[] A, float[] B)> _testMatrices;

    public MatrixCalBenchmark()
    {
        _testMatrices = new List<(float[] A, float[] B)>
        {
            (CreateIdentityMatrix(), CreateRandomMatrix()),
            (CreateRandomMatrix(), CreateRandomMatrix()),
            (CreateFilledMatrix(1.0f), CreateRandomMatrix())
        };

        ValidateResults();
    }

    private void ValidateResults()
    {
        foreach (var (a, b) in _testMatrices)
        {
            var result1 = MatrixCalMethod.DotProductByMathNet(a, b);
            var result2 = MatrixCalMethod.DotProductBySpan(a, b);
            var result3 = MatrixCalMethod.DotProductByMatrix4x4(a, b);

            for (int i = 0; i < 16; i++)
            {
                if (Math.Abs(result1[i] - result2[i]) > 1e-6f || Math.Abs(result1[i] - result3[i]) > 1e-6f)
                {
                    throw new Exception($"计算结果不一致！位置: {i}, MathNet: {result1[i]}, Span: {result2[i]}, Matrix4x4: {result3[i]}");
                }
            }
        }
        Console.WriteLine("所有方法计算结果一致性验证通过！");
    }

    [Benchmark]
    public void MathNetDot()
    {
        foreach (var (a, b) in _testMatrices)
        {
            MatrixCalMethod.DotProductByMathNet(a, b);
        }
    }

    [Benchmark]
    public void SpanDot()
    {
        foreach (var (a, b) in _testMatrices)
        {
            MatrixCalMethod.DotProductBySpan(a, b);
        }
    }

    [Benchmark]
    public void Matrix4x4Dot()
    {
        foreach (var (a, b) in _testMatrices)
        {
            MatrixCalMethod.DotProductByMatrix4x4(a, b);
        }
    }

    // 辅助方法：创建4x4单位矩阵
    private static float[] CreateIdentityMatrix()
    {
        var matrix = new float[16];
        for (var i = 0; i < 4; i++)
            matrix[i * 4 + i] = 1.0f;

        return matrix;
    }

    // 辅助方法：创建4x4随机矩阵
    private static float[] CreateRandomMatrix()
    {
        var random = new Random(42);
        var matrix = new float[16];
        for (var i = 0; i < 16; i++)
            matrix[i] = (float)random.NextDouble();

        return matrix;
    }

    // 辅助方法：创建填充了指定值的4x4矩阵
    private static float[] CreateFilledMatrix(float value)
    {
        var matrix = new float[16];
        for (var i = 0; i < 16; i++)
            matrix[i] = value;

        return matrix;
    }
}