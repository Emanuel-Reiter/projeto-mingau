using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField] private float _interactionRange = 1.0f;
    [SerializeField] private LayerMask _interactionLayer;

    private PlayerDependencies _deps;

    public delegate void OnInteractionAvailableChangedDelegate(bool interactionAvailable);
    public event OnInteractionAvailableChangedDelegate OnInteractionAvailableChanged;
    private bool _interactionAvailable = false;
    public bool InteractionAvailable
    {
        get => _interactionAvailable;
        set
        {
            if (_interactionAvailable == value) return;

            _interactionAvailable = value;
            OnInteractionAvailableChanged?.Invoke(InteractionAvailable);
        }
    }

    private void Start()
    {
        _deps = GetComponent<PlayerDependencies>();
    }

    private void Update()
    {
        SearchInteractables();
    }

    private void SearchInteractables()
    {
        Vector3 origin = transform.position + (Vector3.up * _interactionRange);
        Vector3 direction = Vector3.zero;

        Collider[] objectsInRange = Physics.OverlapSphere(origin, _interactionRange, _interactionLayer);

        if (objectsInRange.Length > 0)
        {
            BaseInteractable interactable = objectsInRange[0].GetComponent<BaseInteractable>();

            if (interactable == null) return;
            if (interactable.HasBeenInteracted) return;

            InteractionAvailable = true;

            if (_deps.Input.InteractPressed) interactable.Interact();
        }
        else
        {
            InteractionAvailable = false;
        }
    }
}
