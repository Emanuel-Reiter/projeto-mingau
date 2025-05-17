using UnityEngine;

public class PlayerGeneralGroundDetector : MonoBehaviour {

    // Ground detection
    [SerializeField] private LayerMask lowHitCollisionMask;
    public bool isGrounded { get; private set; } = false;

    public void OnCollisionEnter(Collision collision) {
        if (collision != null) {
            if (collision.gameObject.layer == lowHitCollisionMask) {
                isGrounded = true;
            }
        }
    }

    public void OnCollisionExit(Collision collision) {
        if (collision != null) {
            if (collision.gameObject.layer == lowHitCollisionMask) {
                isGrounded = false;
            }
        }
    }
}
