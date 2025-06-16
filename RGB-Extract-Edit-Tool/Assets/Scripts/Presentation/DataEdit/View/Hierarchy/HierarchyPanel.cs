using UnityEngine;
using UnityEngine.UI;

namespace DataEdit
{
    public class HierarchyPanel : MonoBehaviour, IEventBusSubscriber<RefreshPanelArgs>
    {
        #region Injection

        EditDataUpdater editDataUpdater;
        EditDataReceiver editDataReceiver;

        #endregion

        [SerializeField] Transform contentTr;
        [SerializeField] EditHierarchyChannelCell editHierarchyChannelCellPrefab;

        void OnEnable()
        {
            this.SubscribeEvent<RefreshPanelArgs>();
        }

        void OnDisable()
        {
            this.UnsubscribeEvent<RefreshPanelArgs>();
        }

        public void Init(DataEditMain.EditDataInjection editDataInjection)
        {
            editDataUpdater = editDataInjection.editDataUpdater;
            editDataReceiver = editDataInjection.editDataReceiver;
        }

        public void OnEventReceived(RefreshPanelArgs args)
        {
            // 1. 기존 셀 모두 제거
            foreach (Transform child in contentTr)
            {
                Destroy(child.gameObject);
            }

            // 2. 채널 데이터 받아오기
            var channelKeys = editDataReceiver.GetChannelData();
            if (channelKeys == null)
                return;

            // 3. 채널별로 셀 생성 및 초기화
            foreach (var key in channelKeys)
            {
                var cell = Instantiate(editHierarchyChannelCellPrefab, contentTr);
                // EditHierarchyChannelCell.Init(int channelIndex, int groupIndex, int groupInindex, bool isOn)
                cell.Init(key.index, key.groupInindex, key.groupInindex, true); // groupIndex는 상황에 맞게 할당
            }
        }
    }
}

