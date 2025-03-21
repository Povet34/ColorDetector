using UnityEngine;
using UnityEngine.UI;

public class VideoViewPanel : MonoBehaviour, IPanelSync
{

    //���⿡
    //VideoViewChannel
    //VideoViewSequenceGroup
    //�̷��� �ִٰ� ���� ������ �Ǹ� Apply�Ѵ�.

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
