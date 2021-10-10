using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DiceBoard : MonoBehaviour
{

        public DiceData[] DiceOptions;
        public Vector3Int spawnPosition;
        public Tilemap tilemap{get; private set;}
        public DiceGroup activeGroup {get; private set;}
        //public Vector2Int boardSize= new Vector2Int(10,20);
        // public RectInt Bounds {
        //     get
        //     {
        //     Vector2Int position = new Vector2Int(-this.boardSize.x / 2, -this.boardSize.y / 2);
        //     return new RectInt(position, this.boardSize);
        //     }
        // }   


    // Start is called before the first frame update
    void Awake()
    {
        this.tilemap = GetComponentInChildren<Tilemap>();
        this.activeGroup = GetComponentInChildren<DiceGroup>();

        //initialize all the possible options with there cells array
        for(int i = 0;i<this.DiceOptions.Length;i++){
        this.DiceOptions[i].Initialize();
        }
        
    }
    void Start()
    {
        SpawnGroup();
    }

    void SpawnGroup()
    {
        int random = Random.Range(0,this.DiceOptions.Length);
        DiceData newGroup = this.DiceOptions[random];
        this.activeGroup.Initialize(this, spawnPosition, newGroup);
        SetOnBoard(this.activeGroup);//pass the dice group collection to be placed on the board

                
  

    }   
    // Update is called once per frame
    void Update()
    {
        
    }

    //place the cell/tilegroup in the active dice set on the board
    public void SetOnBoard(DiceGroup diceGroup)
    {
        for(int i = 0;i< diceGroup.cells.Length;i++)
        {
            //each tile in this dice group array is placed on the map
            Vector3Int tilePosition = diceGroup.cells[i];//+ piece.position
            //what is the position it needs to be placed(more important if there is more than 1)/which tile to use
            //we have a position where we are falling/place what are the coordinates from that location do the other tiles need to go to fill in that shape
            this.tilemap.SetTile(tilePosition,diceGroup.data.tile);//
        }

    }
}
