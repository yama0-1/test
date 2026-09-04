using EscapeGame.Interaction;
using EscapeGame.SaveSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace EscapeGame.Interaction
{
    [CreateAssetMenu(menuName = "EscapeGame/Preconditions/Selected Item", fileName = "Cond_Selected_")]
    public sealed class SelectedItemPrecondition : InteractionPrecondition
    {
        [SerializeField] private ItemDefinition _requiredItem;

        public override bool Evaluate(in InteractionContext context)
            => context.SelectedItem == _requiredItem;
    }
}