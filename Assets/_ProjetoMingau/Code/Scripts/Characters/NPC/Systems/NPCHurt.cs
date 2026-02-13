using DG.Tweening;
using UnityEngine;

public class NPCHurt : MonoBehaviour
{
    private NPCDependencies _deps;

    [SerializeField] private AudioSfxDef _hitAudio;
    [SerializeField] private ParticleSystem _hitVFX;
    [SerializeField] private GameObject _armature;

    private void Start()
    {
        _deps = GetComponent<NPCDependencies>();

        _deps.Attributes.OnTakeDamage += Hurt;
    }

    private void OnDisable()
    {
        _deps.Attributes.OnTakeDamage -= Hurt;
    }

    private void OnDestroy()
    {
        _deps.Attributes.OnTakeDamage -= Hurt;

    }

    private void Hurt()
    {
        PlayHurtSFX();
        PlayHurtVFX();
        PlayHurtAnim();
    }

    private void PlayHurtAnim()
    {
        ResetUITransforms();

        _armature.transform.DOScale(1.1f, 0.1f)
            .SetEase(Ease.InOutSine)
            .SetLoops(2, LoopType.Yoyo);


        _armature.transform.DOLocalRotate(new Vector3(-15f, 0f, 0f), 0.11f)
            .SetEase(Ease.InOutSine)
            .SetLoops(2, LoopType.Yoyo)
            .OnComplete(() =>
            {
                ResetUITransforms();
                _armature.transform.DOKill();
            });
    }

    private void ResetUITransforms()
    {
        _armature.transform.localScale = Vector3.one;
        _armature.transform.localRotation = Quaternion.identity;
    }

    private void PlayHurtSFX()
    {
        if (_hitAudio == null) return;

        Vector3 audioPos = transform.position + Vector3.up;
        AudioSfxDef audio = Instantiate(_hitAudio);
        audio.Pitch = Random.Range(0.9f, 1.1f);
        AudioPool.Play(audio, audioPos);
    }

    private void PlayHurtVFX()
    {
        if (_hitVFX == null) return;

        Vector3 rot = new Vector3(0f, Random.Range(0f, 360f), 0f);

        _hitVFX.transform.rotation = Quaternion.Euler(rot);
        _hitVFX.Play();
    }
}
