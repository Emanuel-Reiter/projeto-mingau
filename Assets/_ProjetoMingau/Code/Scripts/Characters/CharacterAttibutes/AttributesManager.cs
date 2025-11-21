using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    [Header("Posture params")]
    [SerializeField] private bool _enablePostureDamage = true;
    [SerializeField] private int _maxPosture = 1;
    private int _currentPosture;
    public bool IsPostureBroken = false;

    [Header("Other")]
    [SerializeField] private ParticleSystem _dieVFX;

    private void Start()
    {
        _currentHP = _maxHP;
        _currentPosture = _maxPosture;

        // Checks if the attributes manager is owned by the player
        _isPlayer = gameObject.CompareTag("Player") ? true : false;
    }

    public void TakeDamage(int amount)
    {
        _currentHP = Mathf.Clamp(_currentHP - amount, 0, _maxHP);

        if (_isPlayer) Debug.Log($"Took {amount} of damage!");

        CheckIsDead();
        UpdateHealthUI();
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
        _currentHP = Mathf.Clamp((_currentHP + amount), 0, _maxHP);
    }

    public void CheckIsDead()
    {
        bool hasDied= _currentHP <= 0;

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
    }

    private IEnumerator DieCoroutine()
    {
        if (_isPlayer)
        {
            try
            {
                Scene scene = SceneManager.GetActiveScene();
                if (scene != null) LevelLoader.Instance.LoadLevel(scene.name, () => Revive());
            }
            catch
            {
                Debug.LogError("Error during scene loading!");
            }
        }

        yield return null;

        if (_dieVFX != null)
        {
            ParticleSystem vfx = Instantiate(_dieVFX, transform.position, Quaternion.identity, null);
            vfx.Play();
            Destroy(vfx.gameObject, _dieVFX.main.duration);
        }

        if (!_isPlayer) gameObject.SetActive(false);
    }

    private void UpdateHealthUI()
    {
        return;
    }
}
