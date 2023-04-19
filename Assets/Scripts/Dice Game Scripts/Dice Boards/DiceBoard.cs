using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DiceBoard : MonoBehaviour
{
    public DiceMatch diceMatch;
    public DiceFXController diceFXController;
    private CountDown currentClock;
    public DifficultyManager difficultyManager;


    public DiceData[] DiceOptions;
    public Vector3Int spawnPosition;
    public Tilemap tilemap { get; private set; }
    public DiceGroup activeGroup { get; private set; }
    //for is valid
    public Vector2Int boardSize = new Vector2Int(6, 10);//cells on the board
    public RectInt Bounds
    {
        get
        {
            //Bounds: x-3 y:-5
            //width 6 height 10

            Vector2Int position = new Vector2Int(-this.boardSize.x / 2, -this.boardSize.y / 2);
            return new RectInt(position, this.boardSize);
        }
    }

    public CurrentState currentState = CurrentState.Beginning;
    public enum CurrentState
    {
        Beginning,
        Playing,
        Disengaging,
        Scoring,
        GameOver,
    }
    

    // Start is called before the first frame update
    public void Init(IClock currentClock)
    {
        #region Bound Testing
        //i wonder if it wont go all the way because you can divide 9 by 2 that easily when the bounds is divided by 2
        //Debug.Log($"BOUNDS x : {Bounds.xMin} and {Bounds.xMax}");//BOUNDS x : -3 and 3
        //Debug.Log($"BOUNDS y : {Bounds.yMin} and {Bounds.yMax}");//BOUNDS y : -5 and 5
        //Debug.Log($"BOUNDS with and height : {Bounds.width} and {Bounds.height}");//BOUNDS with and height : 6 and 10
        //Debug.Log($"BOUNDS size :{Bounds.size}");//BOUNDS size :(6, 10)
        //Debug.Log($"board size :{this.boardSize}");//board size :(6, 10)
        //Debug.Log($"-this.boardSize.y / 2 :{-this.boardSize.y / 2}");//-this.boardSize.y / 2 :-5
        //Debug.Log($"this.boardSize.y :{this.boardSize.y}");//this.boardSize.y :10
        #endregion


        this.tilemap = GetComponentInChildren<Tilemap>();
        this.activeGroup = GetComponentInChildren<DiceGroup>();

        //initialize all the possible options with their cells array
        for (int i = 0; i < this.DiceOptions.Length; i++)
        {
            this.DiceOptions[i].Initialize();
        }

        currentClock.startGameDelegate += SpawnGroup;

    }



    public void SpawnGroup()
    {
        DiceData newGroup = difficultyManager.DiceFactory();
        DiceData newGroup2 = difficultyManager.DiceFactory();

        //make sure they are diffrent
        if (newGroup.number >= DiceNumber.Seven && newGroup2.number >= DiceNumber.Seven)
        {
            Debug.Log("Both Dice are bombs");
            while (newGroup2.number == DiceNumber.Seven)
            {
                newGroup2 = difficultyManager.DiceFactory();
            }

        }
        Debug.Log("Spawning Group");
        this.activeGroup.isPlaying = true;
        this.activeGroup.isScoring = false;
        
        //int random = Random.Range(0, this.DiceOptions.Length);
        //DiceData newGroup = this.DiceOptions[random];
        //Debug.Log($"This is the new group {newGroup.color}");
        //int random2 = Random.Range(0, this.DiceOptions.Length);
        //DiceData newGroup2 = this.DiceOptions[random2];
        //Debug.Log($"This is new group number two {newGroup2.color}");


        Debug.Log($"<color=purple>{newGroup.number} and {newGroup2.number}</color>");
        this.activeGroup.Initialize(this, spawnPosition, newGroup, newGroup2);
        SetOnBoard(this.activeGroup);//pass the dice group collection to be placed on the board

        if (activeGroup.isHardDropping)
        {
            StopCoroutine(activeGroup.DelayHardDrop());
            StartCoroutine(activeGroup.DelayHardDrop());
        }

    }

    //this should be in a state manager method
    public void ClearGroupFromBoard()
    {
        //this function removes the group from the board while its scoreing
        //Debug.Log($"<color=red> clear group from baord</color>");
        this.activeGroup.isScoring = true;
        DiceData newGroup = null;
        DiceData newGroup2 = null;
        this.activeGroup.Initialize(this, spawnPosition, newGroup, newGroup2);
    }


    /// <summary>
    /// We are checking if there is still a dice attacked to the dice group. If there is we take that piece and set it at its position in the dice group (diceGroup.cells[1]) and diceGroup.cells[0]
    /// and add it to the position of where the dice group is on the board diceGroup.position.
    /// </summary>
    /// <param name="diceGroup"></param>
    public void SetOnBoard(DiceGroup diceGroup)
    {
       
        //Debug.LogWarning("SET ON BOARD");
        if (diceGroup.dynamicData != null)
        {
            this.tilemap.SetTile(diceGroup.cells[1] + diceGroup.position, diceGroup.dynamicData.tile);
        }
        else { //Debug.Log("dynamic dice is null");
             }
        if (diceGroup.data != null)
        {
            this.tilemap.SetTile(diceGroup.cells[0] + diceGroup.position, diceGroup.data.tile);
        }
        else
        {
            //Debug.Log("data dice is null");
        }
    }

    /// <summary>
    /// based of a position passed in, the tile will be cleared there.
    /// </summary>
    /// <param name="position"></param>
    public void Clear(Vector3Int position)
    {
        //Debug.Log("Clear group " + position);

        this.tilemap.SetTile(position, null);

        //this.tilemap.HasTile(tilePosition)
        //Debug.Log("Clear group      " + this.tilemap.HasTile(position));

    }

    public void Clear(DiceGroup group)
    {
        //Debug.Log("Clear group");
        for (int i = 0; i < group.cells.Length; i++)
        {
            Vector3Int tilePosition = group.cells[i] + group.position;
            this.tilemap.SetTile(tilePosition, null);


     

        }

    }

    public bool IsValidPosition(DiceGroup group, Vector3Int position)
    {
        //Debug.Log($"Is valid?"); 
        RectInt bounds = this.Bounds;
        //Debug.Log("Here are the bounds " + bounds);
        // The position is only valid if every cell is valid
        for (int i = 0; i < group.cells.Length; i++)
        {
            Vector3Int tilePosition = group.cells[i] + position;
            //Debug.Log("Here is the tile position "+ (Vector2Int)tilePosition);
            // An out of bounds tile is invalid
            if (!bounds.Contains((Vector2Int)tilePosition))
            {
                //Debug.LogError("bounds does not contain that position");
                return false;
            }

            // A tile already occupies the position, thus invalid
            if (this.tilemap.HasTile(tilePosition))
            {
                //Debug.LogError("tile map has that tile");
                return false;
            }
        }

        return true;

    }

    public bool IsValidPositionSingleDice(Vector3Int position)
    {
        //get the bounds
        RectInt bounds = this.Bounds;

        //what position are we checking?
        Vector3Int tilePosition = position;

        //is that position in the bounds?
        if (!bounds.Contains((Vector2Int)tilePosition))
        {
            return false;
        }
        //Debug.Log($"Does this tile map have a tile in the position you are reaching for? {this.tilemap.HasTile(tilePosition)}");
        //does that position alreadt have a tile 
        if (this.tilemap.HasTile(tilePosition))
        {
            return false;
        }

        //you are good to go to the next position
        return true;

    }
    public bool CanOnePieceContinue(Vector3Int diceOnePosOnBoard, Vector3Int diceTwoPosOnBoard, out int? whichDice)
    {
        bool canDiceOne = IsValidPositionSingleDice(diceOnePosOnBoard);
        bool canDiceTwo = IsValidPositionSingleDice(diceTwoPosOnBoard);

        bool canOnePieceContinue = (canDiceOne && !canDiceTwo) || (!canDiceOne && canDiceTwo) ? true : false;
        //check if they are ontop of eachother
        canOnePieceContinue = (diceOnePosOnBoard.y - 1 == diceTwoPosOnBoard.y) || (diceOnePosOnBoard.y == diceTwoPosOnBoard.y - 1) ? false : canOnePieceContinue;

        if (canOnePieceContinue)
            whichDice = canDiceOne == true ? 0 : 1;
        else
            whichDice = null;

        return canOnePieceContinue;
    }

    //or make a place single position on the board with different arguments

    public void SetSingleDiceOnBoard(Vector3Int position, Tile tile)
    {
        this.tilemap.SetTile(position, tile);
    }

    public void SetSingleDiceOnBoard(DiceGroup diceGroup,int diceToSet)
    {
        Tile tileToSet = diceToSet == 0 ? diceGroup.data.tile : diceGroup.dynamicData.tile;
        Debug.Log($"we are going to set {diceToSet} on the board at  {diceGroup.cells[diceToSet] + diceGroup.position} with tile that is {tileToSet}");

        this.tilemap.SetTile(diceGroup.cells[diceToSet] + diceGroup.position, tileToSet);

    }


    
}
