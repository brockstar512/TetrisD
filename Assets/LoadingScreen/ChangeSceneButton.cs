using UnityEngine;
using UnityEngine.UI;

//using UnityEvents;

public class ChangeSceneButton : MonoBehaviour
{
    public Scenes targetScene;
    private Button button;

    private void Start()
    {
        button = this.GetComponent<Button>();
        button.onClick.AddListener(delegate { LoadingManager.Instance.LoadScene(targetScene); });
    }
    public void ChangeScene()
    {
        LoadingManager.Instance.LoadScene(targetScene);
    }

    private void OnDestroy()
    {
        button = this.GetComponent<Button>();
        button.onClick.RemoveAllListeners();
    }
}



public enum Scenes
{
    MainMenu,
    Dice,
    Game
}
