using EscapeGame.SaveSystem;
using EscapeGame.Viewport;
using System;
using UnityEngine;

namespace EscapeGame.Core
{
    public sealed class Interactable : MonoBehaviour
    {
        [SerializeField] private InteractionPrecondition[] _conditions = Array.Empty<InteractionPrecondition>();
        [SerializeField] private InteractEffect[] _effects; 
    
        public bool TryInteract(in InteractionContext context)
        {
            foreach (var condition in _conditions)
            {
                if (condition != null && !condition.Evaluate(context))
                    return false;
            }
    
            foreach (InteractEffect effect in _effects) effect.Execute(context);
            return true;
        }
    }
    
    public readonly struct InteractionContext
    {
        public readonly ViewpointSO CurrentView;
        public readonly ItemDefinition SelectedItem;
    
        public InteractionContext(ViewpointSO currentView, ItemDefinition selectedItem)
        {
            CurrentView = currentView;
            SelectedItem = selectedItem;
        }
    }
}
