using UnityEngine;

public class PlayerDeveloperUI : MonoBehaviour
{

    // Exteral player references
    private PlayerDependencies _dependencies;
    private PlayerStateManager _stateManager;
    private PlayerLocomotion _locomotion;

    // Framerate calculation
    private int _lastFrameIndex;
    private float[] _frameDeltaTimeArray;
    private int _framerate;

    private bool _isCursorLocked = false;
    public bool IsCursorLocked => _isCursorLocked;

    private void Awake()
    {
        _frameDeltaTimeArray = new float[512];
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 0;
    }

    private void Start()
    {
        _dependencies = GetComponent<PlayerDependencies>();
        _stateManager = GetComponent<PlayerStateManager>();
        _locomotion = GetComponent<PlayerLocomotion>();

        ToggleCursorLock();
    }

    private void Update()
    {
        DisplayFrametate();
    }

    private void ToggleCursorLock()
    {
        if (!_isCursorLocked)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        _isCursorLocked = !_isCursorLocked;
    }

    private void DisplayFrametate()
    {
        _frameDeltaTimeArray[_lastFrameIndex] = Time.unscaledDeltaTime;
        _lastFrameIndex = (_lastFrameIndex + 1) % _frameDeltaTimeArray.Length;

        _framerate = Mathf.RoundToInt(CalculateAverageFramerate());
    }

    private float CalculateAverageFramerate()
    {
        float total = 0.0f;

        foreach (float deltaTime in _frameDeltaTimeArray)
        {
            total += deltaTime;
        }

        return _frameDeltaTimeArray.Length / total;
    }

    private int GUIPositionY(int row, int height)
    {
        return row * height;
    }

    private void OnGUI()
    {
        int xOffset = 32;
        int width = 512;
        int height = 32;

        GUI.skin.label.fontSize = 24;

        GUI.Label(new Rect(xOffset, GUIPositionY(1, height), width, height), $"fps: {_framerate}");

        GUI.Label(new Rect(xOffset, GUIPositionY(2, height), width, height), $"curState: {_stateManager.CurrentState}");

        GUI.Label(new Rect(xOffset, GUIPositionY(3, height), width, height), $"isGrounded: {_locomotion.IsGrounded}");
        GUI.Label(new Rect(xOffset, GUIPositionY(4, height), width, height), $"onSlope: {_locomotion.OnSteepSlope}");
        GUI.Label(new Rect(xOffset, GUIPositionY(5, height), width, height), $"groundAngle: {_locomotion.GroundAngle}");

        Vector3 verticalVelcity = new Vector3(0.0f, _locomotion.VerticalVelocity, 0.0f);
        Vector3 targetVelocity = _locomotion.HorizontalVelocity + verticalVelcity;
        GUI.Label(new Rect(xOffset, GUIPositionY(6, height), width, height), $"targetVel: {targetVelocity}");
        GUI.Label(new Rect(xOffset, GUIPositionY(7, height), width, height), $"horizontalVel: {_locomotion.HorizontalVelocity}");
        GUI.Label(new Rect(xOffset, GUIPositionY(8, height), width, height), $"verticalVel: {_locomotion.VerticalVelocity}");
        GUI.Label(new Rect(xOffset, GUIPositionY(9, height), width, height), $"slopeVel: {_locomotion.SlopeVelocity}");

        GUI.Label(new Rect(xOffset, GUIPositionY(10, height), width, height), $"isJumping: {_dependencies.Jump.IsJumping}");
    }
}
