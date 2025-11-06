using UnityEngine;

public abstract class BaseCollectable : MonoBehaviour
{
    [Header("Params")]
    [SerializeField] private int _value = 1;
    public int Value => _value;

    public abstract void Collect();
}
