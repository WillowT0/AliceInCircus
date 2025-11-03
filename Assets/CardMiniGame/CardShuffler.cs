using System.Collections.Generic;

using UnityEngine;



public class CardShuffler : MonoBehaviour

{

    void Start()

    {

        ShuffleCards();

    }



    public void ShuffleCards()

    {


        // Pobieramy wszystkie karty bêd¹ce dzieæmi tego obiektu

        List<Transform> cardList = new List<Transform>();

        foreach (Transform child in transform)

        {

            cardList.Add(child);

        }



        // Tasowanie (Fisher-Yates)

        for (int i = 0; i < cardList.Count; i++)

        {

            int randomIndex = Random.Range(i, cardList.Count);

            // Zamiana pozycji w hierarchii

            cardList[i].SetSiblingIndex(randomIndex);

            cardList[randomIndex].SetSiblingIndex(i);

        }

    }

}