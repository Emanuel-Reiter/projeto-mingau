using UnityEngine;

public class FollowBone : MonoBehaviour {

    [SerializeField] private Transform targetBone;

    private void LateUpdate() {
        this.transform.position = targetBone.position;
        this.transform.rotation = targetBone.rotation;
    }
}
