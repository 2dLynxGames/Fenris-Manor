using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetObject : MonoBehaviour
{
    private Vector2 startPosition;
	private Quaternion startRotation;
	private Vector2 startScale;

    private Rigidbody2D rigidbody2d;
    // Start is called before the first frame update
    void Start() {
        startPosition = transform.position;
		startRotation = transform.rotation;
		startScale = transform.localScale;

		if(GetComponent<Rigidbody2D>() != null)
		{
			rigidbody2d = GetComponent<Rigidbody2D>();
		}
	}

    public void ResetThisObject() {
        transform.position = startPosition;
        transform.rotation = startRotation;
        transform.localScale = startScale;

        if (rigidbody2d != null) {
            rigidbody2d.velocity = Vector2.zero;
        }
    }
}
