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

    enum CountDownText
    {
        Start,
        Set,
        Ready,
    }
    private void Awake()
    {
        //StartCoroutine(GameCountDown());
    }

    void Update()
    {
        Debug.Log(isCounting);
        if (isCounting)
            timerText.text = Clock();
    }

    public IEnumerator GameCountDown()
    {
        CountDownText text;
        startTime = (min * 60) + sec;

        countDownDisplay.text = "";
        countDownDisplay.gameObject.SetActive(true);
        while (countDownTime > 0)
        {
            text = (CountDownText)countDownTime;
            countDownDisplay.text = text.ToString();
            yield return new WaitForSeconds(1f);
            countDownTime--;
        }
        isCounting = true;
        text = (CountDownText)countDownTime;
        countDownDisplay.text = text.ToString();
        yield return new WaitForSeconds(1f);
        //Clock();
        
        //start the game
        countDownDisplay.gameObject.SetActive(false);
    }

    public string Clock()
    {
        //if (currentTime <= 0)
        //    return;
        //startTime -= Time.deltaTime;
        Debug.Log(startTime);
        float t = startTime - Time.time;
        Debug.Log(t);
        string minutes = ((int)t / 60).ToString();
        float sec = Mathf.Floor(t % 60);
        string seconds = sec < 10 ? "0" + sec.ToString("f0") : sec.ToString("f0");
        return minutes + ":" + seconds;
    }

    public IEnumerator Timer()
    {
        yield return new WaitForSeconds(1f);

    }
}
