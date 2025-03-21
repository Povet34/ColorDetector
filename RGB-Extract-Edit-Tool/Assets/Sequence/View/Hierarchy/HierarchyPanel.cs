using UnityEngine;

public class HierarchyPanel : MonoBehaviour, IPanelSync
{
    #region IPanelSync

    public void Sync(IPanelSync owner)
    {
        if (this.Equals(owner))
            return;

        throw new System.NotImplementedException();
    }

    void Start()
    {
        RegistSyncEvent();
    }

    void OnDestroy()
    {
        UnregistSyncEvent();
    }


    public void RegistSyncEvent()
    {
        DataExtractChannelSyncHelper.GetInstance().onSync += Sync;
    }

    public void UnregistSyncEvent()
    {
        DataExtractChannelSyncHelper.GetInstance().onSync -= Sync;
    }

    #endregion

    // Update is called once per frame
    void Update()
    {
        
    }

}
