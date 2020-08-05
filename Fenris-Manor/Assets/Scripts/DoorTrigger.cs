using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour {

    public GameObject oldCamera;
    public GameObject newCamera;

    public GameObject roomSpawnPoint;
    public float timeToWait = 1;

    protected PlayerController playerController;
    protected PlayerPlatformerController platformerController;
    protected LevelManager levelManager;

    private EnemyController[] enemies;

    // TODO: Make a player walking animation that starts once the door is open and ends upon reachign destination. 

    void Start() {
        levelManager = FindObjectOfType<LevelManager>();
    }

	public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag != "Player")
            return;
        if (playerController = other.GetComponentInParent<PlayerController>()) {
            StartCoroutine(PlayerInDoor());
            StartCoroutine(playerController.DisableControls(other.gameObject, timeToWait));
            newCamera.SetActive(true);
            oldCamera.SetActive(false);
            other.transform.parent.transform.position = roomSpawnPoint.transform.position;
        }
    }

    IEnumerator PlayerInDoor() {
        playerController.SetInDoor(true);
        levelManager.ToggleEnemies(false);
        yield return new WaitForSecondsRealtime(timeToWait);
        levelManager.ToggleEnemies(true);
        playerController.SetInDoor(false);

    }
}
