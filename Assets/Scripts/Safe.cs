using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cat : MonoBehaviour {

	Rigidbody2D rb;

	[SerializeField]
	GameObject codePanel, closedSafe, openedSafe;

	public static bool isSafeOpened = false;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D> ();
		codePanel.SetActive (false);
		closedSafe.SetActive (true);
		openedSafe.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
	

		if (isSafeOpened) {
			codePanel.SetActive (false);
			closedSafe.SetActive (false);
			openedSafe.SetActive (true);
		}
	}
    private void FixedUpdate()
    {

    }
    void OnTriggerEnter2D(Collider2D col)
	{
		if (col.gameObject.name.Equals ("Safe") && !isSafeOpened) {
			codePanel.SetActive (true);
		}
	}

	void OnTriggerExit2D(Collider2D col)
	{
		if (col.gameObject.name.Equals ("Safe")) {
			codePanel.SetActive (false);
		}
	}

    void Awake()
    {
        isSafeOpened = false;
    }
}
