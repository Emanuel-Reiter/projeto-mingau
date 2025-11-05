using System;
using System.Collections.Generic;
using System.Linq;

public class BaseAttribute
{
    // Core values
    private float _baseValue;
    private float _currentValue;
    private float _maxValue;

    // Modifier lists
    private List<float> _flatModifiers = new List<float>();
    private List<float> _percentModifiers = new List<float>();

    // Events for value changes
    public event Action<float> OnBaseValueChanged;
    public event Action<float> OnCurrentValueChanged;
    public event Action<float> OnMaxValueChanged;

    public float BaseValue
    {
        get => _baseValue;
        set
        {
            if (_baseValue != value)
            {
                _baseValue = value;
                RecalculateMaxValue();
                OnBaseValueChanged?.Invoke(_baseValue);
            }
        }
    }

    public float CurrentValue
    {
        get => _currentValue;
        set
        {
            float newValue = Math.Clamp(value, 0f, MaxValue);
            if (_currentValue != newValue)
            {
                float oldValue = _currentValue;
                _currentValue = newValue;
                OnCurrentValueChanged?.Invoke(_currentValue);
            }
        }
    }

    public float MaxValue
    {
        get => _maxValue;
        private set
        {
            if (_maxValue != value)
            {
                float oldMax = _maxValue;
                _maxValue = value;

                // Clamp current value to new max
                CurrentValue = _currentValue;

                OnMaxValueChanged?.Invoke(_maxValue);
            }
        }
    }

    public BaseAttribute(float baseValue)
    {
        _baseValue = baseValue;
        RecalculateMaxValue();
        _currentValue = _maxValue;
    }

    public void AddFlatModifier(float modifier)
    {
        _flatModifiers.Add(modifier);
        RecalculateMaxValue();
    }

    public void RemoveFlatModifier(float modifier)
    {
        _flatModifiers.Remove(modifier);
        RecalculateMaxValue();
    }

    public void AddPercentModifier(float modifier)
    {
        _percentModifiers.Add(modifier);
        RecalculateMaxValue();
    }

    public void RemovePercentModifier(float modifier)
    {
        _percentModifiers.Remove(modifier);
        RecalculateMaxValue();
    }

    private void RecalculateMaxValue()
    {
        float flatTotal = _flatModifiers.Sum();
        float percentTotal = _percentModifiers.Sum();

        float newMaxValue = (_baseValue + flatTotal) * (1 + percentTotal / 100f);
        MaxValue = newMaxValue;
    }

    // Helper methods for common operations
    public void IncreaseCurrentValue(float amount)
    {
        CurrentValue += amount;
    }

    public void DecreaseCurrentValue(float amount)
    {
        CurrentValue -= amount;
    }

    public void ResetToMaxValue()
    {
        CurrentValue = MaxValue;
    }

    public void ResetToBaseValue()
    {
        BaseValue = _baseValue;
        _flatModifiers.Clear();
        _percentModifiers.Clear();
        RecalculateMaxValue();
        CurrentValue = MaxValue;
    }
}