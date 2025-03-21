using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Data�� ��ũ�� �����ִ� ������ �Ѵ�.
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
    public Action<IPanelSync> onSync; //�����Ϳ� �ִ� ������ �����ͼ� �����Ѵ�.

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
