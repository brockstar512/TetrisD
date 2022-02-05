using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CountDown : MonoBehaviour, IClock
{
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
        StartCoroutine(GameCountDown());
        startGameDelegate += StartClock;


        startTime = (min * 60) + sec;
        timerText.text = Clock();

    }
    private void OnDisable()
    {
        startGameDelegate -= StartClock;
    }

    void Update()
    {
        if (isCounting)
            timerText.text = Clock();
    }

    public IEnumerator GameCountDown()
    {
        countDownDisplay.text = "";
        countDownDisplay.gameObject.SetActive(true);
        while (countDownTime > 0)
        {
            countDownDisplay.text = countDownText[countDownTime];
            yield return new WaitForSeconds(1f);
            countDownTime--;
        }
        //give the last sign a second
        countDownDisplay.text = countDownText[countDownTime];
        yield return new WaitForSeconds(1f);
        
        countDownDisplay.gameObject.SetActive(false);
        startGameDelegate?.Invoke();

    }

    private void StartClock()
    {
        //startTime = (min * 60) + sec;
        isCounting = true;

    }
    public string Clock()
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
