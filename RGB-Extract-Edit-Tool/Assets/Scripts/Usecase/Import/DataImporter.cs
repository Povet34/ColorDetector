using UnityEngine;
using System.Collections.Generic;

public class DataImporter
{
    IImportFrom importFrom;
    LocalFileLoader_Excel localFileLoader_Excel;

    public DataImporter(IImportFrom importFrom, LocalFileLoader_Excel localFileLoader_Excel)
    {
        this.importFrom = importFrom;
        this.localFileLoader_Excel = localFileLoader_Excel;
    }
    public Dictionary<SavedChannelKey, SavedChannelValue> Import(string filePath)
    {
        return importFrom.Import(filePath);
    }

    public List<SavedChannelKey> LoadChannelInfos()
    {
        return importFrom.LoadChannelInfos(localFileLoader_Excel.OpenFilePath());
    }
}
