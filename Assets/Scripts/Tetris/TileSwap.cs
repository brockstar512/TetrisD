using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileSwap : MonoBehaviour
{

    private TetrominoData sideLinePiece;
    //dont need
    public int indexOfSidePiece;
    public Vector3Int spawnPosition;
    //dont need
    public Vector2Int miniBoardSize= new Vector2Int(4,4);
    //dont need
    public RectInt Bounds {
        get
        {
            Vector2Int position = new Vector2Int(-this.miniBoardSize.x / 2, -this.miniBoardSize.y / 2);
            return new RectInt(position, this.miniBoardSize);
        }
    }
    public Tilemap swapMap;//the tile map that will have filled tiles
    public Board board;
    public Vector3Int[] cells {get; private set;}//the tiles of swap map that will fill

    
    public TetrominoData Swap(TetrominoData incoming)
    {
        TetrominoData toReturn = sideLinePiece;
        sideLinePiece = incoming;

        if(this.cells == null)
        {
            //if swap is empty return a random piece
            int random = Random.Range(0,7);
            toReturn = board.tetrominoes[random];
            //fill in the tiles of that tile map with these cells
            this.cells = new Vector3Int[incoming.cells.Length];
        }
        else{
            //clear what was there before
            Clear();
            }
            //whatever is the new sidline piece fill in thos tiles
        for(int i = 0; i< sideLinePiece.cells.Length;i++){
            this.cells[i] = (Vector3Int)sideLinePiece.cells[i];
            //fill it in at the designiated spawn position  
            Vector3Int tilePosition = this.cells[i]+spawnPosition; 
            this.swapMap.SetTile(tilePosition,sideLinePiece.tile);
        }
        //return the piece that was either there before or it is reandom
        return toReturn;
    }


    //clear tilemap of what was previosuly drawn
    private void Clear()
    {
        for(int i = 0;i< this.cells.Length;i++)
        {
            Vector3Int tilePosition = this.cells[i] + spawnPosition;
            this.swapMap.SetTile(tilePosition,null);
        }
    }


}
