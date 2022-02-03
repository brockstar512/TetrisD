using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Threading.Tasks;
using System.Linq;

public class DiceMatch : MonoBehaviour
{
    public event Action<int, bool, int> scoreEvent;
    public event Action<int> bombEvent;



    public DiceFXController diceFXController;
    public DiceBoard diceBoard;
    [SerializeField] Tilemap mainMap;    
    [SerializeField] Grid grid;
    List<bool> TaskList = new List<bool>();

    #region Grid Data
    private enum YGridCell
    {
        One_Top =3,
        Two = 2,
        Three = 1,
        Four =0,
        Five = -1,
        Six = -2,
        Seven = -3,
        Eight = -4,
        Nine_Bottom = -5,
    }
    private enum XGridCell
    {
        One_Left = -3,
        Two = -2,
        Three = -1,
        Four = 0,
        Five = 1,
        Six_Right = 2,
    }
    bool taskIsRunning = false;
    Dictionary<Vector3Int, DiceImprint> TilePos;
    #endregion 

    const float GRAVITY = 0.045f;//0.045f;
    private int Chain;

    private void Awake()
    {
        Initialize();
        //TaskList.Add(true);
    }

    void Update()
    {


        if (Input.GetMouseButtonDown(0))
        {
            //MouseTileReader();
        }

    }

    void Initialize()
    {
        TilePos = new Dictionary<Vector3Int, DiceImprint>();

        for (int y = (int)YGridCell.Nine_Bottom; y <= (int)YGridCell.One_Top; y++)
        {
            for (int x = (int)XGridCell.One_Left; x <= (int)XGridCell.Six_Right; x++)
            {
                Vector3Int tile = new Vector3Int(x,y,0);
                DiceImprint emptyTile = new DiceImprint(tile);
                TilePos.Add(tile,emptyTile);
                //Debug.Log($"KEYS:{tile}");//KEYS:(2, 3, 0)
            }
        }

    }

    /// <summary>
    /// replaces the dice value in the tile pos dictionary
    /// </summary>
    /// <param name="Pos"></param>
    /// <param name="die"></param>
    public void SetTileDict(Vector3Int Pos, DiceImprint die)
    {
        TilePos[Pos] = die;
    }

    /// <summary>
    /// deletes the tiles that have a match
    /// </summary>
    /// <param name="Pos"></param>
    public void RemoveTileDict(Vector3Int Pos)
    {
        TilePos[Pos] = new DiceImprint(Pos);
    }

    public void Score()
    {
        Debug.Log($"Scoring");
        diceBoard.ClearGroupFromBoard();
        Chain = 0;
        CheckForMatches();

    }

