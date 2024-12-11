using System.Globalization;

namespace OpenGlSharp.Models;

public abstract partial class Camera
{
    /// <summary>
    /// 相机所在的位置
    /// </summary>
    public Vector3 Position { get; internal set; } = new(0, 0, 1);

    /// <summary>
    /// 相机观察的点位坐标（归一化后的坐标，实际值为Position+Front）
    /// </summary>
    public Vector3 Front { get; internal set; } = new(0, 0, -1);

    /// <summary>
    /// 相机的头顶的方向，这个一般不会改变
    /// </summary>
    public Vector3 Up { get; internal set; } = new(0, 1, 0);

    /// <summary>
    /// 视图（相机）矩阵
    /// </summary>
    public virtual Matrix ViewMatrix
        => Matrix.CreateLookAt(Position, Position + Front, Up);

    /// <summary>
    /// 投影矩阵
    /// </summary>
    public abstract Matrix ProjectionMatrix { get; }
}

partial class Camera
{
    public override string ToString()
    {
        var pos = string.Format(
            CultureInfo.InvariantCulture,
            "Position:\t{0:0.000},{1:0.000},{2:0.000}",
            Position.X,
            Position.Y,
            Position.Z);

        var lookup = string.Format(
            CultureInfo.InvariantCulture,
            "Front:\t{0:0.000},{1:0.000},{2:0.000}",
            Front.X,
            Front.Y,
            Front.Z);

        var top = string.Format(
            CultureInfo.InvariantCulture,
            "Up:\t{0:0.000},{1:0.000},{2:0.000}",
            Up.X,
            Up.Y,
            Up.Z);

        return string.Join('\n', [pos, lookup, top]);
    }
}