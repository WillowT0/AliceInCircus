using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    private void Awake() { instance = this; }

    public List<Card> cards;
    private Card firstCard;
    private Card secondCard;
    private bool canClick = true;

    public void CardRevealed(Card card)
    {
        if (!canClick || card == firstCard) return;

        card.Reveal();

        if (firstCard == null)
        {
            firstCard = card;
        }
        else
        {
            secondCard = card;
            StartCoroutine(CheckMatch());
        }
    }

    IEnumerator CheckMatch()
    {
        canClick = false;

        if (firstCard.id == secondCard.id)
        {
            // dopasowane
            firstCard = null;
            secondCard = null;
        }
        else
        {
            // nie dopasowane, odwracamy po 1 sek
            yield return new WaitForSeconds(1f);
            firstCard.Hide();
            secondCard.Hide();
            firstCard = null;
            secondCard = null;
        }

        canClick = true;
    }
}
