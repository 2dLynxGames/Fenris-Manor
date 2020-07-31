using UnityEngine;
using System.Collections;

public class DestroyObjectOverTime : MonoBehaviour {

	public float lifeTime;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		lifeTime = lifeTime - Time.deltaTime;

		if(lifeTime <= 0f)
		{
			if (gameObject.tag == "Enemy") {
				gameObject.GetComponentInParent<SpawnEnemy>().EnemyKilled();
			}
			Destroy(gameObject);
		}
	}
}
