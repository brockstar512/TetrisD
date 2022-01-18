using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DiceFX : MonoBehaviour
{

    public AnimatedTile _effect;
    public Vector3Int location = new Vector3Int(0, 0, 0);
    public Tilemap map;
    public float maxSpeed;
    public float minSpeed;




    public void FXStart()
    {
        AnimatedTile tileSequence = _effect;
        tileSequence.m_AnimationStartFrame = 0;
        tileSequence.m_MaxSpeed = maxSpeed;
        tileSequence.m_MinSpeed = minSpeed;
        map.SetTile(location, tileSequence);
        StartCoroutine(OperationStopWatch(location, tileSequence));
    }


    public IEnumerator OperationStopWatch(Vector3Int Position, AnimatedTile tile)
    {
        //Debug.Log("time:::" + tile.m_AnimatedSprites.Length / tile.m_MaxSpeed);
        yield return new WaitForSeconds((tile.m_AnimatedSprites.Length / tile.m_MaxSpeed));
        map.SetTile(Position, null);
        Destroy(this.gameObject);
    }
}
