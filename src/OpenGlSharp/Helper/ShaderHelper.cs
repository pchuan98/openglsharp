using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OpenGlSharp.Helper;

public static class ShaderHelper
{
    /// <summary>
    /// 
    /// </summary>
    public static string VertexShader => File.ReadAllText("Shaders/shader.vert");

    /// <summary>
    /// 
    /// </summary>
    public static string FragmentShader => File.ReadAllText("Shaders/shader.frag");
}