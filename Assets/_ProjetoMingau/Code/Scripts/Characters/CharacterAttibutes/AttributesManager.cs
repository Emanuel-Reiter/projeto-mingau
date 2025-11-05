using System.Collections;
using UnityEngine;

public class AttributesManager : MonoBehaviour
{
    private bool _isDead = false;
    public bool IsDead => _isDead;

    private BaseAttribute _health = new BaseAttribute(100.0f);

    public void TakeDamage(float amount)
    {
        _health.DecreaseCurrentValue(amount);
        CheckIsDead();
    }

    public void Heal(float amount)
    {
        _health.IncreaseCurrentValue(amount);
    }

    public void CheckIsDead()
    {
        bool isDead = _health.CurrentValue <= 0.0f;

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
}
