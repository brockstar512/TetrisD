using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DiceFXController : MonoBehaviour
{
    public GameObject popFXPrefab;
    public GameObject bubbleFXPrefab;
    public GameObject slamFXPrefab;
    public GameObject explosionFXPrefab;
    public GameObject explosionTwoFXPrefab;
    public GameObject touchFXPrefab;
    public GameObject disengageFXPrefab;

    public  Effect popFXP;

    //public AnimatedTile m_Bubble;
    //public AnimatedTile m_Slam;
    //public AnimatedTile m_Explosion;
    //public AnimatedTile m_ExplosionTwo;
    //public AnimatedTile m_Disengage;
    //public AnimatedTile m_Pop;
    //public AnimatedTile m_Touch;


    public enum TileEffect
    {
        bubble,
        slam,
        explosion,
        explosionTwo,
        disengage,
        pop,
        touch
    }

    public TileEffect effect;
    public Vector3Int location = new Vector3Int(0, 0, 0);


    //public Transform m_GridParent;
    //public GameObject m_TileMap_Prefab;
    //public AnimatedTile m_tilePrefabAnimated;
    //public Tile m_tilePrefabStatic;
    // while (tileSequence.m_AnimatedSprites[tileSequence.m_AnimatedSprites.Length-1] !=)

    public Tilemap map;


    GameObject ConfigureTile(TileEffect effect)
    {
        GameObject tileSequence;
        switch(effect)
        {
            case TileEffect.bubble:
                tileSequence = bubbleFXPrefab;
            break;
            case TileEffect.explosion:
                tileSequence = explosionFXPrefab;
                break;
            case TileEffect.explosionTwo:
                tileSequence = explosionTwoFXPrefab;
                break;
            case TileEffect.pop:
                tileSequence = popFXPrefab;
                break;
            case TileEffect.touch:
                tileSequence = touchFXPrefab;
                break;
            case TileEffect.slam:
                tileSequence = slamFXPrefab;
                break;
            case TileEffect.disengage:
                tileSequence = disengageFXPrefab;
                break;
            default:
                tileSequence = slamFXPrefab;
                break;
        }


        return tileSequence;
    }

    public void FX(TileEffect effect, Vector3Int location)
    {
        GameObject fx = ConfigureTile(effect);
        fx = Instantiate(fx, this.transform);
        fx.GetComponent<DiceFX>().map = this.map;
        fx.GetComponent<DiceFX>().location = location;
        fx.GetComponent<DiceFX>().FXStart();
    }

    public void FX(Vector3Int location)
    {
       popFXP.map = this.map;
       StartCoroutine(popFXP.Animate(location));
       
    }


}
