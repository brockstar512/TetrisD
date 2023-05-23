using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class GameSetUp : MonoBehaviour
{
    public static GameType gameType;
    public static int difficulty;

    public enum GameType
    {
        Marathon,
        TimeAttack,
        LineBreaker
    }
    public string[] gameDescriptions =
    {
        "See how long you can last before reaching the top of The board!",
        "You've got a minute to get as many points as you can!",
        "Make your lines count. You only can eliminate 10!"
    };
    [SerializeField] Slider slider;
    [SerializeField] Button Play;
    [SerializeField] TextMeshProUGUI description;
    [SerializeField] TextMeshProUGUI highScore;
    [SerializeField] Toggle marathonToggle;
    [SerializeField] Toggle timeToggle;
    [SerializeField] Toggle lineToggle;


    private void Awake()
    {
        slider.onValueChanged.AddListener((val) => difficulty = (int)val);
        Play.onClick.AddListener(OnPlay);

        marathonToggle.onValueChanged.AddListener(delegate { HandleToggle(marathonToggle); });
        timeToggle.onValueChanged.AddListener(delegate { HandleToggle(timeToggle); });
        lineToggle.onValueChanged.AddListener(delegate { HandleToggle(lineToggle); });


        HandleToggle(marathonToggle);
    }
    private void OnEnable()
    {
        slider.value = difficulty;
    }

    void HandleToggle(Toggle toggle)
    {
        if (!toggle.isOn)
            return;
        int index = toggle.gameObject.transform.GetSiblingIndex();
        description.text = gameDescriptions[index];

        switch (index)
        {
            case 0:
                gameType = GameType.Marathon;
                highScore.text = DataStore.Instance.playerData.marathonHighscore.ToString();
                break;
            case 1:
                gameType = GameType.TimeAttack;
                highScore.text = DataStore.Instance.playerData.timeAttackHighscore.ToString();
                break;
            case 2:
                gameType = GameType.LineBreaker;
                highScore.text = DataStore.Instance.playerData.lineBreakerHighscore.ToString();
                break;
        }

    }
    private void OnPlay()
    {
        Debug.Log($"The game mode is {gameType}");
        Debug.Log($"The difficulty is {difficulty}");

    }

    private void OnDestroy()
    {
        slider.onValueChanged.RemoveAllListeners();
        Play.onClick.RemoveAllListeners();
        marathonToggle.onValueChanged.RemoveAllListeners();
        timeToggle.onValueChanged.RemoveAllListeners();
        lineToggle.onValueChanged.RemoveAllListeners();
    }

}
