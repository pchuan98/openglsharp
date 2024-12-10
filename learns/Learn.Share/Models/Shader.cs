using System.Numerics;
using Silk.NET.OpenGL;

namespace Learn.Share.Models;

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

// todo 使用更加抽象的方式写这个
partial class Shader
{
    public void Uniform1(string name, float value) =>
        _gl.Uniform1(_gl.GetUniformLocation(Handle, name), value);

    public void Uniform1(string name, int value) =>
        _gl.Uniform1(_gl.GetUniformLocation(Handle, name), value);

    public void UniformMatrix33(string name, Span<float> matrix, bool transpose = false) =>
        _gl.UniformMatrix3(_gl.GetUniformLocation(Handle, name), transpose, matrix);

    public void UniformMatrix44(string name, Span<float> matrix, bool transpose = false) =>
        _gl.UniformMatrix4(_gl.GetUniformLocation(Handle, name), transpose, matrix);

    public unsafe void UniformMatrix44(string name, Matrix4x4 matrix, bool transpose = false)
    {
        var location = _gl.GetUniformLocation(_handle, name);
        if (location == -1)
            throw new Exception($"{name} uniform not found on shader.");

        _gl.UniformMatrix4(location, 1, transpose, (float*)&matrix);
    }

    public void UniformMatrix33(string name, Span<double> matrix, bool transpose = false) =>
        _gl.UniformMatrix3(_gl.GetUniformLocation(Handle, name), transpose, matrix);
}
