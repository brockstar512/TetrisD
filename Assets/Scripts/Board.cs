using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

//control the overall state
public class Board : MonoBehaviour
{
    public TetrominoData[] tetrominoes;//possible terominoes option
    //a way to kep variable public without exposing it in the editor
    public Tilemap tilemap{get; private set;}
    public Piece activePiece {get; private set;}
    public Vector3Int spawnPosition;
    public Vector2Int boardSize= new Vector2Int(10,20);
    public RectInt Bounds {
        get
        {
            Vector2Int position = new Vector2Int(-this.boardSize.x / 2, -this.boardSize.y / 2);
            return new RectInt(position, this.boardSize);
        }
    }
    


    private void Awake(){
        this.tilemap = GetComponentInChildren<Tilemap>();
        this.activePiece = GetComponentInChildren<Piece>();

        for(int i = 0;i<this.tetrominoes.Length;i++){
        this.tetrominoes[i].Initialize();
        }
    }

    private void Start()
    {
        SpawnPiece();
    }

    private void SpawnPiece()
    {
        int random = Random.Range(0,this.tetrominoes.Length);
        TetrominoData data = this.tetrominoes[random];

        //passing our piece the game board, spawn position, and random data
        this.activePiece.Initialize(this, spawnPosition, data);

        //now we set piece on gamebooard
        Set(this.activePiece);
    }

    public void Set(Piece piece)
    {
        for(int i = 0;i< piece.cells.Length;i++)
        {
            Vector3Int tilePosition = piece.cells[i] + piece.position;
            this.tilemap.SetTile(tilePosition,piece.data.tile);
        }

    }
        public void Clear(Piece piece)
    {
        for(int i = 0;i< piece.cells.Length;i++)
        {
            Vector3Int tilePosition = piece.cells[i] + piece.position;
            this.tilemap.SetTile(tilePosition,null);
        }

    }

        public bool IsValidPosition(Piece piece, Vector3Int position)
    {
        RectInt bounds = this.Bounds;
        Debug.Log("Here are the bounds "+bounds);
        // The position is only valid if every cell is valid
        for (int i = 0; i < piece.cells.Length; i++)
        {
            Vector3Int tilePosition = piece.cells[i] + position;
            Debug.Log("Here is the tile position "+ (Vector2Int)tilePosition);
            // An out of bounds tile is invalid
            if (!bounds.Contains((Vector2Int)tilePosition)) {
                Debug.LogError("bounds does not contain that position");
                return false;
            }

            // A tile already occupies the position, thus invalid
            if (this.tilemap.HasTile(tilePosition)) {
                Debug.LogError("tile map has that tile");
                return false;
            }
        }

        return true;
    }
}
