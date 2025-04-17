using UnityEngine;

public class PlayerCamera : MonoBehaviour {

    [Range(0.1f, 10.0f)][SerializeField] private float mouseSensitivity = 1.0f;

    private Camera playerCamera;
    private Transform playerTransform;
    private PlayerInputManager input;

    private float horizontalInput, verticalInput;
    private float xRotation = 0.0f; // Track the camera X rotation (pitch)

    [Header("Camera Settings")]
    [SerializeField] private Vector3 positionOffset = new Vector3(0.0f, 1.75f, 0.0f);
    [SerializeField] private float smoothTime = 0.025f; //Camera rotation smoothing time

    private Quaternion targetRotation; // Desired camera rotation
    private bool isCursorLocked = false;

    private void Start() {
        playerCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        input = GetComponent<PlayerInputManager>();

        // Initialize the camera rotation
        targetRotation = playerCamera.transform.localRotation;

        ToggleLockCursor();
    }

    private void Update() {
        HandleMouseInput();
        HandlePlayerRotation();
        HandleCameraRotation();
        HandleCameraPosition();
    }

    private void HandleMouseInput() {
        float mouseSensitivityMultiplaier = 0.1f;

        horizontalInput = input.cameraLookDirection.x * (mouseSensitivity * mouseSensitivityMultiplaier);
        verticalInput = input.cameraLookDirection.y * (mouseSensitivity * mouseSensitivityMultiplaier);

        // Adjust camera pitch and clamp it to avoid flipping.
        xRotation -= verticalInput;
        xRotation = Mathf.Clamp(xRotation, -90.0f, 90.0f);
    }

    private void HandlePlayerRotation() {
        // Rotate the player on the Y-axis without any smoothing for more responsiveness to the movement controls.
        playerTransform.Rotate(Vector3.up * horizontalInput);
    }

    private void HandleCameraRotation() {
        // Create the target rotation using pitch and player yaw.
        targetRotation = Quaternion.Euler(xRotation, playerTransform.eulerAngles.y, 0.0f);

        // Linearly interpolates the camera rotation.
        playerCamera.transform.localRotation = Quaternion.Lerp(
            playerCamera.transform.localRotation, targetRotation, smoothTime / Time.deltaTime);
    }

    private void HandleCameraPosition() {
        float cameraFollowSpeed = 20.0f;

        // Smoothly move the camera to follow the player with the position offset.
        Vector3 targetPosition = playerTransform.position + positionOffset;
        playerCamera.transform.position = Vector3.Lerp(playerCamera.transform.position, targetPosition, cameraFollowSpeed * Time.deltaTime);
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
