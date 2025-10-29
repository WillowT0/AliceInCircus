using UnityEngine;

public class NPC_CheckItem : MonoBehaviour
{
    [Header("Ustawienia przedmiotu")]
    public string itemToCheck = "clock"; // ID przedmiotu do sprawdzenia

    [Header("Canvas do otwarcia")]
    public GameObject canvasToOpen;      // Canvas przypisany w Inspectorze

    [Header("Debug opcje")]
    public bool debugMode = true;        // Czy debugowanie jest włączone

    private InventoryManager inventoryManager;

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            inventoryManager = FindObjectOfType<InventoryManager>();

            if (inventoryManager != null)
            {
                CheckItem();
            }
            else
            {
                Debug.LogWarning("Nie znaleziono InventoryManager w scenie!");
            }
        }
        else
        {
            Debug.LogWarning("Nie znaleziono gracza z tagiem 'Player'.");
        }
    }

    void Update()
    {
        // Tryb debug – naciśnij "U", aby zasymulować posiadanie itemu
        if (debugMode && Input.GetKeyDown(KeyCode.U))
        {
            Debug.Log("[DEBUG] Wciśnięto U — symulacja posiadania itemu!");
            OpenCanvas();
        }
    }

    void CheckItem()
    {
        if (inventoryManager.HasItem(itemToCheck))
        {
            Debug.Log("Gracz ma item!");
            OpenCanvas();
        }
        else
        {
            Debug.Log("Gracz nie ma itemu.");
        }
    }

    void OpenCanvas()
    {
        if (canvasToOpen != null)
        {
            canvasToOpen.SetActive(true);
        }
        else
        {
            Debug.LogWarning("Brak przypisanego Canvasu w Inspectorze!");
        }
    }
}
