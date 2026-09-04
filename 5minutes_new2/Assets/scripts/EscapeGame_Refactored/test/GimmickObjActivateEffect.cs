using EscapeGame.Inventory;
using EscapeGame.SaveSystem;
using UnityEngine;

public sealed class GimmickObjActivateEffect : InteractEffect
{
    [SerializeField] private GimmickValuePrecondition _triggerCondition;
    [SerializeField] private GameObject _obj;
    [SerializeField] private bool isActiveObj = true;

    private void Start()
    {
        Execute(default);
    }

    public override void Execute(in InteractionContext context)
    {
        if (_triggerCondition.Evaluate(default))
        {
            _obj.SetActive(isActiveObj);
        }

    }
}