namespace OpenGlSharp.Models;

/// <summary>
/// 相机控制命令
/// </summary>
public abstract class CameraController
{
    /// <summary>
    /// 上下俯角
    /// </summary>
    public float Pitch { get; set; }

    /// <summary>
    /// 左右摆角
    /// </summary>
    public float Yaw { get; set; }

    /// <summary>
    /// 旋转角
    /// </summary>
    public float Roll { get; set; }

    /// <summary>
    /// 移动敏感度
    /// </summary>
    public float Sensitivity { get; set; }

    /// <summary>
    /// 数据帧更新
    /// </summary>
    public abstract void Update();
}