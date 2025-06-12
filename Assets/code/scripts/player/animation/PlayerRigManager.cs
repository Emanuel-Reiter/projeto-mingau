using UnityEngine;
using UnityEngine.Animations.Rigging;

public class PlayerRigManager : MonoBehaviour {
    [Header("Rig references")]
    public Rig tailRig;

    public void SetTailRigWeight(float weight) { tailRig.weight = weight; }
}
