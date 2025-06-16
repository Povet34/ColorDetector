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
            // 1. ���� �� ��� ����
            foreach (Transform child in contentTr)
            {
                Destroy(child.gameObject);
            }

            // 2. ä�� ������ �޾ƿ���
            var channelKeys = editDataReceiver.GetChannelData();
            if (channelKeys == null)
                return;

            // 3. ä�κ��� �� ���� �� �ʱ�ȭ
            foreach (var key in channelKeys)
            {
                var cell = Instantiate(editHierarchyChannelCellPrefab, contentTr);
                // EditHierarchyChannelCell.Init(int channelIndex, int groupIndex, int groupInindex, bool isOn)
                cell.Init(key.index, key.groupInindex, key.groupInindex, true); // groupIndex�� ��Ȳ�� �°� �Ҵ�
            }
        }
    }
}

