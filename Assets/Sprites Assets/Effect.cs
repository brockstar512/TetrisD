using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;



[CreateAssetMenu(fileName = "NewEffect", menuName = "ScriptableObjects/Effect", order = 1)]
public class Effect : ScriptableObject
{
    public DiceFXController.TileEffect effect;
    //public float frameCount;
    public float FPS;
    //public float framesPerSecond;
    public Tile[] tiles;




    public IEnumerator Animate(Vector3Int pos, Tilemap map)
    {
        //Tilemap map = map;
        for(int i = 0; i < tiles.Length; i++)
        {
            map.SetTile(pos, tiles[i]);
            yield return new WaitForSeconds(Mathf.Pow(FPS,-1));//how long we show this image
        }
        map.SetTile(pos, null);
        yield return null;

    }

}
