using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DiceFXController : MonoBehaviour
{

    public Effect popFX;
    public Effect slamFX;
    public Effect disengageLeftFX;
    public Effect disengageRightFX;



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
        pop,
        touch
    }

    private TileEffect effect = TileEffect.none;
    private Vector3Int location = new Vector3Int(0, 0, 0);


    //public Transform m_GridParent;
    //public GameObject m_TileMap_Prefab;
    //public AnimatedTile m_tilePrefabAnimated;
    //public Tile m_tilePrefabStatic;
    // while (tileSequence.m_AnimatedSprites[tileSequence.m_AnimatedSprites.Length-1] !=)

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

            default:
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


}
