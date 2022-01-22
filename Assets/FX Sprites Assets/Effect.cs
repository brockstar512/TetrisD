using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Threading.Tasks;




[CreateAssetMenu(fileName = "NewEffect", menuName = "ScriptableObjects/Effect", order = 1)]
public class Effect : ScriptableObject
{
    public DiceFXController.TileEffect effect;
    //public float frameCount;
    public float FPS;
    //public float framesPerSecond;
    public Tile[] tiles;
    public bool isDoneAnimating = false;




    public IEnumerator Animate(Vector3Int pos, Tilemap map)
    {

        isDoneAnimating = false;
        //Tilemap map = map;
        for (int i = 0; i < tiles.Length; i++)
        {
            map.SetTile(pos, tiles[i]);
            yield return new WaitForSeconds(Mathf.Pow(FPS,-1));//how long we show this image
        }
        map.SetTile(pos, null);
        isDoneAnimating = true;
        yield return null;
    }

    public async Task AnimateTask(Vector3Int pos, Tilemap map)
    {
        Debug.Log("Animating Task");

        for (int i = 0; i < tiles.Length; i++)
        {
            map.SetTile(pos, tiles[i]);
            await Task.Delay((int)(Mathf.Pow(FPS, -1) * 1000));//how long we show this image
        }
        map.SetTile(pos, null);

        //await Task.Yield();

    }

}
