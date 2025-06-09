using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneCanvas : MonoBehaviour
{
    [SerializeField] Button goGraphButton;
    [SerializeField] Button goExtractButton;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        goGraphButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("GraphScene");
        });

        goExtractButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("ExtractScene");
        });
    }
}
