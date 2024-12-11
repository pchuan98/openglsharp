namespace OpenGlSharp.Models;

/// <summary>
/// 相机控制命令
/// </summary>
/// <param name="camera"></param>
public abstract class CameraController(Camera camera)
{
    /// <summary>
    /// 偏航(xoz平面，从x开始顺时针)
    /// </summary>
    public float Yaw { get; internal set; } = -90f;

    /// <summary>
    /// 俯仰角(yoz平面，从-z开始逆时针)
    /// </summary>
    public float Pitch { get; internal set; } = 0f;

    /// <summary>
    /// 摆动角度(xoy平面y开始顺时针)
    /// </summary>
    public float Roll { get; internal set; } = 0f;

    /// <summary>
    /// 控制的相机对象
    /// </summary>
    public Camera CameraObj { get; set; } = camera;

    /// <summary>
    /// 平移操作
    /// </summary>
    /// <param name="offset"></param>
    public abstract void Translate(Vector3 offset);

    /// <summary>
    /// 重置所有的相机参数
    /// </summary>
    public abstract void Reset();

    /// <summary>
    /// 使用欧拉角旋转相对角度
    /// </summary>
    /// <param name="yaw">The angle of rotation that relative to current yaw angle, in degree, around the Y axis.</param>
    /// <param name="pitch">The angle of rotation that relative to current yaw angle, in degree, around the X axis.</param>
    /// <param name="roll">The angle of rotation that relative to current yaw angle, in degree, around the Z axis.</param>
    public abstract void RotateByEuler(float yaw, float pitch, float roll);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="yaw"></param>
    /// <param name="pitch"></param>
    /// <param name="roll"></param>
    public abstract void RotateByEulerAbsolute(float yaw, float pitch, float roll);
}