    /// <summary>
    /// this is the Main function of this script. It checks every tile and sees if there is a match. From there it handles the logic of which functions to call next
    /// </summary>
    void CheckForMatches()
    {
        bool hasMatch = false;
        //-3 -2 -1 0 1 2

        //3  (...)
        //2  (...)
        //1  (...)
        //0  (...)
        //-1 (...)
        //-2 (4th)
        //-3 (3rd)
        //-4 (2nd)
        //-5 (1st) (then this column)

        Dictionary<Vector3Int, int> MatchedDice = new Dictionary<Vector3Int, int>();


        for (int y = (int)YGridCell.Nine_Bottom; y <= (int)YGridCell.One_Top; y++)
        {

            for (int x = (int)XGridCell.One_Left; x<= (int)XGridCell.Six_Right; x++)
            {
                //what position are we checking
                Vector3Int position = new Vector3Int(x, y, 0);
                //does it have a tile?
                bool hasTile = mainMap.HasTile(position);
                //if (hasTile)
                //{
                //    Debug.Log($"This position of {x},{y} it has a tile and it is a {TilePos[position].number} ");
                //}

                //if there isnt a tile go to the next one
                if (!hasTile)
                    continue;

                //Do they have a pair in bounds
                bool withinHorizontalBounds = (position.x + (int)TilePos[position].number) <= 2 ? true : false;
                bool withinVerticalBounds = (position.y + (int)TilePos[position].number) <= 3 ? true : false;

                //get the number of the current position
                DiceNumber number = TilePos[position].number;

                //if the dice is zero go to the next tile
                if (number == DiceNumber.Zero)
                    continue;

                ////if its a bomb
                //if (number == DiceNumber.Seven)
                //ExplodeFX



                Debug.Log($"Bug check 1 {withinHorizontalBounds} for dice {TilePos[position].number} its checking position starting at {position} to {(position.x + (int)TilePos[position].number)} away on the x axis");


                //get a list of the dice inbetween in case they are the same color
                List<Vector3Int> BetweenDice = new List<Vector3Int>();
                //Debug.Log($"withing horizontal bounds? {withinHorizontalBounds}");
                //Debug.Log($"withing vertical bounds? {withinVerticalBounds}");


                Debug.Log($"Bug check abov if");
                //if it is within our bounds
                if (withinHorizontalBounds)
                {
                    Debug.Log($"Bug check below if");

                    //get the dice x number away
                    Vector3Int horizontalPosCheck = new Vector3Int(position.x + (int)number, position.y, 0);

                    //if the numbers are the same?
                    bool hasHorizontalMatch = TilePos[position].number == TilePos[horizontalPosCheck].number;

                    //if the colors are the same?
                    bool hasHorizontalColorMatch = TilePos[position].color == TilePos[horizontalPosCheck].color;

                        Debug.Log($"Bug check 2 {withinHorizontalBounds} for dice {TilePos[position].number} is there a match: {hasHorizontalMatch} between {TilePos[position].number} and { TilePos[horizontalPosCheck].number}");

                     //if there is a horizontal match and they have a dice in between them...
                    if (hasHorizontalMatch && IsConnected(position, horizontalPosCheck, withinHorizontalBounds, BetweenDice))
                    {
                        scoreEvent?.Invoke((int)TilePos[position].number, hasHorizontalColorMatch, Chain);

                        //figure out if we need to loop through all this again because the board has changed
                        hasMatch = true;
                        //add the start position
                        BetweenDice.Add(position);
                        //add the matching dice
                        BetweenDice.Add(horizontalPosCheck);
                            //this log will tell you what the position and number of the matching dice is
                            Debug.Log($"<color=green>Match</color> pos {position} comparing with other position {horizontalPosCheck} the two numbers are {TilePos[position].number} and {TilePos[horizontalPosCheck].number}");

                            //add the inbetween tiles and give them a value that corresponds with how often they occur
                        foreach (Vector3Int item in BetweenDice)
                        {
                            MatchedDice[item] = MatchedDice.ContainsKey(item) ? MatchedDice[item] + 1 : 1;
                        }
                    }
                }
                //we do the same with the vertial bounds as we did with the horizontal.
                if (withinVerticalBounds) {
                    Vector3Int verticalPosCheck = new Vector3Int(position.x, position.y + (int)number, 0);

                    bool hasVerticalMatch = TilePos[position].number == TilePos[verticalPosCheck].number;
                    bool hashasVerticalColorMatch = TilePos[position].color == TilePos[verticalPosCheck].color;

                    if (hasVerticalMatch && IsConnected(position, verticalPosCheck, withinVerticalBounds, BetweenDice))
                    {
                        scoreEvent?.Invoke((int)TilePos[position].number, hashasVerticalColorMatch, Chain);

                        //figure out if we need to loop through all this again because the board has changed
                        hasMatch = true;
                        BetweenDice.Add(position);
                        BetweenDice.Add(verticalPosCheck);
                        //Debug.Log($"<color=green>Match</color> pos {position} comparing with other position {verticalPosCheck} the two numbers are {TilePos[position].number} and {TilePos[verticalPosCheck].number}");
                        foreach (Vector3Int item in BetweenDice)
                        {
                            MatchedDice[item] = MatchedDice.ContainsKey(item) ? MatchedDice[item] + 1 : 1;                            
                        }
                    }
                }

            }

        }
        #region Helper Log
        {
            /* this will tell you the tile that will be removed the level of animation and the tile number.
            foreach(KeyValuePair<Vector3Int, int>  pair in MatchedDice)
            {
                                                               //location to delete    //level of animation                 //tile number
                Debug.Log($"<color=red>Matches In Dictionary </color>: {pair.Key} -> {pair.Value} and here is the number {(int)TilePos[pair.Key].number}");
            }
            */
        }
        #endregion
        if (hasMatch)
        {
            //pass in the matched dice dictionary if there is a match otherwise we should continue playing
            RemoveTiles(MatchedDice);
            //keep track of how many times we've ran this for waterfall bonus
            Chain++;
            return;
        }
        //continue playing.
        diceBoard.SpawnGroup();
    }

