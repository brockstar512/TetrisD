using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEvents;

public class ChangeSceneButton : MonoBehaviour
{
    public Scenes targetScene;


    public void ChangeScene()
    {
        LoadingManager.Instance.LoadScene(targetScene.ToString());
    }
}



public enum Scenes
{
    MainMenu,
    Dice,
    Game
}
