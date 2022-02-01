using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Threading.Tasks;


public class DiceGroup : MonoBehaviour
{
    public DiceMatch diceMatch;
    public DiceFXController diceFXController;


    public DiceDisengage diceDisengage { get; private set; }
    public DiceGhost diceGhost;


    public DiceBoard diceBoard {get;private set;}//anytime the piece moves we need to pass that info to redraw that game piece
    public DiceData data {get;private set;}//the dice that stays still 
    public DiceData dynamicData { get; private set; }//the dice that roates around the other

    /// <summary>
    /// This is tehe position the group is on the board as it falls down the board.
    /// </summary>
    public Vector3Int position {get; private set;}
    /// <summary>
    /// We have an array of cells to help manipulate the shape when it moves/rotatets. This also keeps track of the local position of the dice.
    /// one dice will always be 0,0 within this group but the dynamic dice is moving around and this helps keep track of it.
    /// (cell[0]) = data (cell[1]) = dynamicData
    /// </summary>
    public Vector3Int[] cells {get; private set;}


    //dropping
    public float stepDelay = 1f;
    public float lockDelay = 0.5f;

    private float stepTime;
    private float lockTime;

    public bool isDisengaging = false;//is playing or playerHasControll
    public bool isScoring = false;
    public bool isPlaying = false;
    public bool isHorizontal{ get{return this.cells[0].y == this.cells[1].y; } }
    //{get;private set;}


    //TODO
    //-clean check for match
    //-clean state manager
    //-clean public fields
    //-chaining
    //-animation
    //-visual queues
    public enum GameState
    {
        None,
        Disengaging,
        Scoring,
        Playing,

    }
    public GameState gameState = GameState.None;

 


    #region testing regions

    DynamicDiceState dynamicDiceState = DynamicDiceState.Right;

    Dictionary<DynamicDiceState, Vector3Int> RotationDict =
    new Dictionary<DynamicDiceState, Vector3Int>()
    {
	    { DynamicDiceState.Right,new Vector3Int(1, 0, 0)},
        { DynamicDiceState.Down,new Vector3Int(0,  -1, 0)},
        { DynamicDiceState.Left,new Vector3Int(- 1, 0, 0)},
        { DynamicDiceState.Up,new Vector3Int(0, 1, 0)}
    };
    #endregion

    //const float DisengageDropSpeed = 0.05f;



    //TODO clean the initialize function up and see if i can do a deep dive into explainning whats going on 
    ///game board           spawn location      dice data currently active
    public void Initialize(DiceBoard diceBoard, Vector3Int position, DiceData data, DiceData dynamicData)
    {
        diceDisengage = this.GetComponent<DiceDisengage>();
        diceDisengage.Initialize(diceBoard);

        //Debug.Log("Spawn location:  "+ position);
        this.diceBoard = diceBoard;
        this.position = position;
        
        this.data = data;
        this.dynamicData = dynamicData;

        if (isScoring)
            return;

        this.stepTime = Time.time+ stepDelay;
        this.lockTime = 0f;


        //if we have not initialized this array do it now
        if (this.cells == null)
        {
            //this.cells = new Vector3Int[data.cellLocation.Length];//single die
            this.cells = new Vector3Int[2];//changed it to have a cell for the second die
            //this.cells = new Vector3Int[1];//single die

        }
        /*
        //fill in the array with the current tile info in play
        for(int i = 0; i< data.cellLocation.Length;i++){
            this.cells[i] = (Vector3Int)data.cellLocation[i];
        }*/
        this.cells[0] = (Vector3Int)data.cellLocation[0];//(Vector3Int)
        //To the right -> 
        this.cells[1] = new Vector3Int( data.cellLocation[0].x + 1, data.cellLocation[0].y , 0); //this is dynamic dice position//TODO This is very imortant



    }

