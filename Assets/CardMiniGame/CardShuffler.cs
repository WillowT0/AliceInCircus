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
        List<Transform> cardList = new List<Transform>();
        foreach (Transform child in transform)
        {
            cardList.Add(child);
        }

        for (int i = 0; i < cardList.Count; i++)
        {
            int randomIndex = Random.Range(i, cardList.Count);
            
            cardList[i].SetSiblingIndex(randomIndex);
            cardList[randomIndex].SetSiblingIndex(i);
        }
    }
}
