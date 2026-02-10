using UnityEngine;

public class NPCDieState : NPCBaseState
{
    [Header("State params")]
    [SerializeField] private AnimationClip _dieAnim;
    [SerializeField] private float _interpolationTime = 0.05f;
    [SerializeField] private ParticleSystem _dieVFX;
    [SerializeField] private AudioSfxDef _dieAudio;

    public override void CheckExitState(NPCStateManager npc)
    {

    }

    public override void EnterState(NPCStateManager npc)
    {
        npc.Deps.Animation.PlayAnimationInterpolated(_dieAnim, _interpolationTime);

        if (_dieAudio != null)
        {
            Vector3 audioPos = transform.position + Vector3.up;
            AudioPool.Play(_dieAudio, audioPos);
        }

        if(_dieVFX  != null)
        {
            _dieVFX.gameObject.SetActive(true);
            _dieVFX.transform.parent = null;
            _dieVFX.Play();
            Invoke("DisableVFX", _dieVFX.main.duration);
        }

        // Disables the root NPC parent
        npc.Deps.Attributes.gameObject.SetActive(false);
    }

    private void DisableVFX()
    {
        if (_dieVFX == null) return;

        _dieVFX.gameObject.SetActive(false);
    }

    public override void ExitState(NPCStateManager npc)
    {

    }

    public override void PhysicsUpdateState(NPCStateManager npc)
    {

    }

    public override void UpdateState(NPCStateManager npc)
    {

    }
}
