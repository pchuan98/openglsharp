using System.Diagnostics;

namespace OpenGlSharp.Models;

/// <summary>
/// fps自动计时器
/// </summary>
public class FpsTimer
{
    private readonly Stopwatch _stopwatch;
    private readonly int[] _frameTimes;
    private int _frameIndex;
    private int _frameCount;
    private long _totalFrameTime;

    public FpsTimer(int bufferSize = 1000)
    {
        bufferSize = Math.Max(1, bufferSize);
        _frameTimes = new int[bufferSize];
        _stopwatch = Stopwatch.StartNew();
    }

    /// <summary>
    /// 在更新的地方调用该函数
    /// </summary>
    public void Frame()
    {
        var elapsedMilliseconds = _stopwatch.ElapsedMilliseconds;

        // 计算当前帧的实际耗时
        var frameTime = (int)elapsedMilliseconds;

        if (_frameCount < _frameTimes.Length)
            _frameCount++;

        else
            _totalFrameTime -= _frameTimes[_frameIndex];


        _frameTimes[_frameIndex] = frameTime;
        _totalFrameTime += frameTime;

        _frameIndex = (_frameIndex + 1) % _frameTimes.Length;

        _stopwatch.Restart();
    }

    public float Fps
    {
        get
        {
            if (_frameCount == 0 || _totalFrameTime == 0)
                return 0;

            return 1000f * _frameCount / _totalFrameTime;
        }
    }
}