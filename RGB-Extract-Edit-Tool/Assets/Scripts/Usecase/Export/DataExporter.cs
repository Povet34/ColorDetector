using System.Collections.Generic;

public class DataExporter
{
    IToExport toExport;

    public DataExporter(IToExport toExport)
    {
        this.toExport = toExport;
    }

    public void Export(Dictionary<SavedChannelKey, SavedChannelValue> data, string filePath)
    {
        toExport.Export(data, filePath);
    }
}
