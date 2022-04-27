using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class SwipeEffect : MonoBehaviour,IDragHandler,IBeginDragHandler,IEndDragHandler
{

    private Vector3 _initialPosition;
    private float _distanceMoved;
    private bool _swipeLeft;

    public event Action cardMoved;
    
    public void OnDrag(PointerEventData eventData)
    {
        //drags card
        transform.localPosition = new Vector2(transform.localPosition.x+eventData.delta.x,transform.localPosition.y);
    
        //depending which side you swipe towards give it an angle
            //-move right of the center
        if(transform.localPosition.x - _initialPosition.x > 0)
        {
            //roatate -30 degrees 
            transform.localEulerAngles = new Vector3(0,0,Mathf.LerpAngle(0,-30,
            (_initialPosition.x + transform.localPosition.x)/(Screen.width/2)));
        }
        else //-move left of the center
        {
            //roatate +30 degrees 
            transform.localEulerAngles = new Vector3(0,0,Mathf.LerpAngle(0,30,
            (_initialPosition.x - transform.localPosition.x)/(Screen.width/2)));
            //lerp Angles also only take 0 - 1 values so we have to make it positive

        }
    }
    //gets initial position
        public void OnBeginDrag(PointerEventData eventData)
    {
        _initialPosition = transform.localPosition;
    }
    //snaps card back
    public void OnEndDrag(PointerEventData eventData)
    {
        _distanceMoved = Mathf.Abs(transform.localPosition.x - _initialPosition.x);
        //if its moved more than 40% it stays there... less than it will snap back

        //you did not swipe all the way
        if(_distanceMoved<0.4*Screen.width)
        {
            //reset the travel distance to the initial position if swipe not complete
            transform.localPosition = _initialPosition;
            //reset the rotation to 0 if the swipe is not complete
            transform.localEulerAngles = Vector3.zero;

        }
        //you swiped
        else
        {
            //swiped right
            if (transform.localPosition.x > _initialPosition.x)
            {
                _swipeLeft = false;
                
            }
            else
            {
                //swipe left
                _swipeLeft = true;
            }
            //invoke the action subscribed to this event
            cardMoved?.Invoke();

            //move the card all the way
            StartCoroutine(MovedCard());
        }
    }
    //take the car all the way to the side and destory it
    private IEnumerator MovedCard()
    {
        float time = 0;
        //as your'e moving it make the card fade
        while (GetComponent<Image>().color != new Color(1, 1, 1, 0))
        {
            time += Time.deltaTime;
            if (_swipeLeft)
            {
                transform.localPosition = new Vector3(Mathf.SmoothStep(transform.localPosition.x,
                    transform.localPosition.x-Screen.width,time),transform.localPosition.y,0);
            }
            else
            {
                transform.localPosition = new Vector3(Mathf.SmoothStep(transform.localPosition.x,
                    transform.localPosition.x+Screen.width,time),transform.localPosition.y,0);
            }
            GetComponent<Image>().color = new Color(1,1,1,Mathf.SmoothStep(1,0,4*time));
            yield return null;
        }
        Destroy(gameObject);
    }
}
