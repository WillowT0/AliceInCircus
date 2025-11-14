using UnityEngine;

public class QTETrigger2D : MonoBehaviour
{
    public PointerController pointerController;
    public GameObject qteUI; // Your QTE Canvas/UI

    private bool triggered = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !triggered)
        {
            triggered = true;
            qteUI.SetActive(true);
            pointerController.StartQTE();
        }
    }
}
