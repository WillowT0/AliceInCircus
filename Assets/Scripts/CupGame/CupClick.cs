using UnityEngine;

public class CupClick : MonoBehaviour
{
    public int cupIndex;
    private CupGameController controller;

    void Start()
    {
        controller = FindObjectOfType<CupGameController>();
    }

    void OnMouseDown()
    {
        controller.ChooseCup(cupIndex);
    }
}