    #region Update
    void Update()
    {
        if (isScoring)
            return;

        if (isDisengaging)
            return;

        if (!isPlaying)
            return;
        //MoveController();
        this.diceBoard.Clear(this);

        this.lockTime += Time.deltaTime;
        
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Move(Vector2Int.left);
        }
        if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            Move(Vector2Int.right);
        }
        if(Input.GetKeyDown(KeyCode.DownArrow)){
            Move(Vector2Int.down);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            HardDrop();

        }


        if (Input.GetKeyDown(KeyCode.RightShift))
        {
            Rotate(DynamicDiceState.Right);
        }
        else if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            Rotate(DynamicDiceState.Left);
            
        }

        if(Time.time >= this.stepTime)
        {
            Step();
        }

        //the disengage check might get switched after this update function runs but before it ends, so the check in the beginning is not good enough.
        //we also need to check here because this will revert the dice back into the board.
        if (!isDisengaging && !isScoring)
        {
            this.diceBoard.SetOnBoard(this);
        }
    }

    #endregion

    #region Rotation Methods

    /// <summary>
    /// Rotate takes in either a right or left parameter. If it's right, it will check what state the dynamic die
    /// currently is in and will determine what the position is to the right or left based off of that.
    /// example ...if we pass in right and our current dice is south of our static die then is will determine that left is the targeted tile.
    /// Then we check if the direction is valid. If it is we apply the rotation. if not we swap tiles and reverse the direction.
    /// </summary>
    /// <param name="intendedDirection"></param>
    void Rotate(DynamicDiceState intendedDirection)
    {
        switch (intendedDirection)
        {
            case DynamicDiceState.Right:
                switch (dynamicDiceState)
            {       
                case DynamicDiceState.Right:
                    intendedDirection = DynamicDiceState.Down;
                    break;
                case DynamicDiceState.Left:
                    intendedDirection = DynamicDiceState.Up;
                    break;
                case DynamicDiceState.Down:
                    intendedDirection = DynamicDiceState.Left;
                    break;
                case DynamicDiceState.Up:
                    intendedDirection = DynamicDiceState.Right;
                break;
            }
                break;
            case DynamicDiceState.Left:
                switch (dynamicDiceState)
                {
                    case DynamicDiceState.Right:
                        intendedDirection = DynamicDiceState.Up;
                        break;
                    case DynamicDiceState.Left:
                        intendedDirection = DynamicDiceState.Down;
                        break;
                    case DynamicDiceState.Down:
                        intendedDirection = DynamicDiceState.Right;
                        break;
                    case DynamicDiceState.Up:
                        intendedDirection = DynamicDiceState.Left;
                        break;
                }
                break;
        }
        
        if (this.diceBoard.IsValidPosition(this, this.position + RotationDict[intendedDirection]))
            ApplyRotation(intendedDirection);
        else
            ReverseRotationAndSwap(intendedDirection);
    }

    /// <summary>
    /// Apply rotation just applies the rotation from a dictionary based off of the argument passed in and updates the dynamic dice position
    /// </summary>
    /// <param name="rotation"></param>
    void ApplyRotation(DynamicDiceState rotation)
    {
        Vector3Int newPosition;
        switch(rotation)
        {
            case DynamicDiceState.Right:
                dynamicDiceState = DynamicDiceState.Right;
                newPosition = RotationDict[DynamicDiceState.Right];
                break;
            case DynamicDiceState.Up:
                dynamicDiceState = DynamicDiceState.Up;
                newPosition = RotationDict[DynamicDiceState.Up];
                break;
            case DynamicDiceState.Left:
                dynamicDiceState = DynamicDiceState.Left;
                newPosition = RotationDict[DynamicDiceState.Left];
                break;
            case DynamicDiceState.Down:
                dynamicDiceState = DynamicDiceState.Down;
                newPosition = RotationDict[DynamicDiceState.Down];
                break;
            default:
                newPosition = new Vector3Int(0, 0, 0);
                    break;

        }
        this.cells[1] = new Vector3Int(newPosition.x, newPosition.y, 0);
        this.lockTime = 0f;
    }

    /// <summary>
    /// This function reverses the direction you intented to go and swaps the tile information. This is called when the tile you are trying to rotate to
    /// is out of bounds
    /// </summary>
    /// <param name="rotation"></param>
    void ReverseRotationAndSwap(DynamicDiceState rotation)
    {
        switch(rotation)
        {
            case DynamicDiceState.Up:
                ApplyRotation(DynamicDiceState.Down);
                break;
            case DynamicDiceState.Down:
                ApplyRotation(DynamicDiceState.Up);
                break;
            case DynamicDiceState.Right:
                ApplyRotation(DynamicDiceState.Left);
                break;
            case DynamicDiceState.Left:
                ApplyRotation(DynamicDiceState.Right);
                break;

        }
        SwapTilesInGroup();
    }

    #endregion

    #region Movement Methods
    /// <summary>
    /// Moves tile after checking is valid position which check the bounds of the board, then if that tile has a tile on it
    /// if the tile does not and it is in the bounds then you can move there.... reverts the locktime to 0
    /// </summary>
    /// <param name="translation"></param>
    /// <returns>if the move is valid</returns>
    private bool Move(Vector2Int translation)
    {
        Vector3Int newPosition = this.position;
        newPosition.x += translation.x;
        newPosition.y += translation.y;

        bool valid = this.diceBoard.IsValidPosition(this, newPosition);
        
        // Only save the movement if the new position is valid
        if (valid)
        {
            this.position = newPosition;
            this.lockTime = 0f;
        }

        return valid;
    }

    
    /// <summary>
    /// immedietly moves the tile down until move does not return it being valid then it locks the piece into place. Diengages the single dice that can move if it can.
    /// </summary>
    void HardDrop()
    {
        while (Move(Vector2Int.down))
        {
            continue;
        }
        int? continuingDice;
        if (this.diceBoard.CanOnePieceContinue(new Vector3Int(cells[0].x, cells[0].y - 1, cells[0].z) + this.position, new Vector3Int(cells[1].x, cells[1].y - 1, cells[1].z) + this.position, out continuingDice))
        {
            //Debug.LogError($"DISENGAGE dice {continuingDice}");
            HandleDisengagement(continuingDice);
            return;
        }
        //Debug.Log("FX");
        //diceFXController.FX(DiceFXController.TileEffect.bigSlamLeft, this.cells[0] + this.position);
        //diceFXController.FX(DiceFXController.TileEffect.bigSlamRight, this.cells[1] + this.position);
        //Lock();
        HardDropFX(this.cells[0] + this.position, this.cells[1] + this.position);

    }

    async void HardDropFX(Vector3Int locationLeft, Vector3Int locationRight)
    {
        List<Task> tasks = new List<Task>();
        //this decides which hard drop fx to do
        switch(isHorizontal)
        {
            case true:
            //Debug.Log($"is it horizontal {isHorizontal}");
            tasks.Add(diceFXController.FXTask(DiceFXController.TileEffect.bigSlamLeft, locationLeft));
            tasks.Add(diceFXController.FXTask(DiceFXController.TileEffect.bigSlamRight, locationRight));
                break;
            case false:
                switch(this.cells[0].y < this.cells[1].y)
                {
                    case true:
                        tasks.Add(diceFXController.FXTask(DiceFXController.TileEffect.slam, locationLeft));
                        break;
                    case false:
                        tasks.Add(diceFXController.FXTask(DiceFXController.TileEffect.slam, locationRight));
                        break;
                }
                break;
        }
        await Task.WhenAll(tasks);
        Lock();

    }


    /// <summary>
    /// resets the step time and forces the tile group to move down
    /// </summary>
    void Step()
    {
        this.stepTime = Time.time + this.stepDelay;
        Move(Vector2Int.down);

        //this will only run when we get to the bottom because everytome move passes it resets locktime
        if(lockTime >= this.lockDelay)
        {
            //just the cells position by themselves is there local position, so we have to add the dice groups position to get where they are on the tile map
            int? continuingDice;
            if (this.diceBoard.CanOnePieceContinue(new Vector3Int(cells[0].x, cells[0].y - 1, cells[0].z) + this.position, new Vector3Int(cells[1].x, cells[1].y - 1, cells[1].z) + this.position, out continuingDice))
            {
                //Debug.LogError($"DISENGAGE dice {continuingDice}");
                HandleDisengagement(continuingDice);
                return;

            }
                Lock();
        }
    }

    /// <summary>
    /// sets the tile group on the board. takes away our reference by/while spawining in a new dice group
    /// </summary>
    void Lock()
    {
        //if(false)
        {
            
            DiceImprint staticDice = new DiceImprint(this.data, this.cells[0] + this.position);
            DiceImprint dynamicDice = new DiceImprint(this.dynamicData, this.cells[1] + this.position);

            
            diceMatch.SetTileDict(this.cells[0] + this.position, staticDice);
            diceMatch.SetTileDict(this.cells[1] + this.position, dynamicDice);
        }

        this.diceBoard.SetOnBoard(this);
        //this.diceBoard.SpawnGroupq();
        diceMatch.Score();
    }
    #endregion

    #region Disengagement Methods
    void HandleDisengagement(int? continuingDice)
    {
        isDisengaging = true;


        int leaveDiceBehind = 1 - (int)continuingDice;//if the coninuing dice is 1 this will be 0. if the coninuing dice is 0 this will be 1
        DiceData stillData = leaveDiceBehind == 0 ? this.data : this.dynamicData;
        DiceData travelingDice = leaveDiceBehind == 1 ? this.data : this.dynamicData;
        Vector3Int holdPos = new Vector3Int((cells[leaveDiceBehind].x + this.position.x), (cells[leaveDiceBehind].y + this.position.y), 0);
        Vector3Int startPos = new Vector3Int((cells[(int)continuingDice].x + this.position.x), (cells[(int)continuingDice].y + this.position.y), 0);

        Vector3Int finishPos = diceGhost.GET_LOWEST_Y_COORD;

        this.diceBoard.Clear(startPos);
        this.diceBoard.Clear(holdPos);


        diceDisengage.Disengage(stillData, travelingDice, holdPos, startPos, finishPos);
        //this seems a little redundant with the positions being passed into both but i suppose the dice hold the position has well as the dict
        DiceImprint still = new DiceImprint(stillData, holdPos);
        DiceImprint falling = new DiceImprint(travelingDice, finishPos);
        diceMatch.SetTileDict(holdPos, still);
        diceMatch.SetTileDict(finishPos, falling);
    }

    /// <summary>
    /// Once the disengagment logic is done, it will call this function that will reset the boolean and spawn the next dice group.
    /// </summary>
    public void HandlePostDisengagement()
    {
        isDisengaging = false;
        diceMatch.Score();
    }
    #endregion


    /// <summary>
    /// Swaps the dice data information in the tiles
    /// </summary>
    void SwapTilesInGroup()
    {

        //swap number as well
        DiceData temp = this.dynamicData;
        this.dynamicData = this.data;
        this.data = temp;
    }


    public void ClearGroupFromBoard()
    {
        isScoring = true;
        DiceData newGroup = null;
        DiceData newGroup2 = null;
        this.Initialize(diceBoard, diceBoard.spawnPosition, newGroup, newGroup2);

    }
    //out 
    public void StateManager(ref GameState from, GameState to)
    {
        switch (to)
        {
            case GameState.Disengaging:
                from = to;
                break;
            case GameState.Playing:
                from = to;
                //this.diceBoard.SpawnGroup();
                break;
            case GameState.Scoring:
                from = to;
                //check for score
                break;
            default:
                break;
        }
    }

}

enum DynamicDiceState
{
    Left = -1,
    Up,
    Right,
    Down
}
//if roation is valid rotate it// otherwise send the inverse and swap
//up -> data.cellLocation[0].y +1
//right ->data.cellLocation[0].x + 1
//down -> data.cellLocation[0].y -1
//left -> data.cellLocation[0].x - 1
