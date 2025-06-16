using DataExtract;
using System.Collections.Generic;
using UnityEngine;

public class PanelSyncer
{
    List<IPanelSync> _panels;

    public PanelSyncer(List<IPanelSync> panels)
    {
        this.panels = panels;
    }

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

    public void Refresh(EditParam param)
    {
        foreach (var panel in panels)
        {
            panel.Refresh(param as RefreshParam);
        }
    }
}
