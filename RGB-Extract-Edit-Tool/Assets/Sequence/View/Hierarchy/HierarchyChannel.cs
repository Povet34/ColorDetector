using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DataExtract
{
    public class HierarchyChannel : MonoBehaviour, IPanelChannel
    {
        public int channelIndex { get; set; }
        public Vector2 position { get; set; }

        [SerializeField] TMP_Text channelText;
        [SerializeField] TMP_InputField inputX;
        [SerializeField] TMP_InputField inputY;

        public void Init(CreateChannelParam param)
        {
            channelIndex = param.chIndex;
            position = param.createPos;

            channelText.text = $"CH {channelIndex}";
            inputX.text = ((int)position.x).ToString();
            inputY.text = ((int)position.y).ToString();
        }

        public void Move(MoveChannelParam param)
        {
            throw new System.NotImplementedException();
        }

        public void Destroy(DeleteChannelParam param)
        {
            throw new System.NotImplementedException();
        }
    }
}