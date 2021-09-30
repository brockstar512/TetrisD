using UnityEngine.Tilemaps;
using UnityEngine;

public enum Tetromino
{
    I,
    O,
    T,
    J,
    L,
    S,
    Z
}

[System.Serializable]
public struct TetrominoData
{
    public Tetromino tetromino;
    public Tile tile;//which color tile we are using
    public Vector2Int[] cells{get;private set;}//what shape the tetromino should be

    public void Initialize()
    {
        this.cells = Data.Cells[this.tetromino];
    }

}