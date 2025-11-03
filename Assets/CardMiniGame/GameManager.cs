using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private void Awake()
    {
        instance = this;
    }

    [Header("Cards")]
    public List<Card> cards = new List<Card>();

    private Card firstCard;
    private Card secondCard;
    private bool canClick = true;

    //Event that NPC_DodoController will subscribe to
    public event System.Action OnGameWin;

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
            // Mark both as matched
            firstCard.Match();
            secondCard.Match();

            firstCard = null;
            secondCard = null;

            //  Check win condition
            yield return new WaitForSeconds(0.3f);
            CheckIfGameWon();
        }
        else
        {
            // Not matched — hide after delay
            yield return new WaitForSeconds(1f);
            firstCard.Hide();
            secondCard.Hide();
            firstCard = null;
            secondCard = null;
        }

        canClick = true;
    }

    private void CheckIfGameWon()
    {
        foreach (Card c in cards)
        {
            if (!c.IsMatched)
                return; // Still cards to match
        }

        //All matched
        Debug.Log("All pairs found — game won!");
        OnGameWin?.Invoke();
    }
}
