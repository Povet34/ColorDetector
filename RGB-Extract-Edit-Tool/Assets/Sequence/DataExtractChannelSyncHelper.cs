using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Data의 싱크를 맞춰주는 역할을 한다.
/// </summary>
public class DataExtractChannelSyncHelper : MonoBehaviour
{
    #region Singleton

    static DataExtractChannelSyncHelper _inst;

    public static DataExtractChannelSyncHelper GetInstance()
    {
        if (_inst == null)
        {
            _inst = FindAnyObjectByType<DataExtractChannelSyncHelper>();
        }
            
        if (_inst == null)
        {
            _inst = new GameObject(nameof(DataExtractChannelSyncHelper)).AddComponent<DataExtractChannelSyncHelper>();
        }

        return _inst;
    }

    #endregion

    public DataController dataController;
    public Action<IPanelSync> onSync; //데이터에 있는 정보를 가져와서 적용한다.

    //Channel
    public Action<int, Vector2> onChannelMove;
    public Action<int, Vector2> onChannelCreate;
    public Action<int> onChannelDelete;

    /// <summary>
    /// arg1 : channel index
    /// arg2 : group index
    /// </summary>
    public Action<int, int> onChannelExcludeGroup;

    /// <summary>
    /// arg1 : channel index
    /// arg2 : group index
    /// </summary>
    public Action<int, int> onChannelIncludeGroup;

    //Group
    public Action<int, Vector2> onGroupCreate;
    public Action<int> onGroupDelete;

    public void Apply(IPanelSync ownerPanel)
    {
        onSync?.Invoke(ownerPanel);
    }

    public void CreateChannel(IPanelSync ownerPanel, IChannel.InitInfo info)
    {
        dataController.CreateChannel(info);
        Apply(ownerPanel);
    }
}
