using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameHandler : MonoBehaviour {

    [SerializeField] private GameObject unitGameObject;
    private IUnit unit;

    private void Awake() {
        unit = unitGameObject.GetComponent<IUnit>();
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.S)) {
            Save();
        }

        if (Input.GetKeyDown(KeyCode.L)) {
            Load();
        }
    }

    private void Save() {
        // Save
        Vector3 playerPosition = unit.GetPosition();
        int goldAmount = unit.GetGoldAmount();

        PlayerPrefs.SetFloat("playerPositionX", playerPosition.x);
        PlayerPrefs.SetFloat("playerPositionY", playerPosition.y);

        PlayerPrefs.SetInt("goldAmount", goldAmount);

        PlayerPrefs.Save();

    }

    private void Load() {
        // Load
        if (PlayerPrefs.HasKey("playerPositionX")) {
            float playerPositionX = PlayerPrefs.GetFloat("playerPositionX");
            float playerPositionY = PlayerPrefs.GetFloat("playerPositionY");
            Vector3 playerPosition = new Vector3(playerPositionX, playerPositionY);
            int goldAmount = PlayerPrefs.GetInt("goldAmount", 0);

            unit.SetPosition(playerPosition);
            unit.SetGoldAmount(goldAmount);
        } else {
            // No save is available
        }
    }

    //marathon highscore
    //time attack highscore
    //line breaker highscore
    //

}
