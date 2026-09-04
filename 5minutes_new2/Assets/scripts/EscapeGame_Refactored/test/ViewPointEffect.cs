using EscapeGame.Viewport;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewPointEffect : InteractEffect
{
    [SerializeField] private ViewpointSO _targetViewpoint;
    [SerializeField] private ViewportRequestChannel channel;

    public override void Execute(in InteractionContext context)
    {
        channel.Raise(_targetViewpoint);
    }
}
