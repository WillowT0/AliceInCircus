using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    public int id; // unikalny ID dla pary
    public Image frontImage;
    public Image backImage;
    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
    }

    public void OnClick()
    {
        GameManager.instance.CardRevealed(this);
    }

    public void Reveal()
    {
        frontImage.gameObject.SetActive(true);
        backImage.gameObject.SetActive(false);
    }

    public void Hide()
    {
        frontImage.gameObject.SetActive(false);
        backImage.gameObject.SetActive(true);
    }
}
