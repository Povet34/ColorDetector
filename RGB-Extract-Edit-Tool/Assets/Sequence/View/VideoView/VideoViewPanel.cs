using UnityEngine;
using UnityEngine.UI;

public class VideoViewPanel : MonoBehaviour, IPanelSync
{

    //���⿡
    //VideoViewChannel
    //VideoViewSequenceGroup
    //�̷��� �ִٰ� ���� ������ �Ǹ� Apply�Ѵ�.

    RawImage videoView;

    public void ChannelMove(IPanelSync.ChannelMoveParam param)
    {
        throw new System.NotImplementedException();
    }

    public void CreateChannel(IPanelSync.CreateChannelParam param)
    {
        throw new System.NotImplementedException();
    }

    public void DeleteChannel(IPanelSync.DeleteChannelParam param)
    {
        throw new System.NotImplementedException();
    }

    public void RegistSyncEvent()
    {
        throw new System.NotImplementedException();
    }

    public void UnregistSyncEvent()
    {
        throw new System.NotImplementedException();
    }
}
