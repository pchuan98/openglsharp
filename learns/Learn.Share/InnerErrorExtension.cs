﻿using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Silk.NET.OpenGL;

namespace Learn.Share;

/// <summary>
/// Throw OpenGl inner error
/// </summary>
public static class InnerErrorExtension
{
    [Conditional("DEBUG")]
    public static void DetectLastError(
        this GL gl,
        [CallerFilePath] string? path = null,
        [CallerMemberName] string? method = null,
        [CallerLineNumber] int line = 0)
    {
        var error = gl.GetError();

        if (error == GLEnum.NoError)
            return;
    }

    public static void DetectShaderError(
        this GL gl,
        uint handle,
        [CallerFilePath] string? path = null,
        [CallerMemberName] string? method = null,
        [CallerLineNumber] int line = 0)
    {
        //gl.GetShader(handle,GLEnum.ShaderCompiler,out var state)

        var error = gl.GetShaderInfoLog(handle);

        if (string.IsNullOrWhiteSpace(error))
            return;

        throw new ValidationException(error);
    }

    public static void DetectLinkError(
        this GL gl,
        uint handle,
        [CallerFilePath] string? path = null,
        [CallerMemberName] string? method = null,
        [CallerLineNumber] int line = 0)
    {
        gl.GetProgram(handle, GLEnum.LinkStatus, out var code);

        if (code != 0)
            return;

        var logs = gl.GetProgramInfoLog(handle);
        throw new ValidationException(logs);
    }
}