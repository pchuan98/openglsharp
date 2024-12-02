using Silk.NET.OpenGL;

// todo 等更熟悉了再改写这个

namespace OpenGlSharp.Models;

public partial class Shader2 : IDisposable
{
    /// <summary>
    /// gl obj
    /// </summary>
    public GL Gl { get; private set; }

    /// <summary>
    /// program handle
    /// </summary>
    public uint Handle { get; init; }

    /// <summary>
    /// 
    /// </summary>
    public void Dispose() => Gl.DeleteProgram(Handle);
}

partial class Shader2
{
    public Shader2(GL gl, uint? program = null)
    {
        Gl = gl;
        Handle = program ?? Gl.CreateProgram();
    }
}
