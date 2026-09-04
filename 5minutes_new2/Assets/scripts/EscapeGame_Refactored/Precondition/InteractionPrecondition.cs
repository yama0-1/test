using UnityEngine;
using EscapeGame.SaveSystem;

namespace EscapeGame.Interaction
{
    /// <summary>
    /// 相互作用の前提条件を表す基底。ScriptableObject なのでアセット化でき、
    /// 複数種類を同じ配列にまとめて評価できる (多態)。
    /// 新しい条件種別は、このクラスを継承したアセットを増やすだけ (Open/Closed)。
    /// </summary>
    public abstract class InteractionPrecondition : ScriptableObject
    {
        public abstract bool Evaluate(in InteractionContext context);
    }
}
