using System.Collections;
using UnityEngine;

public class AttributesManager : MonoBehaviour
{
    private bool _isPlayer = false;
    
    private bool _isAlive = true;
    public bool IsAlive
    {
        get => _isAlive;
        set
        {
            if (value == _isAlive) return;
            _isAlive = value;
        }
    }

    [Header("Damage params")]
    [SerializeField] private int _damage = 1;
    public int Damage => _damage;

    [Header("HP params")]
    [SerializeField] private int _maxHP = 5;
    private int _currentHP;

    public int CurrentHP
    {
        get => _currentHP;
        set
        {
            if (value == _currentHP) return;
            _currentHP = Mathf.Clamp(value, 0, _maxHP);

            CheckIsAlive();
            DamagePosture();
        }
    }

    public delegate void OnTakeDamageDelegate();
    public event OnTakeDamageDelegate OnTakeDamage;


    [Header("Posture params")]
    [SerializeField] private bool _enablePostureDamage = true;
    [SerializeField] private int _maxPosture = 1;
    private int _currentPosture;
    public int CurrentPosture
    {
        get => _currentPosture;
        set
        {
            if(value == _currentPosture) return;
            _currentPosture = Mathf.Clamp(value, 0, _maxPosture);
        }
    }

    private bool _isPostureBroken = false;
    public bool IsPostureBroken
    {
        get => _isPostureBroken;
        set 
        {
            if( value == _isPostureBroken) return;
            _isPostureBroken = value;
        }
    }

    private void Start()
    {
        _isPlayer = gameObject.CompareTag("Player") ? true : false;

        CurrentHP = _maxHP;
        CurrentPosture = _maxPosture;
    }

    #region HP
    public void TakeDamage(int amount) { CurrentHP -= amount; OnTakeDamage?.Invoke(); }
    public void Heal(int amount) { CurrentHP += amount; }
    
    public void CheckIsAlive()
    {
        IsAlive = CurrentHP > 0;
        if (!IsAlive) Die();
    }

    private void Die() { StartCoroutine(DieCoroutine()); }

    public void Revive() { Heal(99999); }
    #endregion

    #region Posture
    private void DamagePosture()
    {
        if (!_enablePostureDamage) return;

        CurrentPosture--;
        if (CurrentPosture <= 0) IsPostureBroken = true;
    }

    public void ResetPosture()
    {
        CurrentPosture = _maxPosture;
        IsPostureBroken = false;
    }
    #endregion

    private IEnumerator DieCoroutine()
    {
        if (!_isPlayer) yield break;

        _ = LevelManager.I.LoadLevel(LevelManager.I.CurrentLoadedLevel);
        Revive();
    }
}
