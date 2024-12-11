using OpenGlSharp.Extensions.Math;

namespace OpenGlSharp.Models;

public sealed class PerspectiveCamera : Camera
{
    public override Matrix ProjectionMatrix
        => Matrix.CreatePerspectiveFieldOfView(Fov.AsRadian(), Aspect, NearPlane, FarPlane);

    /// <summary>
    /// fov by degree
    /// </summary>
    public float Fov { get; set; } = 45f;

    /// <summary>
    /// width / height
    /// </summary>
    public float Aspect { get; set; } = 1f;

    /// <summary>
    /// 近平面 > 0
    /// </summary>
    public float NearPlane { get; set; } = 0.1f;

    /// <summary>
    /// 远平面 > near > 0
    /// </summary>
    public float FarPlane { get; set; } = 100f;
}