using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;


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
  
        stack.Push(currentPage);
        currentPage.gameObject.SetActive(false);
        currentPage.gameObject.transform.localScale = new Vector3(0, 0, 0);
        currentPage.alpha = 0;
        screen.gameObject.transform.localScale = new Vector3(1, 1, 1);
        screen.alpha = 1;
        screen.gameObject.SetActive(true);
        currentPage = screen;

    }




    public void Back()
    {
        currentPage.gameObject.transform.localScale = new Vector3(0, 0, 0);
        currentPage.gameObject.SetActive(false);
        currentPage.alpha = 0;
        CanvasGroup previousPage = this.stack.Pop();
        previousPage.gameObject.SetActive(true);
        previousPage.gameObject.transform.localScale = new Vector3(1, 1, 1);
        previousPage.alpha = 1;
        currentPage = previousPage;

    }

}
