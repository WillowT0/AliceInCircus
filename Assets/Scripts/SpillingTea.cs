using System.Collections;
using UnityEngine;

public class SpillingTea : MonoBehaviour
{
    [Header("Ustawienia Czasu")]
    [Tooltip("Ile sekund herbata jest rozlana (niebezpieczna)")]
    [SerializeField] private float activeTime = 2.0f;
    [Tooltip("Ile sekund przerwy między rozlaniami (bezpieczna)")]
    [SerializeField] private float inactiveTime = 3.0f;

    [Header("Elementy Herbaty (Dzieci)")]
    [Tooltip("Przeciągnij tutaj obiekt 'SpilledTea' z hierarchii")]
    [SerializeField] private GameObject spilledTeaObject;

    private SpriteRenderer teaSprite;
    private Collider2D teaCollider;

    void Start()
    {
        if (spilledTeaObject != null)
        {
            teaSprite = spilledTeaObject.GetComponent<SpriteRenderer>();
            teaCollider = spilledTeaObject.GetComponent<Collider2D>();
        }
        else
        {
            Debug.LogError("Nie przypisano obiektu 'SpilledTea' w inspektorze!", this);
            return;
        }

        StartCoroutine(TeaCycle());
    }

    private IEnumerator TeaCycle()
    {
        while (true)
        {
        
            teaSprite.enabled = false;
            teaCollider.enabled = false;

            yield return new WaitForSeconds(inactiveTime);

            teaSprite.enabled = true;
            teaCollider.enabled = true;

            yield return new WaitForSeconds(activeTime);
        }
    }
}