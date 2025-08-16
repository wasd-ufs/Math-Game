using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// enum para definir o tipo da funcao
/// </summary>
public enum FunctionType
{
    Exponential,
    Logarithmic,
    Polynomial,
    Root
}

public class FunctionItem : BaseItem
{
    [SerializeField] private FunctionType _functionType;
    [SerializeField] private int _inputCount;
    [SerializeField] private List<int> _inputs;
    
    
    public FunctionType GetFunctionType => _functionType;

    public void SetFunctionType(FunctionType functionType)
    {
        _functionType = functionType;
        //mudar sprite
    }
}
