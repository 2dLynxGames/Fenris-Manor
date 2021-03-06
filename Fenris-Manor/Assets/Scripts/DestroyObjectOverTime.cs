﻿using UnityEngine;
using System.Collections;

public class DestroyObjectOverTime : MonoBehaviour {

	public float lifeTime;
	
	// Update is called once per frame
	void Update () {
		lifeTime = lifeTime - Time.deltaTime;

		if(lifeTime <= 0f)
		{
			if (gameObject.GetComponentInParent<SpawnEnemy>()) {
				gameObject.GetComponentInParent<SpawnEnemy>().EnemyKilled();
			}
			Destroy(gameObject);
		}
	}
}
