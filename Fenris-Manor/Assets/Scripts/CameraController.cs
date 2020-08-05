using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    protected CinemachineVirtualCamera virtualCamera;
    protected PlayerController playerController;

    void Awake() {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        playerController = FindObjectOfType<PlayerController>();
    }

    void LateUpdate() {
        if (playerController.GetFacing() == PlayerController.FACING.right) {
            virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_ScreenX = 0.35f;
        } else {
            virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>().m_ScreenX = 0.65f;
        }
    }
        
}

