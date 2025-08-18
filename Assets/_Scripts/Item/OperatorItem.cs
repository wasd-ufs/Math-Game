using System;
using UnityEngine;

/// <summary>
/// enum para definir o tipo da operação binária 
/// </summary>
public enum OperatorType
{
    Plus,
    Minus,
    Multiply,
    Divide
}

/// <summary>
/// classe que representa itens que são operadores binários
/// </summary>
public class OperatorItem : IItem
{
    [SerializeField] private OperatorType _type;
    [SerializeField] private Sprite _sprite;
    [SerializeField] private AnimationClip _animation;
    [SerializeField] private string _description;
    
    public OperatorType GetOperatorType() => _type;

    public Func<uint, uint, uint> GetFunction()
    {
        switch (_type)
        {
            case OperatorType.Plus:
                return (a,b) => a + b;
            case OperatorType.Minus:
                return (a,b) => a - b;
            case OperatorType.Multiply:
                return (a,b) => a * b;
            case OperatorType.Divide:
                return (a,b) => a / b;
            default:
                return (a,b) => 0;
        }
    }
    public string GetName() => _type.ToString() + " Operator";
    public Sprite GetSprite() => _sprite;
    public AnimationClip GetAnimation() => _animation;
    public string GetDescription() =>  _description;
    
}
