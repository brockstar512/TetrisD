using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CountUp : MonoBehaviour, IClock
{
    [SerializeField] AudioClip prepareSound;
    [SerializeField] AudioClip startSound;
    [SerializeField] MusicClient musicClient;
    [SerializeField] TextMeshProUGUI timerText;
    public TextMeshProUGUI countDownDisplay;
    private int countDownTime = 2;
    public bool isCounting;
    private float startTime;
    string[] countDownText =
    {
        "START",
        "READY",
        ""
    };
    public event StartGameDelegate startGameDelegate;



    public void StartGame()
    {
        StartCoroutine(GameCountDown());
        startGameDelegate += StartClock;
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
    private void StartClock()
    {
        isCounting = true;
        startTime = Time.time;
        countDownDisplay.text = countDownText[countDownTime];
    }

    public string Clock()
    {
        float t = Time.time - startTime;
        string minutes = ((int)t / 60).ToString();
        float sec = Mathf.Floor(t % 60);
        string seconds = sec < 10 ? "0" + sec.ToString("f0") : sec.ToString("f0");
        return minutes + ":" + seconds;
    }


}
