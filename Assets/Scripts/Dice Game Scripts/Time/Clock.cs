using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Clock : MonoBehaviour, IClock
{
    public event StartGameDelegate startGameDelegate;
    [SerializeField] AudioClip prepareSound;
    [SerializeField] AudioClip startSound;
    [SerializeField] MusicClient musicClient;
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] GameRules gameRules;
    public TextMeshProUGUI countDownDisplay;
    private int countDownTime = 2;
    public bool isCounting;
    private float startTime;
    const int _min = 1;
    const int _sec = 0;
    string[] countDownText =
    {
        "START",
        "READY",
        ""
    };

    public void StartGame()
    {
        StartCoroutine(GameCountDown());
        startGameDelegate += StartClock;

        if (GameSetUp.gameType == GameSetUp.GameType.TimeAttack)
        {
            startTime = (_min * 60) + _sec;
            timerText.text = ClockCounter();

        }
    }
    void Update()
    {

        if (isCounting)
        {
            string val = ClockCounter();
            string valueText = val == "0:0-1" ? "0:00" : val;
            timerText.text = valueText;
        }
    }

    private void OnDisable()
    {
        startGameDelegate -= StartClock;

    }

    private void StartClock()
    {
        isCounting = true;
        if (GameSetUp.gameType != GameSetUp.GameType.TimeAttack)
        {
            startTime = Time.time;
            countDownDisplay.text = countDownText[countDownTime];
        }

    }

    public IEnumerator GameCountDown()
    {
        countDownDisplay.text = "";
        countDownDisplay.gameObject.SetActive(true);
        while (countDownTime > 0)
        {
            countDownDisplay.text = countDownText[countDownTime];
            yield return new WaitForSeconds(1f);
            SoundManager.Instance.PlaySound(prepareSound);
            countDownTime--;
        }
        //give the last sign a second
        countDownDisplay.text = countDownText[countDownTime];
        yield return new WaitForSeconds(1f);
        SoundManager.Instance.PlaySound(startSound);


        countDownDisplay.gameObject.SetActive(false);
        startGameDelegate?.Invoke();
        musicClient.Play();
    }

    public string ClockCounter()
    {
        if (GameSetUp.gameType != GameSetUp.GameType.TimeAttack)
        {
            float t = Time.time - startTime;
            string minutes = ((int)t / 60).ToString();
            float sec = Mathf.Floor(t % 60);
            string seconds = sec < 10 ? "0" + sec.ToString("f0") : sec.ToString("f0");
            return minutes + ":" + seconds;
        }
        else
        {
            Debug.Log((Mathf.Floor(startTime % 60) > 0));
            Debug.Log((((int)startTime / 60) > 0));
            //Debug.Log($"One value {(int)startTime / 60} and {((int)startTime / 60)}");
            //Debug.Log($"Checking  {isCounting} and {(Mathf.Floor(startTime % 60) > 0)} and {(((int)startTime / 60) > 0)}");
            //bool isZero = ((Mathf.Floor(startTime % 60) > 0) && (((int)startTime / 60) > 0));
            if (isCounting)
            {
                startTime -= Time.deltaTime;
            }

            float t = startTime;
            string minutes = ((int)t / 60).ToString();
            float sec = Mathf.Floor(t % 60);
            string seconds = sec < 10 ? "0" + sec.ToString("f0") : sec.ToString("f0");

            if(startTime <= 0)
            {
                gameRules.TimeAttack();

            }
            return minutes + ":" + seconds;
        }

    }

    public void ResetClock()
    {
        timerText.text = "0:00";
    }
}

