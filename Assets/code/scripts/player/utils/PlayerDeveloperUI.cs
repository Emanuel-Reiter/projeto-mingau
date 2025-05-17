using UnityEngine;

public class PlayerDeveloperUI : MonoBehaviour {

    // Exteral player references
    private PlayerAttributes playerAttributes;
    private PlayerStateManager playerStateManager;
    private PlayerMovement playerMovement;

    // Framerate calculation
    private int lastFrameIndex;
    private float[] frameDeltaTimeArray;
    private int framerate;


    private void Awake() {
        frameDeltaTimeArray = new float[64];
    }

    private void Start() {
        InitializeReferences();
    }

    private void Update() {
        DisplayFrametate();
    }

    private void DisplayFrametate() {
        frameDeltaTimeArray[lastFrameIndex] = Time.unscaledDeltaTime;
        lastFrameIndex = (lastFrameIndex + 1) % frameDeltaTimeArray.Length;

        framerate = Mathf.RoundToInt(CalculateAverageFramerate());
    }

    private float CalculateAverageFramerate() {
        float total = 0.0f;

        foreach (float deltaTime in frameDeltaTimeArray) {
            total += deltaTime;
        }

        return frameDeltaTimeArray.Length / total;
    }

    private void InitializeReferences() {
        try {
            // Object references
            playerAttributes = GetComponent<PlayerAttributes>();
            playerMovement = GetComponent<PlayerMovement>();
            playerStateManager = GetComponent<PlayerStateManager>();
        }
        catch {
            Debug.LogError("Some references were not assigned correctly.\nCheck external tag names and components assigned to this object.");
        }
    }

    private void OnGUI() {
        GUI.skin.label.fontSize = 24;

        // Framerate UI
        GUI.Label(new Rect(32, 32, 512, 32), $"FPS: {framerate}");

        // Player state
        GUI.Label(new Rect(32, 64, 512, 32), $"currentState: {playerStateManager.currentState}");

        // Current player horizontal velocity
        GUI.Label(new Rect(32, 96, 512, 32), $"currentVelocity: {playerMovement.horizontalVelocity.magnitude.ToString("F2")}");

        // Current player air jumps left
        GUI.Label(new Rect(32, 128, 512, 32), $"airJumpsLeft: {playerAttributes.GetAirJumpsLeft()}");
    }
}
