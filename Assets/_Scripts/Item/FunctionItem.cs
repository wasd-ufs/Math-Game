using System.Collections.Generic;
using UnityEngine;

public interface FunctionItem : IItem
{
    void PutParameter(NumberItem number);
    NumberItem TakeParameter();
    NumberItem GetFixedParameter();
    
    NumberItem GetResult();
}
