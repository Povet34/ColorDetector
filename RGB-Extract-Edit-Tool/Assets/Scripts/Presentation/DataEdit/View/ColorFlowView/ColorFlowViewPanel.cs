using UnityEngine;
using System.Collections.Generic;

namespace DataEdit
{
    public class ColorFlowViewPanel : MonoBehaviour,
        IEventBusSubscriber<ChannelShowToggleEventArgs>,
        IEventBusSubscriber<RefreshPanelArgs>
    {
        #region Injection

        EditDataUpdater editDataUpdater;
        EditDataReceiver editDataReceiver;

        #endregion

        [SerializeField] ColorFlowViewCell colorFlowViewCellPrefab;
        public List<ColorFlowViewCell> colorFlowViewCells = new List<ColorFlowViewCell>();

        public void Init(DataEditMain.EditDataInjection editDataInjection)
        {
            editDataUpdater = editDataInjection.editDataUpdater;
            editDataReceiver = editDataInjection.editDataReceiver;
        }

        public void SubscribeEvent()
        {
            this.SubscribeEvent<RefreshPanelArgs>();
            this.SubscribeEvent<ChannelShowToggleEventArgs>();
        }

        public void UnsubscribeEvent()
        {
            this.UnsubscribeEvent<RefreshPanelArgs>();
            this.UnsubscribeEvent<ChannelShowToggleEventArgs>();
        }

        void OnEnable()
        {
            SubscribeEvent();
        }

        void OnDisable()
        {
            UnsubscribeEvent();
        }

        // IEventBusSubscriber<ChannelShowToggleEventArgs>
        public void OnEventReceived(ChannelShowToggleEventArgs args)
        {
            foreach (var cell in colorFlowViewCells)
            {
                if (cell.channelIndex == args.channelIndex)
                {
                    cell.gameObject.SetActive(args.IsOn);
                    break;
                }
            }
        }

        // IEventBusSubscriber<RefreshPanelArgs>
        public void OnEventReceived(RefreshPanelArgs args)
        {

        }
    }
}