using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    public int id;
    private bool isRevealed = false;
    public bool IsMatched { get; private set; }

    [Header("References")]
    [SerializeField] private GameObject front;
    [SerializeField] private GameObject back;
    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
        if (button != null)
            button.onClick.AddListener(OnCardClicked);

        Hide(); // start hidden
    }

    private void OnCardClicked()
    {
        if (!IsMatched)
            GameManager.instance.CardRevealed(this);
    }

    public void Reveal()
    {
        if (IsMatched) return;
        isRevealed = true;
        front.SetActive(true);
        back.SetActive(false);
        Debug.Log($"[Card {id}] Revealed!");
    }

    public void Hide()
    {
        if (IsMatched) return;
        isRevealed = false;
        front.SetActive(false);
        back.SetActive(true);
        Debug.Log($"[Card {id}] Hidden!");
    }

    public void Match()
    {
        IsMatched = true;
        if (button != null)
            button.interactable = false;
    }
}
