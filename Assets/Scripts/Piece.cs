using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//individual piece
public class Piece : MonoBehaviour
{
    public Board board {get;private set;}
    public TetrominoData data {get;private set;}
    public Vector3Int position {get; private set;}
    public Vector3Int[] cells {get; private set;}


    //spawn position, which piece is active
    public void Initialize(Board board, Vector3Int position,TetrominoData data)
    {
        this.board = board;
        this.position = position;
        this.data = data;

        if(this.cells == null){
            this.cells = new Vector3Int[data.cells.Length];//always 4 unless custom shape
        }

        for(int i = 0; i< data.cells.Length;i++){
            this.cells[i] = (Vector3Int)data.cells[i];
        }

    }

    //player input
    private void Update()
    {
        this.board.Clear(this);
        if(Input.GetKeyDown(KeyCode.LeftArrow))
        {
            //Debug.Log("Left Arrow pressed");
            Move(Vector2Int.left);
        }else if(Input.GetKeyDown(KeyCode.RightArrow)){
            //Debug.Log("Right Arrow pressed");

            Move(Vector2Int.right);
        }
        this.board.Set(this);

    }
    private bool Move(Vector2Int translation)
    {
        Vector3Int newPosition = this.position;
        newPosition.x += translation.x;
        newPosition.y += translation.y;

        bool valid = this.board.IsValidPosition(this, newPosition);

        // Only save the movement if the new position is valid
        if (valid)
        {
            this.position = newPosition;
        }

        return valid;
    }

}
