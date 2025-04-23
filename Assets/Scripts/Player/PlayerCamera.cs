using System;
using UnityEngine;

public class PlayerCamera : MonoBehaviour {

    // External references
    private Camera playerCamera;
    
    private Transform playerCameraParent;
    private Transform playerTransform;
    private PlayerInputManager input;

    // Camera rotation and position
    private float horizontalInput, verticalInput;
    private float xRotation, yRotation = 0.0f; // Track the camera X rotation (pitch), and the Y rotation (yaw)

    private Quaternion targetRotation; // Desired camera rotation



    // Camera settings
    private float smoothTime = 0.025f; //Camera rotation smoothing time

    private Vector3 cameraPositionOffset = new Vector3(0.0f, 0.0f, -8f);
    private Vector3 cameraParentPositionOffset = new Vector3(0.0f, 2f, 0.0f);

    // Mouse and cursor settings
    [Range(0.1f, 10.0f)][SerializeField] private float mouseSensitivity = 1.0f;
    private bool isCursorLocked = false;

    private void Start() {
        try {
            playerCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
            playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
            playerCameraParent = GameObject.FindGameObjectWithTag("PlayerCameraParent").GetComponent<Transform>();
        }
        catch {
            Debug.LogError("Not all the camera external references have been found.\nMake sure tha all the objects with their respective exists in the game scene.");
        }

        input = GetComponent<PlayerInputManager>();

        // Initialize the camera rotation
        targetRotation = playerCamera.transform.localRotation;

        ToggleLockCursor();
    }

    private void Update() {
        HandleMouseInput();
        HandleCameraRotation();
        HandleCameraPosition();
    }

    private void HandleMouseInput() {
        float mouseSensitivityMultiplaier = 0.1f;

        verticalInput = input.cameraLookDirection.y * (mouseSensitivity * mouseSensitivityMultiplaier);
        horizontalInput = input.cameraLookDirection.x * (mouseSensitivity * mouseSensitivityMultiplaier);

        // Adjust camera pitch and clamp it in order to avoid flipping.
        xRotation -= verticalInput;
        xRotation = Mathf.Clamp(xRotation, -80.0f, 80.0f);

        yRotation += horizontalInput;
    }

    private void HandleCameraRotation() {
        // Create the target rotation using pitch and player yaw.
        targetRotation = Quaternion.Euler(xRotation, yRotation, 0.0f);

        // Linearly interpolates the camera rotation to avoid jittering.
        playerCameraParent.transform.rotation = Quaternion.Lerp(
            playerCameraParent.transform.rotation,
            targetRotation,
            smoothTime / Time.deltaTime);
    }

    private void HandleCameraPosition() {
        float cameraParentFollowSpeed = 20.0f;

        // Smoothly move the camera to follow the player with the position offset to avoid jittering.
        Vector3 playerTargetPosition = playerTransform.position + cameraParentPositionOffset;
        playerCameraParent.transform.position = Vector3.Lerp(playerCameraParent.transform.position, playerTargetPosition, cameraParentFollowSpeed * Time.deltaTime);

        playerCamera.transform.localPosition = cameraPositionOffset;
    }

    private void ToggleLockCursor() {
        if (!isCursorLocked) {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        isCursorLocked = !isCursorLocked;
    }
}
