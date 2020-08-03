using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject followTarget;
    public float followAhead;
    public float smoothSpeed;

    public GameObject minXY;
    public GameObject maxXY;

    private bool verticalStage = false;
    private GameObject player;
    private PlayerController playerController;
    
    private Camera mainCamera;
    private float cameraHeightFromCenter;
    private float cameraWidthFromCenter;

    private Vector3 targetPosition;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = FindObjectOfType<Camera>();
        cameraHeightFromCenter = mainCamera.orthographicSize;
        cameraWidthFromCenter = Mathf.Round(cameraHeightFromCenter * mainCamera.aspect) + 0.5f;

        player = GameObject.Find("Player");
        playerController = player.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        targetPosition = new Vector3(followTarget.transform.position.x, transform.position.y, transform.position.z);

        if(playerController.GetFacing() == PlayerController.FACING.right && !verticalStage) {
            targetPosition.x += followAhead;    
        } else{
            targetPosition.x -= followAhead;    
        }

        if((targetPosition.x + cameraWidthFromCenter) > maxXY.transform.position.x) {
            targetPosition.x = maxXY.transform.position.x - cameraWidthFromCenter;
        } else if ((targetPosition.x - cameraWidthFromCenter) < minXY.transform.position.x) {
            targetPosition.x = minXY.transform.position.x + cameraWidthFromCenter;
        }

        transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);
    }
}
