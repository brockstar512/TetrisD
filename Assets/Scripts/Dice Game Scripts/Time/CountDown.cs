using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CountDown : MonoBehaviour, IClock
{
    [SerializeField] AudioClip prepareSound;
    [SerializeField] AudioClip startSound;
    [SerializeField] MusicClient musicClient;
    [SerializeField] TextMeshProUGUI timerText;
    public TextMeshProUGUI countDownDisplay;
    private int countDownTime = 2;
    public bool isCounting = false;
    private float startTime;
    public int min;
    public int sec;
    public event StartGameDelegate startGameDelegate;

    string[] countDownText =
    {
        "START",
        "READY",
        ""
    };
    public void StartGame()
    {
        Debug.Log("START GAME ON COUNTDOWN");
        startGameDelegate += StartClock;

        StartCoroutine(GameCountDown());

        startTime = (min * 60) + sec;
        timerText.text = ClockCounter();

    }
    private void OnDisable()
    {
        startGameDelegate -= StartClock;
    }

    void Update()
    {
        if (isCounting)
            timerText.text = ClockCounter();
    }

    public IEnumerator GameCountDown()
    {
        Debug.Log("Game count down   1");

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
        Debug.Log("Game count down   2");

        startGameDelegate?.Invoke();
        musicClient.Play();
    }

    private void StartClock()
    {
        Debug.Log("Start game clock");
        //startTime = (min * 60) + sec;
        isCounting = true;

    }
    public string ClockCounter()
    {
        if (isCounting)
        {
            startTime -= Time.deltaTime;
        }
        float t = startTime;
        string minutes = ((int)t / 60).ToString();
        float sec = Mathf.Floor(t % 60);
        string seconds = sec < 10 ? "0" + sec.ToString("f0") : sec.ToString("f0");
        return minutes + ":" + seconds;
    }



}
