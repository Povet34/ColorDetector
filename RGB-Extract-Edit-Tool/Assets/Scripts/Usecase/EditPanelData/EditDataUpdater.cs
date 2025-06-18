using System.Collections.Generic;
using UnityEngine;

namespace DataEdit
{
    public class EditDataUpdater
    {
        IEditDataStore editDataStore;
        public EditDataUpdater(IEditDataStore editDataStore)
        {
            this.editDataStore = editDataStore;
        }

        public void Import(ImportResult importResult)
        {
            editDataStore.Improt(importResult);
        }

        public void UpdateRawDataAll(Dictionary<SavedChannelKey, SavedChannelValue> channelData)
        {
            editDataStore.UpdateRawDataAll(channelData);
        }

        public void UpdateRawData_OneChannel(int channelIndex, List<Color32> colors)
        {
            editDataStore.UpdateRawData_OneChannel(channelIndex, colors);
        }

        public void SetCurrentEditDataPath(string path)
        {
            editDataStore.path = path;
        }
    }
}
