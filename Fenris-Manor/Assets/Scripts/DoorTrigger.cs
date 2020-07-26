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

    void Start() {
        cameraController = FindObjectOfType<CameraController>();
    }

	public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag != "Player")
            return;
        StartCoroutine(DisableControls());
        cameraController.minXY = newRoomMinXY;
        cameraController.maxXY = newRoomMaxXY;
        collision.gameObject.transform.position = roomSpawnPoint.transform.position;
    }

    IEnumerator DisableControls() {
        GameObject player = GameObject.Find("Player");
        if ((platformerController = player.GetComponent<PlayerPlatformerController>()) && 
                (playerController = player.GetComponent<PlayerController>())) {

            Debug.Log("Controls Off");
            ToggleControls(false);
            playerController.playerAnimator.SetBool("idle", true);
            // TODO: Make thise duration different based upon the door open/close animation length once the door is created.
            // TODO: Make a player walking animation that starts once the door is open and ends upon reachign destination. 
            yield return new WaitForSeconds(1);
            platformerController.SetPlayerVelocity(new Vector2(platformerController.GetPlayerVelocity().x, 0));
            Debug.Log("Controls On");
            ToggleControls(true);

        }
    }

    void ToggleControls(bool state) {
        platformerController.enabled = state;
        playerController.playerAnimator.SetBool("idle", !state);
    }
}
