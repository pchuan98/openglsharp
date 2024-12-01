using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Silk.NET.OpenGL;

namespace OpenGlSharp.Models;

public class VertextArrrayObject<TVboType, TEboType> : IDisposable
    where TVboType : unmanaged
    where TEboType : unmanaged
{
    private readonly GL _gl;

    private readonly uint _handle = 0;

    public VertextArrrayObject(GL gl, BufferObject<TVboType> vbo, BufferObject<TEboType> ebo)
    {
        _gl = gl;

        _handle = _gl.GenVertexArray();
        Bind();

        vbo.Bind();
        ebo.Bind();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="index">prop index of vertex</param>
    /// <param name="size">the number of element</param>
    /// <param name="type">value type</param>
    /// <param name="count">the number of set of data</param>
    /// <param name="offset">the number of offsets in each set of data</param>
    public unsafe void VertexAttributePointer(uint index, int size, VertexAttribPointerType type, uint count, uint offset)
    {
        _gl.EnableVertexAttribArray(index);

        _gl.VertexAttribPointer(
            index,
            size,
            type,
            false,
            count * (uint)sizeof(TVboType),
            (void*)(offset * (uint)sizeof(TVboType)));
    }

    public void Bind() => _gl.BindVertexArray(_handle);

    public void Dispose()
    {
        throw new NotImplementedException();
    }
}