using OpenGlSharp.Extensions;
using Silk.NET.OpenGL;

namespace OpenGlSharp.Models;

public partial class Shader : IDisposable
{
    private readonly GL _gl;

    private readonly uint _handle;

    public Shader(GL gl, string vertex, string fragment)
    {
        _gl = gl;

        // vertext
        var vertextShader = Load(ShaderType.VertexShader, vertex);
        var fragmentShader = Load(ShaderType.FragmentShader, fragment);

        // shader program
        _handle = _gl.CreateProgram();
        _gl.AttachShader(_handle, vertextShader);
        _gl.AttachShader(_handle, fragmentShader);
        _gl.LinkProgram(_handle);
        _gl.DetectLinkError(_handle);

        // detach
        _gl.DetachShader(_handle, vertextShader);
        _gl.DetachShader(_handle, fragmentShader);
        _gl.DeleteShader(vertextShader);
        _gl.DeleteShader(fragmentShader);
    }

    public void Use() => _gl.UseProgram(_handle);

    public uint Handle => _handle;

    public void Dispose() => _gl.DeleteProgram(_handle);

    private uint Load(ShaderType type, string sharder)
    {
        var handle = _gl.CreateShader(type);

        _gl.ShaderSource(handle, sharder);
        _gl.CompileShader(handle);

        _gl.DetectShaderError(handle);

        return handle;
    }
}

partial class Shader
{
    public void Uniform1(string name, float value)
        => _gl.Uniform1(_gl.GetUniformLocation(Handle, name), value);

}