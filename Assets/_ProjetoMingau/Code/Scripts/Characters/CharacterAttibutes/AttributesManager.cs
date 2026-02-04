using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class AttributesManager : MonoBehaviour
{
    private bool _isAlive = true;
    public bool IsAlive => _isAlive;

    private bool _isPlayer = false;

    [Header("Damage params")]
    [SerializeField] private int _damage = 1;
    public int Damage => _damage;

    [Header("HP params")]
    [SerializeField] private int _maxHP = 5;
    private int _currentHP;

    public int CurrentHP => _currentHP;


    [Header("Posture params")]
    [SerializeField] private bool _enablePostureDamage = true;
    [SerializeField] private int _maxPosture = 1;
    private int _currentPosture;
    public bool IsPostureBroken = false;

    [Header("UI params")]
    [SerializeField] private GameObject _heartPrefab;

    [SerializeField] private Sprite _heartFullSprite;
    [SerializeField] private Sprite _heartEmptySprite;

    [Header("Other params")]
    [SerializeField] private ParticleSystem _dieVFX;

    private void Start()
    {
        // Checks if the attributes manager is owned by the player
        _isPlayer = gameObject.CompareTag("Player") ? true : false;

        _currentHP = _maxHP;
        _currentPosture = _maxPosture;
    }

    public void TakeDamage(int amount)
    {
        _currentHP = Mathf.Clamp(CurrentHP - amount, 0, _maxHP);

        CheckIsDead();
        DamagePosture();
    }

    private void DamagePosture()
    {
        if (!_enablePostureDamage) return;

        _currentPosture--;
        if (_currentPosture <= 0) IsPostureBroken = true;
    }

    public void ResetPosture()
    {
        _currentPosture = _maxPosture;
        IsPostureBroken = false;
    }

    public void Heal(int amount)
    {
        _currentHP = Mathf.Clamp((CurrentHP + amount), 0, _maxHP);
    }

    public void CheckIsDead()
    {
        bool hasDied = CurrentHP <= 0;

        _isAlive = !hasDied;

        if (hasDied) Die();
    }

    private void Die()
    {
        if (_isAlive) return;
        StartCoroutine(DieCoroutine());
    }

    public void Revive()
    {
        _isAlive = true;
        Heal(99999);
    }

    private IEnumerator DieCoroutine()
    {
        yield return null;
    }
}
