using UnityEngine;


public class DataStore : MonoBehaviour
{
    public static DataStore Instance { get; private set; }
    public PlayerData playerData;

    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
        DontDestroyOnLoad(this);
        Load();
    }


    private void Save()
    {

        PlayerPrefs.SetInt("marathonHighscore", playerData.marathonHighscore);
        PlayerPrefs.SetInt("timeAttackHighscore", playerData.timeAttackHighscore);
        PlayerPrefs.SetInt("lineBreakerHighscore", playerData.lineBreakerHighscore);
        PlayerPrefs.SetString("isFXMuted", playerData.isFXMuted.ToString());
        PlayerPrefs.SetString("isMusicMuted", playerData.isMusicMuted.ToString());

        PlayerPrefs.Save();

    }

    private void Load()
    {
        // Load

        int marathonHighscore = PlayerPrefs.HasKey("marathonHighscore") ? PlayerPrefs.GetInt("marathonHighscore") : 0;
        int timeAttackHighscore= PlayerPrefs.HasKey("timeAttackHighscore") ? PlayerPrefs.GetInt("timeAttackHighscore") : 0; 
        int lineBreakerHighscore = PlayerPrefs.HasKey("lineBreakerHighscore") ? PlayerPrefs.GetInt("lineBreakerHighscore") : 0;
        bool isFXMuted = PlayerPrefs.HasKey("isFXMuted") ? bool.Parse(PlayerPrefs.GetString("isFXMuted")) : false;
        bool isMusicMuted = PlayerPrefs.HasKey("isMusicMuted") ? bool.Parse(PlayerPrefs.GetString("isMusicMuted")) : false;

        playerData = new PlayerData(marathonHighscore, timeAttackHighscore, lineBreakerHighscore, isFXMuted, isMusicMuted);

        Debug.Log("Loading data " + playerData.isFXMuted + "  " + playerData.isMusicMuted);
    }

    private void OnApplicationQuit()
    {
        Debug.Log("Quitting Application");
        Save();
    }
}

public class PlayerData
{
    public int marathonHighscore;
    public int timeAttackHighscore;
    public int lineBreakerHighscore;
    public bool isFXMuted;
    public bool isMusicMuted;

    public PlayerData()
    {
        marathonHighscore = 0;
        timeAttackHighscore = 0;
        lineBreakerHighscore = 0;
        isFXMuted = false;
        isMusicMuted = false;
    }

    public PlayerData(int marathonHighscore, int timeAttackHighscore, int lineBreakerHighscore, bool isFXMuted,bool isMusicMuted)
    {
        this.marathonHighscore = marathonHighscore;
        this.timeAttackHighscore = timeAttackHighscore;
        this.lineBreakerHighscore = lineBreakerHighscore;
        this.isFXMuted = isFXMuted;
        this.isMusicMuted = isMusicMuted;
    }

    public void UpdateScore(GameSetUp.GameType gameType, int newScore)
    {
        switch (gameType)
        {
            case GameSetUp.GameType.Marathon:
                this.marathonHighscore = newScore > marathonHighscore ? newScore : marathonHighscore;
                break;
            case GameSetUp.GameType.TimeAttack:
                this.timeAttackHighscore = newScore > timeAttackHighscore ? newScore : timeAttackHighscore;
                break;
            case GameSetUp.GameType.LineBreaker:
                this.lineBreakerHighscore = newScore > lineBreakerHighscore ? newScore : lineBreakerHighscore;
                break;
        }
    }

    public void UpdateFX(bool isFXMuted)
    {
        this.isFXMuted = isFXMuted;
    }

    public void UpdateMusic(bool isMusicMuted)
    {
        this.isMusicMuted = isMusicMuted;
        Debug.Log("Update music:" + isMusicMuted);

    }
}
