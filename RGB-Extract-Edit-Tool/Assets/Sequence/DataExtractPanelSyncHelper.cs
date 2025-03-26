using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static IPanelSync;

/// <summary>
/// Data의 싱크를 맞춰주는 역할을 한다.
/// </summary>
public class DataExtractPanelSyncHelper
{
    #region Singleton

    static DataExtractPanelSyncHelper _inst;

    public static DataExtractPanelSyncHelper GetInstance()
    {
        if (_inst == null)
        {
            _inst = new DataExtractPanelSyncHelper();
        }

        return _inst;
    }

    #endregion

    DataController dataController;

    //Channel
    public Action<CreateChannelParam> onChannelCreate;
    public Action<ChannelMoveParam> onChannelMove;
    public Action<DeleteChannelParam> onChannelDelete;

    /// <summary>
    /// arg1 : channel index
    /// arg2 : group index
    /// </summary>
    public Action<IPanelSync, int, int> onChannelExcludeGroup;

    /// <summary>
    /// arg1 : channel index
    /// arg2 : group index
    /// </summary>
    public Action<IPanelSync, int, int> onChannelIncludeGroup;

    //Group
    public Action<IPanelSync, int, Vector2> onGroupCreate;
    public Action<IPanelSync, int> onGroupDelete;

    public DataExtractPanelSyncHelper()
    {
        dataController = new DataController();
    }

    public void CreateChannel(CreateChannelParam param)
    {
        dataController.CreateChannel(param.chIndex, param.createPos);
        onChannelCreate?.Invoke(param);
    }

    public void ChannelMove(ChannelMoveParam param)
    {
        dataController.MoveChannel(param.chIndex, param.position);
        onChannelMove?.Invoke(param);
    }

    public void DeleteChannel(DeleteChannelParam param)
    {
        dataController.DeleteChannel(param.indexes);
        onChannelDelete?.Invoke(param);
    }
}
