using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGlSharp.Models;

public sealed class PerspectiveCamera : Camera
{
    public override Matrix ProjectionMatrix { get; init; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="fov"></param>
    /// <param name="aspect"></param>
    /// <param name="near"></param>
    /// <param name="far"></param>
    public PerspectiveCamera(float fov, float aspect, float near = 0.1f, float far = 1000f)
    {
        ProjectionMatrix = Matrix.CreatePerspectiveFieldOfView(fov, aspect, near, far);
    }
}