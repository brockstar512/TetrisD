using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileSwap : MonoBehaviour
{

    public TetrominoData sideLinePiece;
    public int indexOfSidePiece;
    public Vector3Int spawnPosition;
    public Vector2Int miniBoardSize= new Vector2Int(4,4);
    public RectInt Bounds {
        get
        {
            Vector2Int position = new Vector2Int(-this.miniBoardSize.x / 2, -this.miniBoardSize.y / 2);
            return new RectInt(position, this.miniBoardSize);
        }
    }
    public Tilemap swapMap;
    public Board board;
    public Vector3Int[] cells {get; private set;}

    [ContextMenu("Place Piece")]
    public void PlacePiece()
    {
        
        //Vector3Int position,TetrominoData data
        int random = Random.Range(0,6);
        TetrominoData data = board.tetrominoes[random];
        Debug.Log("SWAP: "+data.tetromino);
        //we dont really need a cells array... its more for manipulation on piece
        if(this.cells == null){
            Debug.Log("SWAP: cells is null");
            this.cells = new Vector3Int[data.cells.Length];
        }
        else{Clear();}
        Debug.Log("SWAP cells length: "+data.cells.Length);
        for(int i = 0; i< data.cells.Length;i++){
            this.cells[i] = (Vector3Int)data.cells[i];
            
            Vector3Int tilePosition = this.cells[i]+spawnPosition; 
            Debug.Log("SWAP position: "+tilePosition+"      i-> "+i);
            this.swapMap.SetTile(tilePosition,data.tile);
        }

    }


    public TetrominoData Swap(TetrominoData incomingPiece)
    {
        TetrominoData newPiece = sideLinePiece;
        this.sideLinePiece = incomingPiece;
        return newPiece;
    }

    public void Clear()
    {
        for(int i = 0;i< this.cells.Length;i++)
        {
            Vector3Int tilePosition = this.cells[i] + spawnPosition;
            this.swapMap.SetTile(tilePosition,null);
        }

    }


}
