using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DiceBoard : MonoBehaviour
{
    //-TODO disengage blocks that can still fall
    //-TODO document what I have

    //-todo when all of the above is done.... then work on a bejewelled tutprial to figure out how to handle taking out dice
    public DiceData[] DiceOptions;
        public Vector3Int spawnPosition;
        public Tilemap tilemap{get; private set;}
        public DiceGroup activeGroup {get; private set;}
        //for is valid
        public Vector2Int boardSize= new Vector2Int(6,9);//cells on the board
        public RectInt Bounds {
            get
            {
            Vector2Int position = new Vector2Int(-this.boardSize.x / 2, -this.boardSize.y / 2);
            return new RectInt(position, this.boardSize);
            }
        }   


    // Start is called before the first frame update
    void Awake()
    {
        this.tilemap = GetComponentInChildren<Tilemap>();
        this.activeGroup = GetComponentInChildren<DiceGroup>();

        //initialize all the possible options with their cells array
        for(int i = 0;i<this.DiceOptions.Length;i++){
        this.DiceOptions[i].Initialize();
        }
        
    }
    void Start()
    {
        SpawnGroup();
    }

    public void SpawnGroup()
    {
        int random = Random.Range(0,this.DiceOptions.Length);
        DiceData newGroup = this.DiceOptions[random];
        int random2 = Random.Range(0, this.DiceOptions.Length);
        DiceData newGroup2 = this.DiceOptions[random2];
        this.activeGroup.Initialize(this, spawnPosition, newGroup, newGroup2);
        SetOnBoard(this.activeGroup);//pass the dice group collection to be placed on the board

    }   
    // Update is called once per frame
    void Update()
    {
        
    }

    //TODO explain how this works
    //place the cell/tilegroup in the active dice set on the board
    public void SetOnBoard(DiceGroup diceGroup)
    {
        this.tilemap.SetTile(diceGroup.cells[1] + diceGroup.position, diceGroup.dynamicData.tile);
        this.tilemap.SetTile(diceGroup.cells[0] + diceGroup.position, diceGroup.data.tile);

       
        /*
        for(int i = 0;i< diceGroup.cells.Length;i++)
        {
            //each tile in this dice group array is placed on the map
            Vector3Int tilePosition = diceGroup.cells[i]+ diceGroup.position;
            //what is the position it needs to be placed(more important if there is more than 1)/which tile to use
            //we have a position where we are falling/place what are the coordinates from that location do the other tiles need to go to fill in that shape


            //TODO explain why this is also taken out
            //THIS SHOULD ITERATIVLY place second dice [] of datadice
            //this.tilemap.SetTile(tilePosition, diceGroup.datacollection[i].tile);//
            this.tilemap.SetTile(tilePosition,diceGroup.dynamicData.tile);//this is places 2nd dice on
            this.tilemap.SetTile(tilePosition, diceGroup.data.tile);//this is placeing 1st dice on

        }*/


    }



    public void Clear(DiceGroup group)
    {
        for(int i = 0;i< group.cells.Length;i++)
        {
            Vector3Int tilePosition = group.cells[i] + group.position;
            this.tilemap.SetTile(tilePosition,null);
        }

    }

    public bool IsValidPosition(DiceGroup group, Vector3Int position)
    {
        //Debug.Log($"Is valid?"); 
        RectInt bounds = this.Bounds;
        // Debug.Log("Here are the bounds "+bounds);
        // The position is only valid if every cell is valid
        for (int i = 0; i < group.cells.Length; i++)
        {
            Vector3Int tilePosition = group.cells[i] + position;
            //Debug.Log("Here is the tile position "+ (Vector2Int)tilePosition);
            // An out of bounds tile is invalid
            if (!bounds.Contains((Vector2Int)tilePosition)) {
                //Debug.LogError("bounds does not contain that position");
                return false;
            }

            // A tile already occupies the position, thus invalid
            if (this.tilemap.HasTile(tilePosition)) {
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
    


}
