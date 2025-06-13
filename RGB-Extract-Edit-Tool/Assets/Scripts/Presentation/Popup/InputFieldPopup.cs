using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class InputFieldPopup : MonoBehaviour
{
    [SerializeField] TMP_Text titleText;
    [SerializeField] TMP_InputField inputField;

    [SerializeField] Button confirmButton;
    [SerializeField] Button cancelButton;

    UnityAction<string> onConfirm;
    UnityAction onCancel;

    private void Awake()
    {
        confirmButton.onClick.AddListener(
            () =>
            {
                onConfirm?.Invoke(inputField.text);
                Destroy(gameObject);
            });

        cancelButton.onClick.AddListener(
            () =>
            {
                onCancel?.Invoke();
                Destroy(gameObject);
            });
    }

    public void ShowPopup(string Title, UnityAction<string> confirm, UnityAction cancel = null)
    {
        onConfirm = confirm;
        onCancel = cancel;
        titleText.text = Title;
    }

    public static InputFieldPopup CreateAndShowPopup(Transform parent, string Title, UnityAction<string> confirm, UnityAction cancel = null)
    {
        GameObject popupObject = Instantiate(Resources.Load<GameObject>("Popup/InputFieldPupup"), parent);
        InputFieldPopup popup = popupObject.GetComponent<InputFieldPopup>();
        popup.ShowPopup(Title, confirm, cancel);
        return popup;
    }
}
