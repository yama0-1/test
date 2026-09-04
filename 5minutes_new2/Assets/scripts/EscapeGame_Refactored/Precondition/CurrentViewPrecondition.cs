using EscapeGame.Interaction;
using EscapeGame.Viewport;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName = "EscapeGame/Preconditions/Visible From")]

public sealed class CurrentViewPrecondition : InteractionPrecondition
{
    [SerializeField] private ViewpointSO[] _visibleFrom;

    public override bool Evaluate(in InteractionContext ctx)
    {

        foreach (ViewpointSO viewpoint in _visibleFrom)
        {
            if (viewpoint == ctx.CurrentView) return true;
        }
        return false;

    }
}