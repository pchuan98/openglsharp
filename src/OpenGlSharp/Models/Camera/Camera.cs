using System.Globalization;

namespace OpenGlSharp.Models;

public abstract partial class Camera
{
    /// <summary>
    /// 相机所在的位置
    /// </summary>
    public Vector3 Position { get; set; } = new(0, 0, 1);

    /// <summary>
    /// 相机朝向
    /// </summary>
    public Vector3 Lookup { get; set; } = new(0, 0, -1);

    /// <summary>
    /// 相机的头顶的方向
    /// </summary>
    public Vector3 Top { get; set; } = new(0, 1, 0);

    /// <summary>
    /// 视图（相机）矩阵
    /// </summary>
    public virtual Matrix ViewMatrix
        => Matrix.CreateLookAt(Position, Lookup, Top);

    /// <summary>
    /// 投影矩阵
    /// </summary>
    public abstract Matrix ProjectionMatrix { get; init; }
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
            "Lookup:\t{0:0.000},{1:0.000},{2:0.000}",
            Lookup.X,
            Lookup.Y,
            Lookup.Z);

        var top = string.Format(
            CultureInfo.InvariantCulture,
            "Top:\t{0:0.000},{1:0.000},{2:0.000}",
            Top.X,
            Top.Y,
            Top.Z);

        return string.Join('\n', [pos, lookup, top]);
    }
}