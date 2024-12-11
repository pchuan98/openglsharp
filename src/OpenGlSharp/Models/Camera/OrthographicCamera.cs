namespace OpenGlSharp.Models;

public sealed class OrthographicCamera : Camera
{
    public override Matrix ProjectionMatrix { get; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="l"></param>
    /// <param name="r"></param>
    /// <param name="b"></param>
    /// <param name="t"></param>
    /// <param name="n"></param>
    /// <param name="f"></param>
    public OrthographicCamera(float l, float r, float b, float t, float n, float f)
    {
        ProjectionMatrix = Matrix.CreateOrthographicOffCenter(l, r, b, t, n, f);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="boxSize"></param>
    public OrthographicCamera(float boxSize)
    {
        ProjectionMatrix = Matrix.CreateOrthographicOffCenter(-boxSize, boxSize, -boxSize, boxSize, boxSize, -boxSize);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="near"></param>
    /// <param name="far"></param>
    public OrthographicCamera(float width, float height, float near, float far)
    {
        ProjectionMatrix = Matrix.CreateOrthographic(width, height, near, far);
    }
}
