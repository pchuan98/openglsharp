// ReSharper disable once InvalidXmlDocComment

/**
 * 看下span和array直接分配哪个快一些
 *
 * 结论是全局静态转span最快，但是线程不安全
 *
 * 加了lock之后，多线程下直接分配array结果就是最快的，所以还是用array作为返回结果
 */

using BenchmarkDotNet.Attributes;

namespace OpenGlSharp.Benchmark.Others;

file static class ArrayMethod
{
    private static volatile float[] BaseTranslationArray = [1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1];

    private static volatile object _lock = new object();

    // 直接返回
    public static float[] CreateTranslation1(float tx = 0, float ty = 0, float tz = 0)
        => [1, 0, 0, tx, 0, 1, 0, ty, 0, 0, 1, tz, 0, 0, 0, 1];

    // array -> span -> array
    public static float[] CreateTranslation2(float tx = 0, float ty = 0, float tz = 0)
    {
        lock (_lock)
        {
            var array = BaseTranslationArray.AsSpan();
            array[3] = tx;
            array[7] = ty;
            array[11] = tz;
            return array.ToArray();
        }
    }

    // array array
    public static float[] CreateTranslation3(float tx = 0, float ty = 0, float tz = 0)
    {
        lock (_lock)
        {
            var array = BaseTranslationArray;
            array[3] = tx;
            array[7] = ty;
            array[11] = tz;
            return array.ToArray();
        }
    }

    // only span
    public static Span<float> CreateTranslation4(float tx = 0, float ty = 0, float tz = 0)
    {
        lock (_lock)
        {
            float[] src = [1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1];
            var array = src.AsSpan();

            array[3] = tx;
            array[7] = ty;
            array[11] = tz;
            return array;
        }
    }

    // array -> span
    public static Span<float> CreateTranslation5(float tx = 0, float ty = 0, float tz = 0)
    {
        lock (_lock)
        {
            var array = BaseTranslationArray.AsSpan();
            array[3] = tx;
            array[7] = ty;
            array[11] = tz;
            return array;
        }
    }
}

public class ArrayMethodBenchmark
{
    private static readonly float[][] TestData =
    [
        [1.0f, 2.0f, 3.0f],
        [-1.5f, 0.5f, 2.0f],
        [0.0f, -1.0f, 5.0f],
        [3.14f, 2.718f, 1.414f]
    ];

    private int dataIndex;

    [Params(1000)]
    public int IterationCount { get; set; }

    [Params(1, 4, 8)]  // 添加线程数参数
    public int ThreadCount { get; set; }

    [IterationSetup]
    public void IterationSetup()
    {
        dataIndex = (dataIndex + 1) % TestData.Length;
    }

    private void RunParallel(Action testMethod)
    {
        var tasks = new Task[ThreadCount];
        for (int i = 0; i < ThreadCount; i++)
        {
            tasks[i] = Task.Run(() =>
            {
                for (int j = 0; j < IterationCount; j++)
                {
                    testMethod();
                }
            });
        }
        Task.WaitAll(tasks);
    }

    [Benchmark(Description = "src")]
    public void TestCreateTranslation1()
        => RunParallel(() => ArrayMethod.CreateTranslation1(TestData[dataIndex][0],
            TestData[dataIndex][1],
            TestData[dataIndex][2]));

    [Benchmark(Description = "toArray")]
    public void TestCreateTranslation2()
        => RunParallel(() => ArrayMethod.CreateTranslation2(TestData[dataIndex][0],
            TestData[dataIndex][1],
            TestData[dataIndex][2]));

    [Benchmark(Description = "static array")]
    public void TestCreateTranslation3()
        => RunParallel(() => ArrayMethod.CreateTranslation3(TestData[dataIndex][0],
            TestData[dataIndex][1],
            TestData[dataIndex][2]));

    [Benchmark(Description = "only span")]
    public void TestCreateTranslation4()
        => RunParallel(() => ArrayMethod.CreateTranslation4(TestData[dataIndex][0],
            TestData[dataIndex][1],
            TestData[dataIndex][2]));

    [Benchmark(Description = "static span")]
    public void TestCreateTranslation5()
        => RunParallel(() => ArrayMethod.CreateTranslation5(TestData[dataIndex][0],
            TestData[dataIndex][1],
            TestData[dataIndex][2]));
}