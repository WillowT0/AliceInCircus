using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Memory Cards")]
    public List<Card> cards = new List<Card>(); // Assign all cards in inspector or leave empty to auto-find

    private Card firstCard;
    private Card secondCard;
    private bool canClick = true;
    private int pairsFound = 0;
    private int totalPairs = 0;

    public event Action OnGameWin;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        // Auto-find cards if none assigned
        if (cards.Count == 0)
        {
            cards.AddRange(FindObjectsOfType<Card>());
        }

        // Safety check
        if (cards.Count % 2 != 0)
        {
            Debug.LogWarning("Warning: Odd number of cards detected. Make sure cards are in pairs.");
        }

        totalPairs = cards.Count / 2;
        Debug.Log($"Memory game initialized with {cards.Count} cards ({totalPairs} pairs).");

        ResetGame();
    }

    public void CardRevealed(Card card)
    {
        if (!canClick || card.IsMatched || card == firstCard) return;

        card.Reveal();

        if (firstCard == null)
        {
            firstCard = card;
            return;
        }

        secondCard = card;
        StartCoroutine(CheckMatch());
    }

    private IEnumerator CheckMatch()
    {
        canClick = false;

        // Wait so player sees the second card
        yield return new WaitForSeconds(0.5f);

        if (firstCard.id == secondCard.id)
        {
            firstCard.IsMatched = true;
            secondCard.IsMatched = true;
            pairsFound++;

            Debug.Log($"Match found: {firstCard.id} (Pairs found: {pairsFound}/{totalPairs})");

            // Trigger win only if all pairs are matched
            if (pairsFound >= totalPairs)
            {
                Debug.Log("Player won the memory game!");
                OnGameWin?.Invoke();
            }
        }
        else
        {
            firstCard.Hide();
            secondCard.Hide();
        }

        firstCard = null;
        secondCard = null;
        canClick = true;
    }

    public void ResetGame()
    {
        firstCard = null;
        secondCard = null;
        canClick = true;
        pairsFound = 0;

        foreach (var card in cards)
        {
            card.IsMatched = false;
            card.Hide();
        }

        Debug.Log("Memory game reset.");
    }
}
