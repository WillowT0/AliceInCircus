using UnityEngine;

public class NPC_CheckItem : MonoBehaviour
{
    public string itemToCheck = "clock"; // Item ID to check
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

    void CheckItem()
    {
        if (inventoryManager.HasItem(itemToCheck))
        {
            Debug.Log("Gracz ma item!");
        }
        else
        {
            Debug.Log("Gracz nie ma itemu.");
        }
    }
}
