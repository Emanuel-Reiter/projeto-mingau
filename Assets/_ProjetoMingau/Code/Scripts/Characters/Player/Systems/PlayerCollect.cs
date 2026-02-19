using UnityEngine;

public class PlayerCollect : MonoBehaviour
{
    private PlayerDependencies _deps;

    [Header("Params")]
    [SerializeField] private float _collectRadius = 0.667f;
    [SerializeField] private LayerMask _colletablesLayer;

    [Header("SFX params")]
    [SerializeField] private AudioSfxDef _collectSFX;

    private void Start()
    {
        _deps = GetComponent<PlayerDependencies>();
    }

    private void Update()
    {
        SerchItems();
    }

    private void SerchItems()
    {
        Vector3 origin = transform.position + (Vector3.up * _collectRadius);
        Vector3 direction = Vector3.zero;

        Collider[] objectsInRange = Physics.OverlapSphere(origin, _collectRadius, _colletablesLayer);

        foreach (Collider obj in objectsInRange)
        {
            BaseCollectable collectable = obj.gameObject.GetComponent<BaseCollectable>();

            if(collectable == null) continue;

            collectable.Collect();
            _deps.Inventory.Collectables += collectable.Value;

            // Temp sfx implementation
            // TODO: Refator
            PlayCollectSFX();
        }
    }

    float collectPitch = 0f;
    int collectPitchTimer;
    private void PlayCollectSFX()
    {
        if (_collectSFX == null) return;

        GlobalTimer.I.CancelTimer(collectPitchTimer);

        Vector3 audioPos = transform.position + Vector3.up;
        AudioSfxDef audio = Instantiate(_collectSFX);
        audio.Pitch = 1f + Mathf.Clamp(collectPitch, 0f, 1f);
        AudioPool.Play(audio, audioPos);

        collectPitch += 0.1f;

        collectPitchTimer = GlobalTimer.I.StartTimer(1f, () => { ResetCollectPitch(); });
    }

    private void ResetCollectPitch()
    {
        collectPitch = 0;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if(!Application.isPlaying) return;

        Vector3 origin = transform.position + (Vector3.up * _collectRadius);
        Gizmos.DrawWireSphere(origin, _collectRadius);
    }
#endif
}
