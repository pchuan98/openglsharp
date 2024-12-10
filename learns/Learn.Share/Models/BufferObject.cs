using Silk.NET.OpenGL;

namespace Learn.Share.Models;

/// <summary>
/// VAO
/// VBO
/// VEO
/// </summary>
public class BufferObject<TDataType> : IDisposable
    where TDataType : unmanaged
{
    private readonly GL _gl;
    private readonly uint _handle;
    private readonly BufferTargetARB _target;

    public unsafe BufferObject(GL gl, Span<TDataType> data, BufferTargetARB target)
    {
        _gl = gl;
        _target = target;

        _handle = gl.GenBuffer();
        Bind();

        fixed (void* ptr = &data[0])
        {
            gl.BufferData(
                target,
                (nuint)(sizeof(TDataType) * data.Length),
                ptr,
                BufferUsageARB.StaticDraw);
        }
    }

    public uint Handle => _handle;

    public void Bind() => _gl.BindBuffer(_target, _handle);

    public void UnBind() => _gl.BindBuffer(_target, 0);

    public void Dispose()
    {
        _gl.DeleteBuffer(_handle);
    }
}