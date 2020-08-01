using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour {

    public GameObject roomSpawnPoint;
    public CameraController cameraController;
    public float timeToWait = 1;

    protected PlayerController playerController;
    protected PlayerPlatformerController platformerController;
    protected LevelManager levelManager;
    
    public GameObject newRoomMinXY;
    public GameObject newRoomMaxXY;

    private EnemyController[] enemies;

    // TODO: Make a player walking animation that starts once the door is open and ends upon reachign destination. 

    void Start() {
        cameraController = FindObjectOfType<CameraController>();
        levelManager = FindObjectOfType<LevelManager>();
    }

	public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag != "Player")
            return;
        if (playerController = other.GetComponent<PlayerController>()) {
            StartCoroutine(PlayerInDoor());
            StartCoroutine(playerController.DisableControls(other.gameObject, timeToWait));
            cameraController.minXY = newRoomMinXY;
            cameraController.maxXY = newRoomMaxXY;
            other.gameObject.transform.position = roomSpawnPoint.transform.position;
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
