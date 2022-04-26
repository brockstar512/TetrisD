using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondCard : MonoBehaviour
{

    private SwipeEffect _swipeEffect;
    public GameObject _frontCard;
    // Start is called before the first frame update
    void Start()
    {
        
        _swipeEffect = FindObjectOfType<SwipeEffect>();
        _frontCard = _swipeEffect.gameObject;//modify this
        _swipeEffect.cardMoved+=CardMovedFront;

        //set the size of the second card to .8 the size so it grows into the scene
        transform.localScale = new Vector3(0.8f,0.8f,0.8f);
    }

    // Update is called once per frame
    void Update()
    {
        //where the first gameObject is
        float distanceMoved = _frontCard.transform.localPosition.x;
        //we are working with the absolute value because we are agnostic to the direction, only if it is past the swipe threshold
        if(Mathf.Abs(distanceMoved) > 0)
        {
            float step = Mathf.SmoothStep(0.8f, 1,Mathf.Abs(distanceMoved)/ (Screen.width/2));
            transform.localScale =new Vector3(step,step,step);
        }
    }

    //delete this script and give the game object the swipe effect script as it is the new center card
    void CardMovedFront()
    {
        gameObject.AddComponent<SwipeEffect>();
        Destroy(this);
    }
}
