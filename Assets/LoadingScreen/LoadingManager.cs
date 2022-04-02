using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;



public class LoadingManager : MonoBehaviour
{
    public static LoadingManager Instance;

    [SerializeField] private GameObject _loaderCanvas;
    [SerializeField] private Image _progressBar;
    float _target;


    public Ease exitEase;
    public Ease enterEase;

    //loading screen enter animation and exit
    //enforce a one second delay
    //fade in  and fade out

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public async void LoadScene(string sceneName)
    {
        //start task (enter screen delay the rest of the function until the task is complete)

        _target = 0;
        _progressBar.fillAmount = 0;

        var scene = SceneManager.LoadSceneAsync(sceneName);
        scene.allowSceneActivation = false;

        _loaderCanvas.SetActive(true);

        do
        {
            await System.Threading.Tasks.Task.Delay(100);
            _target = scene.progress;
            

        } while (scene.progress < 0.9f);


        scene.allowSceneActivation = true;
        //make fill either a slider or completely fill the image
        //start task of exit.

        _loaderCanvas.SetActive(false);
    }
    private void Update()
    {
        _progressBar.fillAmount = Mathf.MoveTowards(_progressBar.fillAmount, _target, 3 * Time.deltaTime);
    }

    [ContextMenu("Enter")]
    public void Enter()
    {
        _loaderCanvas.transform.GetComponent<RectTransform>().DOAnchorPos(Vector2.zero, 1.5f).SetEase(enterEase);

    }

    [ContextMenu("Exit")]
    private void Exit()
    {
        _loaderCanvas.transform.GetComponent<RectTransform>().DOAnchorPos(new Vector2(0, 1920), 1f).SetEase(exitEase);

    }

}
