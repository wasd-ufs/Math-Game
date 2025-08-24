using UnityEngine;

/// <summary>
/// classe que representa números
/// </summary>
public class NumberItem : IItem
{
    [SerializeField] private uint _value; // apenas valores positivos
    [SerializeField] private Sprite _sprite;
    [SerializeField] private AnimationClip _animation;
    [SerializeField] private string _description;

    public uint GetValue() => _value;
    public void SetValue(uint value) => _value = value;
    public string GetName() => "Number";
    public AnimationClip GetAnimation() => _animation;
    public Sprite GetSprite() => _sprite;
    public string GetDescription() => _description;
}
