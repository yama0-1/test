using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public abstract class InteractEffect: MonoBehaviour
{
    public abstract void Execute(in InteractionContext context);

}