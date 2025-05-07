using SFB;

public class LocalFileLoader_Video : ILocalFileLoader
{
    string[] filter = new string[3] { "mp4", "mov", "avi" };

    public string OpenFilePath()
    {
        var extensions = new[] {
            new ExtensionFilter("Video Files", filter),
            new ExtensionFilter("All Files", "*" ),
        };
        var paths = StandaloneFileBrowser.OpenFilePanel("Open Video", "", extensions, false);

        if (paths.Length > 0)
            return paths[0];

        return null;
    }
}