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

    [Header("Czajnik")]
    [Tooltip("Przeciągnij tutaj sprite czajnika")]
    [SerializeField] private Transform teapot;

    [Tooltip("O ile stopni przechylić czajnik podczas lania")]
    [SerializeField] private float tiltAngle = -15f;

    private SpriteRenderer teaSprite;
    private Collider2D teaCollider;

    private Quaternion defaultRotation;

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

        if (teapot == null)
        {
            Debug.LogError("Nie przypisano obiektu 'teapot' w inspektorze!", this);
            return;
        }

        defaultRotation = teapot.rotation;

        StartCoroutine(TeaCycle());
    }

    private IEnumerator TeaCycle()
    {
        while (true)
        {
            // TEA OFF (safe)
            teaSprite.enabled = false;
            teaCollider.enabled = false;

            // Un-tilt the teapot
            teapot.rotation = defaultRotation;

            yield return new WaitForSeconds(inactiveTime);

            // TEA ON (spilling)
            teaSprite.enabled = true;
            teaCollider.enabled = true;

            // Tilt teapot
            teapot.rotation = Quaternion.Euler(0, 0, tiltAngle);

            yield return new WaitForSeconds(activeTime);
        }
    }
}