    /// <summary>
    /// We pass in the dictionary of all the dice that need to be removed. the key is the location that needs to be removed and the value is
    /// how many chains that tile was involved with. At the endit applies gravity to the free floating tiles.
    /// </summary>
    /// <param name="MatchedDice"></param>
    async void RemoveTiles(Dictionary<Vector3Int, int> MatchedDice)
    {
        List<Task> tasks = new List<Task>();

        //Debug.LogError("removing tiles");
        foreach (KeyValuePair<Vector3Int, int> pair in MatchedDice) {
            //-the key is the tile position to remove
            //-the value is how many chains is that tile included in

            //delete this position from the board
            diceBoard.Clear(pair.Key);

            //reset position to 0
            TilePos[pair.Key] = new DiceImprint(pair.Key);

            tasks.Add(diceFXController.FXTask(DiceFXController.TileEffect.pop, pair.Key));

        }
        //when all animations are finished
        await Task.WhenAll(tasks);
        //apply the gravity
        ApplyGravity();
    }

    /// <summary>
    /// This sunction check to see if there is a tile in every cell between the tile you are at and that tiles number away. if it is the same color it will add it to the
    /// list of dice to remove. if it is not the same color it will just return that it is completely connected. We pass in the current position. The position that tiles number distance away.
    /// If that checked position is in bounds. The list that will gather which dice should also be removed if any.
    /// </summary>
    /// <param name="position"></param>
    /// <param name="checkingPosition"></param>
    /// <param name="inBounds"></param>
    /// <param name="listOfDiceToRemove"></param>
    /// <returns></returns>
    bool IsConnected(in Vector3Int position, Vector3Int checkingPosition,in bool inBounds, List<Vector3Int>listOfDiceToRemove)
    {
        //Debug.Log($"We are checking {position} againt position {checkingPosition} and is it in bounds? {inBounds}");
        //if what we're checking is out of bounds just ditch it.
        listOfDiceToRemove.Clear();
        if (!inBounds)
            return false;


        //get a reference if they are the same color
        bool sameColor = TilePos[position].color == TilePos[checkingPosition].color;

        //figure out if we are going in the x or y
        Vector3Int directionalCheck = position.x == checkingPosition.x ? new Vector3Int(0,-1,0)  : new Vector3Int(-1, 0, 0);

        //make sure we're not checking the final position
        checkingPosition += directionalCheck;

        //while start position does not equal checking position
        while (position != checkingPosition)
        {
            //if what you're checking is not connected ditch it and break
            if (!mainMap.HasTile(checkingPosition))
            {
                listOfDiceToRemove.Clear();
                return false;
            }

            //if the bookended dice are the same color gather the whole list
            if (sameColor)
                listOfDiceToRemove.Add(checkingPosition);

            //move to the next position to check
            checkingPosition += directionalCheck;
        }

        //this check is connected
        return true;
    }

    /// <summary>
    /// This function just looks at all the empty spaces and pulls the tiles that are above them down so there are no more remaining gaps.
    /// </summary>
    private async void ApplyGravity()
    {
        List<Task> tasks = new List<Task>();

        //start from the bottom
        for (int x =(int)XGridCell.One_Left; x<= (int)XGridCell.Six_Right; x++)
        {
            int MovingDiceInRow = -1;

            for (int y = (int)YGridCell.Nine_Bottom; y <= (int)YGridCell.One_Top; y++)
            {
                Vector3Int position = new Vector3Int(x, y, 0);
 
                //we can skip the tiles that are empty
                if (!mainMap.HasTile(position))//(y doesnt have a tile)
                    continue;

                Vector3Int startPos = position;
                Vector3Int finishPos= new Vector3Int();

                //we found a tile.. if there is another tile under it or we are at the bottom don't run
                //we are falling until we have a tile or we are at the bottom
                //falling is the position of the current dice


                DiceImprint currentDice = new DiceImprint(TilePos[position], position);

                for (int falling = position.y;(mainMap.HasTile(new Vector3Int(x,falling-1,0)) || (falling >= (int)YGridCell.Nine_Bottom)); falling--)
                {
                    Vector3Int newPosition = new Vector3Int(x, falling, 0);
                    finishPos = newPosition;
                }

                //get final position
                MovingDiceInRow++;
                finishPos = new Vector3Int(finishPos.x, finishPos.y + MovingDiceInRow,0);

                if (mainMap.HasTile(finishPos))
                    continue;


                //move the dice down until it can't anymore
                tasks.Add(TravelingDice(currentDice, startPos, finishPos));

            }

        }

        //when all the dice are moved down
        await Task.WhenAll(tasks);
        //check for the matches again
        CheckForMatches();
    }


