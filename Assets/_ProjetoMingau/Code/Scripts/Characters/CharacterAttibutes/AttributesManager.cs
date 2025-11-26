using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    [SerializeField] private Transform _heartContainer;
    [SerializeField] private GameObject _heartPrefab;

    [SerializeField] private Sprite _heartFullSprite;
    [SerializeField] private Sprite _heartEmptySprite;

    private List<GameObject> _currentHearts = new List<GameObject>();


    [Header("Other params")]
    [SerializeField] private ParticleSystem _dieVFX;

    private void Start()
    {
        // Checks if the attributes manager is owned by the player
        _isPlayer = gameObject.CompareTag("Player") ? true : false;

        if (_isPlayer)
        {
            try
            {
                GridLayoutGroup[] gridLayoutGroups = Resources.FindObjectsOfTypeAll<GridLayoutGroup>();

                foreach (GridLayoutGroup group in gridLayoutGroups)
                {
                    if (group.CompareTag("PlayerHealthBar"))
                    {
                        _heartContainer = group.transform;
                        break;
                    }
                }
            }
            catch
            {
                Debug.LogError("Player health bar not found!");
            }
        }

        _currentHP = _maxHP;
        _currentPosture = _maxPosture;

        UpdateUI();
    }

    public void TakeDamage(int amount)
    {
        _currentHP = Mathf.Clamp(CurrentHP - amount, 0, _maxHP);

        CheckIsDead();
        DamagePosture();
        UpdateUI();
    }

    private void UpdateUI()
    {
        if (_heartContainer == null || _heartFullSprite == null || _heartEmptySprite == null) return;

        ClearHearts();

        for (int i = 0; i < _maxHP; i++)
        {
            GameObject newHeart = Instantiate(_heartPrefab, _heartContainer);
            _currentHearts.Add(newHeart);

            if (i < CurrentHP)
            {
                newHeart.GetComponent<Image>().sprite = _heartFullSprite;
            }
            else
            {
                newHeart.GetComponent<Image>().sprite = _heartEmptySprite;
            }
        }
    }

    private void ClearHearts()
    {
        foreach (GameObject heart in _currentHearts)
        {
            Destroy(heart);
        }
        _currentHearts.Clear();
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
        UpdateUI();
    }

    public void CheckIsDead()
    {
        bool hasDied= CurrentHP <= 0;

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
        UpdateUI();
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
}
