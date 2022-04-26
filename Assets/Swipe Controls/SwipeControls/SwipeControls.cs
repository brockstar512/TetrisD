using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SwipeControls : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerDownHandler, IPointerUpHandler
{


    public enum DraggedDirection
    {
        Up,
        Down,
        Right,
        Left
    }
    public enum RotationDirection
    {
        Left,
        Right
    }

    //[Header("Dimension")]
    float mag;
    float width;
    float height;


    //[Header("Dimension")]
    public float tapTimeLimit = 1f;
    public int swipeLimit = 50;
    float tapTime = 0f;
    bool isDrag;
    private Vector2 originPoint;
    RotationDirection tapSide;


    private void Awake()
    {
        //width = (float)Screen.width / 2.0f;
        //height = (float)Screen.height / 2.0f;
        //Debug.Log($"width {width}");
        //Debug.Log($"height {height}");
        RectTransform rectTransform = GetComponent<RectTransform>(); ;
        Debug.Log($"rect {rectTransform.rect.width}");
        width = rectTransform.rect.width;
        height = rectTransform.rect.height;




    }


    public void OnDrag(PointerEventData eventData)
    {
        //Debug.Log($"event data {eventData.position} and origin point {originPoint}");
        mag = (eventData.position - originPoint).magnitude;
        if (mag > swipeLimit)
        {
            //we're going to move it one unit now
            Debug.Log($"1 unit");

            isDrag = true;

            //get the point of where we were vs where we are so we can have the direction.
            Vector3 dragVectorDirection = (eventData.position - originPoint).normalized;
            //Debug.Log($"Origin point {originPoint} and direction {dragVectorDirection} ");

            //this gets from the current drag vector that counts as a move so we know what direction to move
            DraggedDirection dir = GetDragDirection(dragVectorDirection);
            Debug.Log(dir);

            MoveDispatch(dir);
            //reset the origin point from where we counted a movement
            originPoint = eventData.position;

        }

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //Debug.Log("OnEndDrag");
        //Vector3 dragVectorDirection = (eventData.position - eventData.pressPosition).normalized;
        //GetDragDirection(dragVectorDirection);

    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originPoint = eventData.pressPosition;
        //Debug.Log("OnBeginDrag");
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        tapTime = Time.time;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (isDrag)
        {
            isDrag = false;
            return;
        }
        
        if (Time.time - tapTime < tapTimeLimit)
        {
            Debug.Log($"Here is the event {eventData.position.x} and here is the half waypoint  {(width / 2)} and here is the width {width}");


            tapSide = eventData.position.x < (width / 2) ? RotationDirection.Left : RotationDirection.Right;
            MoveDispatch(tapSide);
        }


    }

    private DraggedDirection GetDragDirection(Vector3 dragVector)
    {
        float positiveX = Mathf.Abs(dragVector.x);
        float positiveY = Mathf.Abs(dragVector.y);
        DraggedDirection draggedDir;
        if (positiveX > positiveY)
        {
            draggedDir = (dragVector.x > 0) ? DraggedDirection.Right : DraggedDirection.Left;
        }
        else
        {
            draggedDir = (dragVector.y > 0) ? DraggedDirection.Up : DraggedDirection.Down;
        }
        //Debug.Log(draggedDir);
        return draggedDir;
    }


    public void MoveDispatch(DraggedDirection move)
    {
        Debug.Log($"MOVE {move}");
    }
    public void MoveDispatch(RotationDirection rotation)
    {
        Debug.Log($"ROTATE {rotation}");

    }
}
