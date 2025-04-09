using DataExtract;
using System.Collections.Generic;
using UnityEngine;

public class ChannelSyncer
{
    List<IPanelSync> _panels;
    public List<IPanelSync> panels { get => _panels; set => _panels = value; }

    public void Init(List<IPanelSync> panels)
    {
        _panels = panels;
    }

    public void SyncAllPanel(IPanelSync ownPanel, EditParam param)
    {
        //DLogger.Log_Yellow($"{ownPanel.GetType()} : {param.editType}");

        foreach (var panel in panels)
        {
            if (panel != ownPanel)
            {
                panel.Sync(param);
            }
        }
    }
}
