using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Silk.NET.OpenGL;

namespace OpenGlSharp.Models;


public class VertextArrrayObject<TVboType> : IDisposable
    where TVboType : unmanaged
{
    private readonly GL _gl;

    private readonly uint _handle = 0;

    private uint _defaltIndex = 0;

    public VertextArrrayObject(GL gl, BufferObject<TVboType> vbo)
    {
        _gl = gl;

        _handle = _gl.GenVertexArray();
        Bind();

        vbo.Bind();
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

    /// <summary>
    /// 
    /// </summary>
    /// <param name="type"></param>
    /// <param name="total">每组数据总个数</param>
    /// <param name="elements">当前组数据的element数量</param>
    /// <param name="offset">element相较于每组数据的偏移</param>
    public void AddVertexAttributePointer(VertexAttribPointerType type, uint total, int elements, uint offset)
    {
        VertexAttributePointer(_defaltIndex, elements, type, total, offset);
        _defaltIndex++;
    }

    public void Bind() => _gl.BindVertexArray(_handle);

    public void Dispose() => _gl.DeleteVertexArray(_handle);
}

public class VertextArrrayObject<TVboType, TEboType> : IDisposable
    where TVboType : unmanaged
    where TEboType : unmanaged
{
    private readonly GL _gl;

    private readonly uint _handle = 0;

    private uint _defaltIndex = 0;

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

    /// <summary>
    /// 
    /// </summary>
    /// <param name="type"></param>
    /// <param name="total">每组数据总个数</param>
    /// <param name="elements">当前组数据的element数量</param>
    /// <param name="offset">element相较于每组数据的偏移</param>
    public void AddVertexAttributePointer(VertexAttribPointerType type, uint total, int elements, uint offset)
    {
        VertexAttributePointer(_defaltIndex, elements, type, total, offset);
        _defaltIndex++;
    }

    public void Bind() => _gl.BindVertexArray(_handle);

    public void Dispose() => _gl.DeleteVertexArray(_handle);

}

