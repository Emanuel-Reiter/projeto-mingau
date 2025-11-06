using TMPro;
using UnityEditor.PackageManager;
using UnityEngine;

public class PlayerInventorySystem : MonoBehaviour
{
    private int _collectables = 0;

    [Header("Params")]
    [SerializeField] private float _collectRadius = 0.667f;
    [SerializeField] private LayerMask _colletablesLayer;

    [SerializeField] private TMP_Text _collectablesCountText;

    private void Start()
    {
        UpdateCollectablesUI();
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
            collectable.Collect();
            _collectables += collectable.Value;
            UpdateCollectablesUI();
        }

    }

    private void UpdateCollectablesUI()
    {
        if (_collectablesCountText != null) _collectablesCountText.text = $"x {_collectables}";
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
