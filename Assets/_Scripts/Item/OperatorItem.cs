using UnityEngine;

/// <summary>
/// enum para definir o tipo da operação binária 
/// </summary>
public enum OperatorType
{
    Plus,
    Minus,
    Multiply,
    Divide,
    Min,
    Max,
}

/// <summary>
/// classe que representa itens que são operadores binários
/// </summary>
public class OperatorItem : BaseItem
{
    [SerializeField] private OperatorType _type;
    
    
    public OperatorType GetOperatorType() => _type;
}
