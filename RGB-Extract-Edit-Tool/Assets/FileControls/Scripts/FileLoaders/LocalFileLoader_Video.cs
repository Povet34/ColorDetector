using SFB;

public class LocalFileLoader_Video : ILocalFileLoader
{
    string[] filter = new string[3] { "mp4", "mov", "avi" };
    
    ILoadDataStore loadDataStore;

    public LocalFileLoader_Video(ILoadDataStore loadDataStore)
    {
        this.loadDataStore = loadDataStore;
    }

    public void OpenFilePath()
    {
        var extensions = new[] {
            new ExtensionFilter("Video Files", filter),
            new ExtensionFilter("All Files", "*" ),
        };
        var paths = StandaloneFileBrowser.OpenFilePanel("Open Video", "", extensions, false);

        if (paths.Length > 0)
            loadDataStore.SetVideo(paths[0]);
    }
}