    public async Task TravelingDice(DiceImprint die, Vector3Int start, Vector3Int finish)
    {

        TilePos[start] = new DiceImprint(start);

        Vector3Int current = start;

        while (current != finish)
        {
            this.diceBoard.Clear(current);
            current = new Vector3Int(current.x, current.y + -1, 0);
            this.diceBoard.SetSingleDiceOnBoard(current, die.tile);
            await Task.Delay(((int)(GRAVITY *1000)));
        }
        this.diceBoard.SetSingleDiceOnBoard(finish, die.tile);

        TilePos[finish] = new DiceImprint(die, finish);
    }

    [ContextMenu("TestExplode")]
    void TestExplode()
    {
        Debug.Log("Testing explose");
        Vector3Int first = new Vector3Int(3,3,0);
        Vector3Int second = new Vector3Int(-3,-3,0);
        Vector3Int third = new Vector3Int(3, -3, 0);
        Vector3Int fourth = new Vector3Int(-3, 3, 0);
        Vector3Int five = new Vector3Int(-3, -5, 0);


        //ExplodeFX(first);
        //ExplodeFX(second);
        //ExplodeFX(third);
        //ExplodeFX(five);
        //first a then b then c... etc
        //1a//1b
        //2a//2b
        //3a//3b


    }

    public async void ExplodeFX(Vector3Int location, List<Vector3Int> tilesToRemove)
    {
        //the bomb should not have a dice number or it should be higher than 6
        //if (TilePos[location].number != DiceNumber.Zero)
        //    return;

        List < Vector3Int > ExplodingTiles = new List<Vector3Int>();

        ///do the exploding animation
        List<Task> tasks = new List<Task>();
        tasks.Add(diceFXController.FXTask(DiceFXController.TileEffect.explosion, location));
        //first a then b then c... etc
        //1a//1b
        //2a//2b
        //3a//3b

        for (int x = location.x + -1; x <= location.x - -1; x+=1)
        {
            if (x < Enum.GetValues(typeof(XGridCell)).Cast<int>().Min() || x > Enum.GetValues(typeof(XGridCell)).Cast<int>().Max())//bounds
                continue;

                //Debug.Log($"Y is starting {location.y - -1} and stopping at {location.y - 1}");
                //good
            for (int y = location.y - -1; y >= location.y - 1; y+=-1)
            {
                if (y < Enum.GetValues(typeof(YGridCell)).Cast<int>().Min() || y > Enum.GetValues(typeof(YGridCell)).Cast<int>().Max())//bounds
                    continue;

                //ignoring the bomb tile or empty tiles
                Vector3Int tileLocation = new Vector3Int(x, y, 0);
                if (location.x == x && location.y == y || (!mainMap.HasTile(tileLocation)))
                    continue;

              //TODO make these different animation then the pop animation. i can excluded these compltely from that logic


                //ExplodingTiles.Add(explosion);
                //Debug.Log($"The bomb is at {location}: all the surrounding tiles are {x},{y}");
            }
        }

        //await the bomb fx
        //return the surrounding bomb tiles
        await Task.WhenAll(tasks);

        //score the bomb
        bombEvent?.Invoke(ExplodingTiles.Count);
        //add the exploding tiles to the list to be removed
        ExplodingTiles.ForEach(tile => tilesToRemove.Add(tile));


    }

    #region Test Functions

    private void MouseTileReader()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Vector3 worldPoint = ray.GetPoint(-ray.origin.z / ray.direction.z);
        Vector3Int position = grid.WorldToCell(worldPoint);
        TileBase clickedTile = mainMap.GetTile(position);
        ReadValues(position);
    }

    void ReadValues(Vector3Int position)
    {
        Debug.Log($"At Position {TilePos[position].position} there is a value of {TilePos[position].number} and color {TilePos[position].color}");
    }

    void ReadValues()
    {
        foreach (KeyValuePair<Vector3Int, DiceImprint> entry in TilePos)
        {
            if ((int)entry.Value.number > 0)
            {
                Debug.Log($"At Position {entry.Key} there is a value of {entry.Value.number} and color {entry.Value.color}");
            }

        }
    }
    void RemoveDiceClicked(Vector3Int position)
    {
        diceBoard.Clear(position);
        TilePos[position] = new DiceImprint(position);
        //ApplyGravity();
    }
    #endregion

}
