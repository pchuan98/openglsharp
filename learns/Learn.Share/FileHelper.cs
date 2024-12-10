using StbImageSharp;

namespace Learn.Share;

public static class FileHelper
{
    public static (int width, int height, byte[] data) ReadImage(string path, ColorComponents color = ColorComponents.RedGreenBlueAlpha)
    {
        if (!File.Exists(path))
            throw new FileNotFoundException($"Image file not found.", path);

        // flip y to adjust opengl coord
        StbImage.stbi_set_flip_vertically_on_load(1);

        var result = ImageResult.FromMemory(File.ReadAllBytes(path), color);
        return (result.Width, result.Height, result.Data);
    }

    /// <summary>
    /// 同分支目录查找
    /// </summary>
    /// <param name="name"></param>
    /// <param name="index">最大向上查找次数</param>
    /// <returns></returns>
    public static string? FindFolder(string name, int index = 5)
    {
        var root = Directory.GetCurrentDirectory();

        do
        {
            if (string.IsNullOrWhiteSpace(root)) return null;
            var folder = Path.Join(root, name);

            if (Directory.Exists(folder)) return folder;

            root = Directory.GetParent(root)?.FullName;

        } while (index-- > 0);

        return null;
    }
}