using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class QuitButton : MonoBehaviour
{
    [SerializeField] Button _quit;
    [SerializeField] Button _yes;
    [SerializeField] Button _no;
    [SerializeField] CanvasGroup cg;
    [SerializeField] GameObject canvas;

    private void Start()
    {
        _quit.onClick.AddListener(Open);
        _yes.onClick.AddListener(ExitToMenu);
        _no.onClick.AddListener(Close);
        Close();
    }

    void Open()
    {

        canvas.gameObject.SetActive(true);
        cg.DOFade(1, .25f).SetEase(Ease.OutSine);
        
    }
    void Close()
    {
        Debug.Log("Close");
        canvas.gameObject.SetActive(false);
        cg.DOFade(0, .25f).SetEase(Ease.InSine).OnComplete(() => { canvas.gameObject.SetActive(false); }); 
        
    }

    void ExitToMenu()
    {
        Debug.Log("Menu");

    }

    private void OnDestroy()
    {
        _yes.onClick.RemoveAllListeners();
        _no.onClick.RemoveAllListeners();
        _quit.onClick.RemoveAllListeners();
    }

}
