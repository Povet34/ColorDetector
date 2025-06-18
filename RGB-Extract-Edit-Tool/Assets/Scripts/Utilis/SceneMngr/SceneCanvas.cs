using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneCanvas : MonoBehaviour
{
    [SerializeField] Button goDataEditButton;
    [SerializeField] Button goExtractButton;
    [SerializeField] Button goScenarioButton;
    [SerializeField] Button exitButton;

    private void Awake()
    {
        goDataEditButton.onClick.AddListener(() =>
        {
            SceneManager.LoadSceneAsync("DataEditScene");
        });

        goExtractButton.onClick.AddListener(() =>
        {
            SceneManager.LoadSceneAsync("ExtractScene");
        });

        goScenarioButton.onClick.AddListener(() =>
        {
            SceneManager.LoadSceneAsync("ScenarioVerificationScene");
        });

        exitButton.onClick.AddListener(() => { Application.Quit(); });
    }
}
