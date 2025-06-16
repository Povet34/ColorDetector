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

    public SaveData Import()
    {
        return importFrom.Import(localFileLoader_Excel.OpenFilePath());
    }

    public List<SavedChannelKey> LoadChannelInfos()
    {
        return importFrom.LoadChannelInfos(localFileLoader_Excel.OpenFilePath());
    }
}
