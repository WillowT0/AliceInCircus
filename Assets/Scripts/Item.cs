using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "ScriptableObjects/Item")]
public class Item : ScriptableObject
{
    public string ItemID;            
    public string itemName;          
    public int itemCount;
    public Sprite itemIcon;
    public bool isStackable = true;
}

public static class ScriptableObjectExtension
{
    public static T Clone<T>(this T scriptableObject) where T : ScriptableObject
    {
        if (scriptableObject == null)
        {
            Debug.LogError($"ScriptableObject was null. Returning default {typeof(T)} object.");
            return ScriptableObject.CreateInstance<T>();
        }

        T instance = Object.Instantiate(scriptableObject);
        instance.name = scriptableObject.name;
        return instance;
    }
}
