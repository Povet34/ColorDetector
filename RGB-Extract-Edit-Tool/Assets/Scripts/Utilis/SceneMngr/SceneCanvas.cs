using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneCanvas : MonoBehaviour
{
    [SerializeField] Button goDataEditButton;
    [SerializeField] Button goExtractButton;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        goDataEditButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("DataEditScene");
        });

        goExtractButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("ExtractScene");
        });
    }
}
