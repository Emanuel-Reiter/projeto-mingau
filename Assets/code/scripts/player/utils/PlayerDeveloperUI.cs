using UnityEngine;

public class PlayerDeveloperUI : MonoBehaviour {

    // Exteral player references
    private PlayerDependencies _dependencies;
    private PlayerStateManager _stateManager;
    private PlayerLocomotion _locomotion;
    private PlayerPhysics _physics;

    // Framerate calculation
    private int _lastFrameIndex;
    private float[] _frameDeltaTimeArray;
    private int _framerate;


    private void Awake() {
        _frameDeltaTimeArray = new float[256];
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 0;
    }

    private void Start() {
        _dependencies = GetComponent<PlayerDependencies>();
        _stateManager = GetComponent<PlayerStateManager>();
        _locomotion = GetComponent<PlayerLocomotion>();
        _physics = GetComponent<PlayerPhysics>();
    }

    private void Update() {
        DisplayFrametate();
    }

    private void DisplayFrametate() {
        _frameDeltaTimeArray[_lastFrameIndex] = Time.unscaledDeltaTime;
        _lastFrameIndex = (_lastFrameIndex + 1) % _frameDeltaTimeArray.Length;

        _framerate = Mathf.RoundToInt(CalculateAverageFramerate());
    }

    private float CalculateAverageFramerate() {
        float total = 0.0f;

        foreach (float deltaTime in _frameDeltaTimeArray) {
            total += deltaTime;
        }

        return _frameDeltaTimeArray.Length / total;
    }

    private void OnGUI() {
        GUI.skin.label.fontSize = 24;

        // Framerate UI
        GUI.Label(new Rect(32, 32, 512, 32), $"fps: {_framerate}");

        // Player state
        GUI.Label(new Rect(32, 64, 512, 32), $"state: {_stateManager.CurrentState}");

        // Current player horizontal velocity
        GUI.Label(new Rect(32, 96, 512, 32), $"velocity: {_locomotion.HorizontalVelocity.magnitude.ToString("F2")}");

        // Current player jumps left
        GUI.Label(new Rect(32, 128, 512, 32), $"jumpsRemaining: {_dependencies.Jump.CurrentJumpCount}");

        // Ground
        GUI.Label(new Rect(32, 160, 512, 32), $"groundSnaping: {_physics.UseGroundSnapping}");
        GUI.Label(new Rect(32, 192, 512, 32), $"isGrounded: {_physics.IsGrounded}");
    }
}
