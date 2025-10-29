using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardController : MonoBehaviour
{
    [Header("Card Setup")]
    [SerializeField] private Card cardPrefab;
    [SerializeField] private Transform gridTransform;
    [SerializeField] private Sprite[] sprites;

    private List<Sprite> spritePairs = new List<Sprite>();
    private Card firstSelected;
    private Card secondSelected;

    private int matchCounts;
    private bool gameActive = false;

    private void Start()
    {
        Debug.Log("CardController ready — starting Memory Minigame automatically.");
        InitializeMinigame(); 
    }

    // Initialize and start the memory game
    public void InitializeMinigame()
    {
        // Clear old cards (if replayed)
        foreach (Transform child in gridTransform)
        {
            Destroy(child.gameObject);
        }

        // Prepare new cards
        PrepareSprites();
        CreateCards();

        matchCounts = 0;
        firstSelected = null;
        secondSelected = null;
        gameActive = true;

        Debug.Log("Memory Minigame Initialized!");
    }

    public void SetSelected(Card card)
    {
        if (!gameActive) return;
        if (card.isSelected) return;

        card.Show();

        if (firstSelected == null)
        {
            firstSelected = card;
            return;
        }

        if (secondSelected == null)
        {
            secondSelected = card;
            StartCoroutine(CheckMatching(firstSelected, secondSelected));
            firstSelected = null;
            secondSelected = null;
        }
    }

    private IEnumerator CheckMatching(Card a, Card b)
    {
        yield return new WaitForSeconds(0.3f);

        if (a.iconSprite == b.iconSprite)
        {
            matchCounts++;

            if (matchCounts >= spritePairs.Count / 2)
            {
                Debug.Log("All pairs matched — player wins!");
                gameActive = false;
                OnGameComplete();
            }
        }
        else
        {
            a.Hide();
            b.Hide();
        }
    }

    private void OnGameComplete()
    {
     
    }

    private IEnumerator RestartAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        InitializeMinigame();
    }

    private void PrepareSprites()
    {
        spritePairs.Clear();

        for (int i = 0; i < sprites.Length; i++)
        {
            spritePairs.Add(sprites[i]);
            spritePairs.Add(sprites[i]); // duplicate for matching pair
        }

        ShuffleSprites(spritePairs);
    }

    private void CreateCards()
    {
        for (int i = 0; i < spritePairs.Count; i++)
        {
            Card card = Instantiate(cardPrefab, gridTransform);
            card.SetIconSprite(spritePairs[i]);
            card.controller = this;
        }
    }

    private void ShuffleSprites(List<Sprite> spriteList)
    {
        for (int i = spriteList.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);

            Sprite temp = spriteList[i];
            spriteList[i] = spriteList[randomIndex];
            spriteList[randomIndex] = temp;
        }
    }
}
