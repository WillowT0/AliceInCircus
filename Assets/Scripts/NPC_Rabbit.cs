using UnityEngine;

public class NPC_CheckItem : MonoBehaviour
{
    [Header("Ustawienia przedmiotu")]
    public string itemToCheck = "clock";

    [Header("Canvas do otwarcia")]
    public GameObject canvasToOpen;

    [Header("Finish Point")]
    public FinishPoint finishPoint; // Drag the FinishPoint object here in the Inspector

    [Header("Debug opcje")]
    public bool debugMode = true;

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
            UnlockFinishPoint();
        }
    }

    void CheckItem()
    {
        if (inventoryManager.HasItem(itemToCheck))
        {
            Debug.Log("Gracz ma item!");
            OpenCanvas();
            UnlockFinishPoint();
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

    //  Inform FinishPoint that NPC check was done
    void UnlockFinishPoint()
    {
        Debug.Log("[NPC] Trying to unlock FinishPoint...");

        if (finishPoint != null)
        {
            finishPoint.AllowNextLevel();
            Debug.Log("[NPC] FinishPoint unlocked!");
        }
        else
        {
            Debug.LogWarning("[NPC] FinishPoint reference is missing!");
        }
    }

}
