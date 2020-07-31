using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : MonoBehaviour {

    public GameObject roomSpawnPoint;
    public CameraController cameraController;

    protected PlayerController playerController;
    protected PlayerPlatformerController platformerController;
    
    public GameObject newRoomMinXY;
    public GameObject newRoomMaxXY;

    // TODO: Make a player walking animation that starts once the door is open and ends upon reachign destination. 

    void Start() {
        cameraController = FindObjectOfType<CameraController>();
    }

	public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag != "Player")
            return;
        if (playerController = other.GetComponent<PlayerController>()) {
            StartCoroutine(playerController.DisableControls(other.gameObject, 1));
            cameraController.minXY = newRoomMinXY;
            cameraController.maxXY = newRoomMaxXY;
            other.gameObject.transform.position = roomSpawnPoint.transform.position;
        }
    }
}
