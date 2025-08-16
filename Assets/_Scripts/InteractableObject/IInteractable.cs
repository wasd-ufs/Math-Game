using UnityEngine;

/// <summary>
/// Interface para coisas que o player pode interagir
/// </summary>
public interface IInteractable
{
    void Interact(/*Player player*/);
    bool CanInteract();
}
