using System;
using UnityEngine;

public class LogarithmicItem : IFunctionItem
{   
    [SerializeField] private NumberItem _result;
    [SerializeField] private NumberItem _bases;
    [SerializeField] private NumberItem _logarithm;
    [SerializeField] private Sprite _sprite;
    [SerializeField] private AnimationClip _animation;
    [SerializeField] private string _description;
    
    public string GetName() => "Logarithmic Function";
    
    public Sprite GetSprite() => _sprite;

    public AnimationClip GetAnimation() => _animation;
    
    public NumberItem GetFixedParameter() => _bases;

    public string GetDescription() => _description;

    public void PutParameter(NumberItem number) => _logarithm = number;

    public NumberItem TakeParameter()
    {
        var temp = _logarithm;
        _logarithm = null;
        return temp;
    }

    public NumberItem GetResult()
    {
        _result.SetValue((uint)Math.Log(_logarithm.GetValue(),_bases.GetValue()));
        return _result;
    }
}
