using UnityEngine;

/// <summary>
/// enum para definir qual o valor numérico do número
/// </summary>
public enum NumberValue
{
    Zero,
    One,
    Two,
    Three,
    Four,
    Five,
    Six,
    Seven,
    Eight,
    Nine
}

/// <summary>
/// classe que representa números
/// </summary>
public class NumberItem : BaseItem
{
    [SerializeField] private NumberValue _value;

    public int GetValue() => (int)_value;
}
