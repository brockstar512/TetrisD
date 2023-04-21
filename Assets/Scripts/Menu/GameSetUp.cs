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
    [SerializeField] ToggleGroup gameTypeToggle;




}
