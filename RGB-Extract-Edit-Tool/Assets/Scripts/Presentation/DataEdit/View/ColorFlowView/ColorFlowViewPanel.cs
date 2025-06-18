using UnityEngine;
using System.Collections.Generic;

namespace DataEdit
{
    public class ColorFlowViewPanel : MonoBehaviour,
        IEventBusSubscriber<ChannelShowToggleArgs>,
        IEventBusSubscriber<RefreshPanelArgs>
    {
        #region Injection

        EditDataUpdater editDataUpdater;
        EditDataReceiver editDataReceiver;

        #endregion

        [SerializeField] Transform contentTr;
        [SerializeField] ColorFlowViewCell colorFlowViewCellPrefab;
        List<ColorFlowViewCell> colorFlowViewCells = new List<ColorFlowViewCell>();

        public void Init(DataEditMain.EditDataInjection editDataInjection)
        {
            editDataUpdater = editDataInjection.editDataUpdater;
            editDataReceiver = editDataInjection.editDataReceiver;
        }

        public void SubscribeEvent()
        {
            this.SubscribeEvent<RefreshPanelArgs>();
            this.SubscribeEvent<ChannelShowToggleArgs>();
        }

        public void UnsubscribeEvent()
        {
            this.UnsubscribeEvent<RefreshPanelArgs>();
            this.UnsubscribeEvent<ChannelShowToggleArgs>();
        }

        void OnEnable()
        {
            SubscribeEvent();
        }

        void OnDisable()
        {
            UnsubscribeEvent();
        }

        #region OnEventReceived

        // IEventBusSubscriber<ChannelShowToggleEventArgs>
        public void OnEventReceived(ChannelShowToggleArgs args)
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
            // 1. 기존 셀 모두 제거
            foreach (var cell in colorFlowViewCells)
            {
                if (cell != null)
                    Destroy(cell.gameObject);
            }
            colorFlowViewCells.Clear();

            switch (args.loadType)
            {
                case 0:
                    ViewInferred();
                    break;
                case 1:
                    ViewRaw();
                    break;
            }
        }

        private void ViewInferred()
        {
            // 2. 데이터 준비
            var inferredData = editDataReceiver.GetInferredColorData(); // Dictionary<int, List<Step>>
            var colorPalette = editDataReceiver.GetColorSheet(); // Dictionary<int, Color32>

            if (inferredData == null || colorPalette == null)
                return;

            // 3. 셀 생성 및 초기화
            foreach (var kv in inferredData)
            {
                int channelIndex = kv.Key;
                var steps = kv.Value;

                var colorIndices = new List<int>();
                foreach (var step in steps)
                    colorIndices.Add(step.colorindex);

                var createInfo = new ColorFlowViewCell.CreateInfo
                {
                    channelIndex = channelIndex,
                    colorIndices = colorIndices,
                    colorPalette = colorPalette
                };

                var cell = Instantiate(colorFlowViewCellPrefab, contentTr);
                cell.Init(createInfo);
                colorFlowViewCells.Add(cell);
            }
        }

        private void ViewRaw()
        {
            var recordedData = editDataReceiver.GetRecordedData();
            var colorPalette = editDataReceiver.GetColorSheet(); // Dictionary<int, Color32>

            if (recordedData == null)
                return;

            foreach (var kv in recordedData)
            {
                int channelIndex = kv.Key.index;
                var colors = kv.Value.colors;

                var createInfo = new ColorFlowViewCell.CreateInfo
                {
                    channelIndex = channelIndex,
                    colorIndices = null,
                    colorPalette = colorPalette,
                    rawColors = colors
                };

                var cell = Instantiate(colorFlowViewCellPrefab, contentTr);
                cell.InitRaw(createInfo);
                colorFlowViewCells.Add(cell);
            }
        }

        #endregion
    }
}