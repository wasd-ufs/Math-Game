using System;
using UnityEngine;

public class ExponentialItem : IFunctionItem
{
    [SerializeField] private NumberItem _result;
    [SerializeField] private NumberItem _bases;
    [SerializeField] private NumberItem _exponent;
    [SerializeField] private Sprite _sprite;
    [SerializeField] private AnimationClip _animation;
    [SerializeField] private string _description;
    
    public string GetName() => "Exponential Function";
    
    public Sprite GetSprite() => _sprite;
    
    public AnimationClip GetAnimation() => _animation;
    
    public NumberItem GetFixedParameter() => _bases;
    
    public string GetDescription() => _description;

    public void PutParameter(NumberItem number) => _exponent = number;

    public NumberItem TakeParameter()
    {
        var temp = _exponent;
        _exponent = null;
        return temp;
    }

    public NumberItem GetResult()
    {
        _result.SetValue((uint)Math.Pow(_bases.GetValue(), _exponent.GetValue()));
        return _result;
    }
}
