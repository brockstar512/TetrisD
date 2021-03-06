using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Threading.Tasks;

public class DiceFXController : MonoBehaviour
{
    public DiceBoard diceBoard;

    public Effect popFX;
    public Effect slamFX;
    public Effect disengageLeftFX;
    public Effect disengageRightFX;
    public Effect bigSlamRightFX;
    public Effect bigSlamLeftFX;
    public Effect explosionFX;




    //public AnimatedTile m_Bubble;
    //public AnimatedTile m_Slam;
    //public AnimatedTile m_Explosion;
    //public AnimatedTile m_ExplosionTwo;
    //public AnimatedTile m_Disengage;
    //public AnimatedTile m_Pop;
    //public AnimatedTile m_Touch;


    public enum TileEffect
    {
        none,
        bubble,
        slam,
        explosion,
        explosionTwo,
        disengageLeft,
        disengageRight,
        bigSlamRight,
        bigSlamLeft,
        pop,
        touch
    }

    private TileEffect effect = TileEffect.none;
    private Vector3Int location = new Vector3Int(0, 0, 0);



    public Tilemap map;


    Effect ConfigureTile(TileEffect effect)
    {
        Effect tileSequence;
        switch(effect)
        {
            case TileEffect.pop:
                tileSequence = popFX;
            break;
            case TileEffect.slam:
                tileSequence = slamFX;
                break;
            case TileEffect.disengageLeft:
                tileSequence = disengageLeftFX;
                break;
            case TileEffect.disengageRight:
                tileSequence = disengageRightFX;
                break;
            case TileEffect.bigSlamLeft:
                tileSequence = bigSlamLeftFX;
                break;
            case TileEffect.bigSlamRight:
                tileSequence = bigSlamRightFX;
                break;
            case TileEffect.explosion:
                tileSequence = explosionFX;
                break;
            default:
                Debug.LogError("FX Not Found");
                tileSequence = popFX;
                break;
        }
        return tileSequence;
    }

    public void FX(TileEffect effect, Vector3Int location)
    {
        Effect effectConfig = ConfigureTile(effect);
       StartCoroutine(effectConfig.Animate(location,map));
       
    }

    //public async Task FXTask(TileEffect effect, Vector3Int location)
    //{
    //    Effect effectConfig = ConfigureTile(effect);
    //    StartCoroutine(effectConfig.Animate(location, map));
    //    //effectConfig.isDoneAnimating

    //    //    Debug.Log($"Clearing:::{location}");
    //    //    diceBoard.Clear(location);
    //    //    Effect effectConfig = ConfigureTile(effect);
    //    //    await effectConfig.AnimateTask(location, map);

    //}
    public async Task FXTask(TileEffect effect, Vector3Int location)
    {
        Effect effectConfig = ConfigureTile(effect);
        await effectConfig.AnimateTask(location, map);
    }



}
