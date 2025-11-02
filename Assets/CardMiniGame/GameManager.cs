using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Memory Cards")]
    [Tooltip("Assign all cards in inspector or leave empty to auto-find.")]
    public List<Card> cards = new List<Card>();

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
        // Auto-find cards if none are manually assigned
        if (cards.Count == 0)
            cards.AddRange(FindObjectsOfType<Card>(true)); // include inactive cards


        if (cards.Count % 2 != 0)
            Debug.LogWarning("Warning: Odd number of cards detected. Ensure cards are in pairs.");

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
        }
        else
        {
            secondCard = card;
            StartCoroutine(CheckMatch());
        }
    }

    private IEnumerator CheckMatch()
    {
        canClick = false;

        // Wait briefly so the player can see the second card
        yield return new WaitForSeconds(0.5f);

        if (firstCard.id == secondCard.id)
        {
            // Match found
            firstCard.IsMatched = true;
            secondCard.IsMatched = true;
            pairsFound++;

            Debug.Log($" Match found: {firstCard.id} (Pairs: {pairsFound}/{totalPairs})");

            // Check for win
            if (pairsFound >= totalPairs)
            {
                Debug.Log(" Player won the memory game!");
                OnGameWin?.Invoke();
            }
        }
        else
        {
            // Not a match — wait a bit and hide
            yield return new WaitForSeconds(0.5f);

            firstCard.Hide();
            secondCard.Hide();

            Debug.Log($" No match: {firstCard.id} vs {secondCard.id}");
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

        Debug.Log(" Memory game reset.");
    }
}
