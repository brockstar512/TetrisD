using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateCard : MonoBehaviour
{

    public GameObject cardPrefab;

    void InstantiateNewCard(){
        GameObject newCard = Instantiate(cardPrefab, transform,false);
        //place it first in the hierchy so its behind the main card
        newCard.transform.SetAsFirstSibling();
    }

    // Update is called once per frame
    void Update()
    {
        if(transform.childCount < 2)
        {
            InstantiateNewCard();
        }
    }
}
