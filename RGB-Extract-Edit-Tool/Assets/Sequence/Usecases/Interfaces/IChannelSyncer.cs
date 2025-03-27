using System.Collections.Generic;
using UnityEngine;


namespace DataExtract
{
    public interface IChannelSyncer
    {
        List<IPanelSync> panels { get; set; }
        void Init(List<IPanelSync> panels);
        void SyncAllPanel(IPanelSync ownPanel, EditParam param);
    }
}