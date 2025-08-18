using UnityEngine;

public interface IItem
{
    string GetName();
    Sprite GetSprite();
    AnimationClip GetAnimation();
    string GetDescription();
}
