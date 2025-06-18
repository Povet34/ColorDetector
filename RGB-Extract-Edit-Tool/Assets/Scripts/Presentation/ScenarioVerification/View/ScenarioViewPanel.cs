using UnityEngine;
using System.Collections.Generic;
using System.Collections;

namespace ScenarioVerification
{
    public class ScenarioViewPanel : MonoBehaviour, 
        IEventBusSubscriber<RefreshPanelArgs>,
        IEventBusSubscriber<PlayScenarioArgs>
    {
        #region Injection

        ScenarioDataReceiver scenarioDataReceiver;

        #endregion

        [SerializeField] ScenarioViewChannel scenarioViewChannelPrefab;

        List<ScenarioViewChannel> channels = new List<ScenarioViewChannel>();

        Dictionary<int, List<Color32>> origin;
        Dictionary<int, List<Color32>> modified;


        private void Awake()
        {
            this.SubscribeEvent<RefreshPanelArgs>();
            this.SubscribeEvent<PlayScenarioArgs>();
        }

        private void OnDestroy()
        {
            this.UnsubscribeEvent<RefreshPanelArgs>();
            this.UnsubscribeEvent<PlayScenarioArgs>();
        }

        public void Init(ScenarioVerificationMain.DataControllInjection dataControllInjection)
        {
            scenarioDataReceiver = dataControllInjection.scenarioDataReceiver;
        }

        void PlayOrigin()
        {
            StartCoroutine(PlayBody(origin));
        }

        void PlayModified()
        {
            StartCoroutine(PlayBody(modified));
        }

        IEnumerator PlayBody(Dictionary<int, List<Color32>> data)
        {
            if (data == null || data.Count == 0 || channels.Count == 0)
                yield break;

            // ������ �� ���� (��� ä���� �ּ� ������ ��)
            int frameCount = int.MaxValue;
            foreach (var kv in data)
            {
                if (kv.Value != null && kv.Value.Count < frameCount)
                    frameCount = kv.Value.Count;
            }
            if (frameCount == int.MaxValue || frameCount == 0)
                yield break;

            float frameDelay = 0.05f; // 20fps (�ʿ�� ����)

            for (int frame = 0; frame < frameCount; frame++)
            {
                foreach (var channel in channels)
                {
                    if (data.TryGetValue(channel.channelIndex, out var colorList) && colorList != null && frame < colorList.Count)
                    {
                        channel.SetColor(colorList[frame]);
                    }
                }
                yield return new WaitForSeconds(frameDelay);
            }
        }

        #region OnEventReceived

        public void OnEventReceived(RefreshPanelArgs args)
        {
            // ���� ä�� ������Ʈ ����
            foreach (var channel in channels)
            {
                if (channel != null)
                    Destroy(channel.gameObject);
            }
            channels.Clear();

            // ä�� ������ ��������
            var channelKeys = scenarioDataReceiver.GetChannelDatas();
            if (channelKeys == null)
                return;

            foreach (var key in channelKeys)
            {
                var channelObj = Instantiate(scenarioViewChannelPrefab, transform);
                channelObj.channelIndex = key.index;
                channelObj.Init(key.position);
                channels.Add(channelObj);
            }

            // ����/���� ������ ��ȯ �� ����
            var orDict = scenarioDataReceiver.GetOriginDatas();
            var moDict = scenarioDataReceiver.GetModifiedDatas();

            origin = new Dictionary<int, List<Color32>>();
            if (orDict != null)
            {
                foreach (var kv in orDict)
                {
                    origin[kv.Key.index] = kv.Value.colors;
                }
            }

            modified = new Dictionary<int, List<Color32>>();
            if (moDict != null)
            {
                foreach (var kv in moDict)
                {
                    modified[kv.Key.index] = kv.Value.colors;
                }
            }
        }

        public void OnEventReceived(PlayScenarioArgs args)
        {
            switch(args.dataType)
            {
                case 0:
                    PlayOrigin();
                    break;
                case 1:
                    PlayModified();
                    break;
            }
        }

        #endregion
    }
}