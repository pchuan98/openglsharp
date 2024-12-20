﻿using Silk.NET.OpenGL;
using StbImageSharp;

namespace OpenGlSharp.Models;

public partial class Texture : IDisposable
{
    private readonly GL _gl;

    public uint Handle { get; init; }

    public void Dispose() => _gl.DeleteTexture(Handle);
}

partial class Texture
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="gl"></param>
    /// <param name="data">
    /// Rgba Image Byte Data
    /// </param>
    /// <param name="width"></param>
    /// <param name="height"></param>
    /// <param name="action"></param>
    public unsafe Texture(GL gl, Span<byte> data, uint width, uint height, Action? action = null)
    {
        _gl = gl;
        Handle = _gl.GenTexture();

        Bind();

        fixed (void* ptr = &data[0])
        {
            _gl.TexImage2D(
                TextureTarget.Texture2D,
                0,
                InternalFormat.Rgba,
                width,
                height,
                0,
                PixelFormat.Rgba,
                PixelType.UnsignedByte,
                ptr);

            action ??= SetParams;
            action?.Invoke();
        }

        _gl.GenerateMipmap(TextureTarget.Texture2D);
    }

    public static Texture LoadFromFile(GL gl, string file, Action? action = null)
    {
        if (!File.Exists(file))
            throw new FileNotFoundException("Image file not found", file);

        StbImage.stbi_set_flip_vertically_on_load(1);

        var img = ImageResult.FromMemory(File.ReadAllBytes(file), ColorComponents.RedGreenBlueAlpha);
        if (img is null)
            throw new InvalidOperationException("Image load failed");

        return new Texture(gl, img.Data, (uint)img.Width, (uint)img.Height, action);
    }
}

partial class Texture
{
    public void Bind(TextureUnit slot = TextureUnit.Texture0)
    {
        _gl.ActiveTexture(slot);
        _gl.BindTexture(TextureTarget.Texture2D, Handle);
    }

    public void SetParams()
    {
        // Wrap strq -> xyz coor
        _gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)GLEnum.NearestMipmapNearest);
        _gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)GLEnum.NearestMipmapNearest);

        // filter
        _gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)GLEnum.NearestMipmapNearest);
        _gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)GLEnum.NearestMipmapNearest);
    }
}