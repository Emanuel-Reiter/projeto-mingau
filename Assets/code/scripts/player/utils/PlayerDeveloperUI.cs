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

        GUI.Label(new Rect(32, 32, 512, 32), $"fps: {_framerate}");

        GUI.Label(new Rect(32, 64, 512, 32), $"state: {_stateManager.CurrentState}");

        GUI.Label(new Rect(32, 96, 512, 32), $"velocity: {_locomotion.HorizontalVelocity.magnitude.ToString("F2")}");

        GUI.Label(new Rect(32, 128, 512, 32), $"airJumpsRemaining: {_dependencies.Jump.CurrentAirJumpsCount}");
        GUI.Label(new Rect(32, 160, 512, 32), $"dashRemaining: {_dependencies.Dash.CurrentDashCount}");

        GUI.Label(new Rect(32, 192, 512, 32), $"groundSnapping: {_physics.UseGroundSnapping}");
        GUI.Label(new Rect(32, 224, 512, 32), $"isGrounded: {_physics.IsGrounded}");
        GUI.Label(new Rect(32, 256, 512, 32), $"onSteepSlope: {_physics.GetOnSteepSlope()}");

        float slopeAngle = Vector3.Angle(_physics.GroundNormal, Vector3.up);
        slopeAngle = Mathf.CeilToInt(slopeAngle);
        GUI.Label(new Rect(32, 286, 512, 32), $"slopeAngle: {slopeAngle}");
        GUI.Label(new Rect(32, 318, 512, 32), $"standingOnEntity: {_physics.StandingOnEntity}");
    }
}
