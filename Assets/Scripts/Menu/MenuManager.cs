using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MenuManager : MonoBehaviour
{
    public RectTransform mainMenu, multiplayerMenu, quickPlayMenu, storyModeMenu;
    public Stack<RectTransform> memoryStack = new Stack<RectTransform>();

    // Start is called before the first frame update
    void Start()
    {
        mainMenu.DOAnchorPos(Vector2.zero, 0.25f);
        memoryStack.Push(mainMenu);
    }

    public void GoTo(RectTransform newPage)
    {

        SendLeft(memoryStack.Peek());
        memoryStack.Push(newPage);
        SendCenter(memoryStack.Peek());
    }
    public void Back()
    {

        SendRight(memoryStack.Peek());
        memoryStack.Pop();
        SendCenter(memoryStack.Peek());

    }

    void SendRight(RectTransform toTheRight)
    {
        toTheRight.DOAnchorPos(new Vector2(1080, 0), 0.25f);

    }
    void SendCenter(RectTransform GoTo)
    {
        GoTo.DOAnchorPos(Vector2.zero, 0.25f);
    }
    void SendLeft(RectTransform toTheLeft)
    {
        toTheLeft.DOAnchorPos(new Vector2(-1080, 0), 0.25f);

    }

}
