using UnityEngine;
using UnityEngine.UI;

namespace DataEdit
{
    public class DataEditTool : MonoBehaviour
    {
        [SerializeField] Button loadFromExcelButton;

        public void Init()
        {
            if (loadFromExcelButton != null)
                loadFromExcelButton.onClick.AddListener(OnLoadFromExcelButtonClicked);
        }

        void OnDestroy()
        {
            if (loadFromExcelButton != null)
                loadFromExcelButton.onClick.RemoveListener(OnLoadFromExcelButtonClicked);
        }

        private void OnLoadFromExcelButtonClicked()
        {
            Bus<LoadFromExcelEventArgs>.Raise(new LoadFromExcelEventArgs());
        }
    }
}