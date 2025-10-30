using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class Card : MonoBehaviour
{
    public int id;
    public Image frontImage;
    public Image backImage;

    [HideInInspector]
    public bool IsMatched = false;

    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);

        Hide();
    }

    public void OnClick()
    {
        if (!IsMatched)
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