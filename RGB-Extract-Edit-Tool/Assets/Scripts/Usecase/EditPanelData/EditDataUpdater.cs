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

        public void Import(SaveData data)
        {
            editDataStore.Improt(data);
        }
    }
}
