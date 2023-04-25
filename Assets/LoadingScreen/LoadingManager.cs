using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class LoadingManager : MonoBehaviour
{
    public static LoadingManager Instance;

    [SerializeField] private GameObject _loaderCanvas;
    [SerializeField] private Image _progressBar;
    [SerializeField] private RectTransform _targetRect;
    float _target;
    Scenes currentScene;
    public Ease exitEase;
    public Ease enterEase;
    public bool showBar;

    public event Action OnNewScene;
    private float height;

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

        Debug.Log(_targetRect.rect.height);
        height = this.GetComponent<RectTransform>().rect.height;
        _loaderCanvas.transform.GetComponent<RectTransform>().DOAnchorPos(new Vector2(0, height), 0);
    }

    public async void LoadScene(Scenes sceneName)
    {
        if (!showBar)
            _progressBar.gameObject.SetActive(false);
        await Enter();

        _target = 0;
        _progressBar.fillAmount = 0;

        var scene = SceneManager.LoadSceneAsync(sceneName.ToString());
        scene.allowSceneActivation = false;

        _loaderCanvas.SetActive(true);
        currentScene = sceneName;
        SoundManager.Instance.StopMusic();

        do
        {
            await System.Threading.Tasks.Task.Delay(100);
            _target = scene.progress;
            

        } while (scene.progress < 0.9f);


        scene.allowSceneActivation = true;
        //make fill either a slider or completely fill the image
        //start task of exit.
        await Exit();
        //_loaderCanvas.SetActive(false);
    }
    private void Update()
    {
        if (!showBar)
            return;
        _progressBar.fillAmount = Mathf.MoveTowards(_progressBar.fillAmount, _target, 3 * Time.deltaTime);
    }

    [ContextMenu("Enter")]
    async Task Enter()
    {
        _loaderCanvas.transform.GetComponent<RectTransform>().DOAnchorPos(Vector2.zero, 1.5f).SetEase(enterEase);
        await Task.Delay(1500);

    }

    [ContextMenu("Exit")]
    async Task Exit()
    {
        _loaderCanvas.transform.GetComponent<RectTransform>().DOAnchorPos(new Vector2(0, height), 1f).SetEase(exitEase);
        await Task.Delay(1000);
        OnNewScene?.Invoke();

    }

}
