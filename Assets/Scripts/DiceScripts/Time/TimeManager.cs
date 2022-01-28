using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class TimeManager : MonoBehaviour
{
    //turn this into an interface... startgun which calls counter coroutine

    [SerializeField]Sprite[] numbers;
    [SerializeField] TextMeshProUGUI timerText;
    private float startTime;
    public int min;
    public int sec;
    public bool countUp;
    public int countDownTime = 3;
    public TextMeshProUGUI countDownDisplay;
    //public Image countDownDisplay;



    // Start is called before the first frame update
    void Awake()
    {
        switch (countUp)
        {
            case true:
                startTime = Time.time;
                break;
            case false:
                startTime = (min * 60) + sec;
                break;
        }

        //StartCoroutine(CountDown());
    }

    // Update is called once per frame
    void Update()
    {
        //timerText.text = CountUp();
        timerText.text = CountDown();
    }
    string CountUp()
    {
        float t = Time.time - startTime;
        string minutes = ((int)t / 60).ToString();
        float sec = Mathf.Floor(t % 60);
        string seconds = sec < 10 ? "0" + sec.ToString("f0") : sec.ToString("f0");
        return minutes + ":" + seconds;
    }

    string CountDown()
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
        //return  ":";
    }

    //IEnumerator CountDown()
    //{
    //    countDownDisplay.text = "";
    //    countDownDisplay.gameObject.SetActive(true);
    //    while (countDownTime > 0)
    //    {
    //        //countDownDisplay.sprite = numbers[countDownTime];
    //        countDownDisplay.text = countDownTime.ToString();
    //        yield return new WaitForSeconds(1f);
    //        countDownTime--;
    //    }
    //    //countDownDisplay.sprite= numbers[0];
    //    countDownDisplay.text = "GO!";
    //    yield return new WaitForSeconds(1f);
    //    countDownDisplay.gameObject.SetActive(false);
    //}
}
