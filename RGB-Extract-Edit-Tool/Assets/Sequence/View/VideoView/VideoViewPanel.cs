using UnityEngine;
using UnityEngine.UI;

public class VideoViewPanel : MonoBehaviour, IPanelSync
{

    //여기에
    //VideoViewChannel
    //VideoViewSequenceGroup
    //이런게 있다가 뭔가 조작이 되면 Apply한다.

    RawImage videoView;

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

    void Update()
    {
        
    }
}
