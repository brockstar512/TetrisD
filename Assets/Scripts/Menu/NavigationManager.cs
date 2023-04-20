using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class NavigationManager : MonoBehaviour
{
    public static NavigationManager Instance { get; private set; }
    [SerializeField] Button Quickplay;
    [SerializeField] Button Settings;
    [SerializeField] Button HowToPlay;
    [SerializeField] Button Muliplayer;

    [SerializeField] CanvasGroup HowToPlayScreen;
    [SerializeField] CanvasGroup QuickPlayScreen;
    [SerializeField] CanvasGroup SettingScreen;
    [SerializeField] CanvasGroup LandingPage;
    [SerializeField] CanvasGroup MultiplayerPage;
    private Stack<CanvasGroup> stack;
    CanvasGroup currentPage;
    //where to we put difficulty
    [SerializeField] Button QuickplayBack;
    [SerializeField] Button OnlinePlayBack;
    [SerializeField] Button HowToPlayBack;
    [SerializeField] Button SettingsBack;
    [SerializeField] Button MuliplayerBack;



    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        InitializePage();
        currentPage = LandingPage;
        stack = new Stack<CanvasGroup>();
    }
    void InitializePage()
    {
        Quickplay.onClick.AddListener(delegate { OpenPage(QuickPlayScreen); });
        HowToPlay.onClick.AddListener(delegate { OpenPage(HowToPlayScreen); });
        Settings.onClick.AddListener(delegate { OpenPage(SettingScreen); });

        QuickplayBack.onClick.AddListener(Back);
        HowToPlayBack.onClick.AddListener(Back); ;
        SettingsBack.onClick.AddListener(Back); ;
    }

    void OpenPage(CanvasGroup screen)
    {
        screen.gameObject.SetActive(true);
        currentPage.DOFade(0, .1f).OnComplete(() =>
        {
            screen.DOFade(1, .2f).OnComplete(() =>
            {
                stack.Push(currentPage);
                currentPage.gameObject.SetActive(false);
                currentPage = screen;
            });
        });
    }

    public void CachePage(CanvasGroup previousScreen, CanvasGroup newScreen)
    {
        //stack.Push(previousScreen);
        newScreen.gameObject.SetActive(true);

        //do I need to change the alpha at all?
        previousScreen.DOFade(0, .1f).OnComplete(() =>
        {
            newScreen.DOFade(1, .2f).OnComplete(() =>
            {
                stack.Push(previousScreen);
                previousScreen.gameObject.SetActive(false);
                currentPage = newScreen;
            });
        });
    }


    public void Back()
    {
        CanvasGroup previousPage = this.stack.Pop();
        previousPage.gameObject.SetActive(true);
        currentPage.DOFade(0, .1f).OnComplete(() => {
            previousPage.DOFade(1, .2f).OnComplete(() =>
            {
                currentPage.gameObject.SetActive(false);
                currentPage = previousPage;
            });
        });
    }

}
