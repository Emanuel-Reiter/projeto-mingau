using NUnit.Framework;
using System.Collections;
using UnityEngine;

public class AttributesManager : MonoBehaviour
{
    private bool _isDead = false;
    public bool IsDead => _isDead;

    [Header("Damage params")]
    [SerializeField] private int _damage = 1;
    public int Damage => _damage;

    [Header("HP params")]
    [SerializeField] private int _maxHP = 5;
    private int _currentHP;

    [Header("Posture params")]
    [SerializeField] private bool _enablePostureDamage = true;
    [SerializeField] private int _maxPosture = 1;
    private int _currentPosture;
    public bool IsPostureBroken = false;

    private void Start()
    {
        _currentHP = _maxHP;
        _currentPosture = _maxPosture;
    }

    public void TakeDamage(int amount)
    {
        Debug.Log("Took damage");
        _currentHP = Mathf.Clamp(_currentHP - amount, 0, _maxHP);

        CheckIsDead();
        UpdateHealthUI();
        DamagePosture();
    }

    private void DamagePosture()
    {
        if (!_enablePostureDamage) return;

        _currentPosture--;
        if(_currentPosture <= 0) IsPostureBroken = true;
    }

    public void ResetPosture() 
    { 
        _currentPosture = _maxPosture;
        IsPostureBroken = false;
    }

    public void Heal(int amount)
    {
        _currentHP = Mathf.Clamp((_currentHP + amount), 0, _maxHP);
    }

    public void CheckIsDead()
    {
        bool isDead = _currentHP <= 0;

        if (isDead) Die();
        _isDead = isDead;
    }

    public void Die()
    {
        if(!_isDead) return;
        StartCoroutine(DieCoroutine());
    }

    private IEnumerator DieCoroutine()
    {
        yield return null;
        gameObject.SetActive(false);
    }

    private void UpdateHealthUI()
    {
        return;
    }
}
