using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractableObject : MonoBehaviour, IInteractable
{
    [SerializeField] private Sprite _sprite;
    [SerializeField] private Collider2D _interactableCollider;
    [SerializeField] private List<UnityEvent> _onInteract;
    
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void Interact()
    {
        foreach (var unityEvent  in _onInteract)
            unityEvent.Invoke();
    }

    public bool CanInteract()
    {
        throw new System.NotImplementedException();
    }
}
