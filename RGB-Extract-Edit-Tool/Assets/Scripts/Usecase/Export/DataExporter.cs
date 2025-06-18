using System.Collections.Generic;

public class DataExporter
{
    IToExport toExport;

    public DataExporter(IToExport toExport)
    {
        this.toExport = toExport;
    }

    public void ExportNew(OriginData data, string filePath)
    {
        toExport.ExportNew(data, filePath);
    }

    public void ExportAdd(AdditionalData data, string filePath)
    {
        toExport.ExportAdd(data, filePath);
    }
}
