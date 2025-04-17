using UnityEngine;

public class PlayerAttributes : MonoBehaviour {
    // Movement related attributes
    private float runSpeed = 8.0f;

    public float currentSpeed { get; private set; }

    [HideInInspector] public float rotationInterpolationTime = 0.05f;
    [HideInInspector] public float rotationInterpolationlocity;
    //private float speedInterpolationTime = 0.25f;

    // Gravity attributes
    public float gravityAcceleration = -9.807f;

    private void Start () {
        currentSpeed = runSpeed;
    }
}
