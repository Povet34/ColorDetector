using SFB;

public class LocalFileLoader_Excel : ILocalFileLoader
{
    string[] filter = new string[2] { "xlsx", "xls" };

    public string OpenFilePath()
    {
        var extensions = new[] {
            new ExtensionFilter("Excel Files", filter),
            new ExtensionFilter("All Files", "*" ),
        };
        var paths = StandaloneFileBrowser.OpenFilePanel("Open Excel File", "", extensions, false);

        if (paths.Length > 0)
            return paths[0];

        return null;
    }
}
