using UnityEngine;
using UnityEngine.UI;

public class VideoViewPanel : MonoBehaviour, IPanelSync
{

    //여기에
    //VideoViewChannel
    //VideoViewSequenceGroup
    //이런게 있다가 뭔가 조작이 되면 Apply한다.

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
