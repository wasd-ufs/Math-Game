using UnityEngine;

public class BaseItem :  MonoBehaviour
{   
    [SerializeField] private string _name;
    [SerializeField] private Sprite _sprite;
    [SerializeField] private string _description;
    
    public string GetName() => _name;
    public Sprite GetSprite() => _sprite;
    public string GetDescription()  => _description;
}