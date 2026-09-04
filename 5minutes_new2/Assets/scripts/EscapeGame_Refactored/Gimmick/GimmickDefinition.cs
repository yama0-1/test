using System.Xml.Linq;
using UnityEngine;

// GimmickDefinition.cs
[CreateAssetMenu(menuName = "EscapeGame/Gimmick Definition")]
public sealed class GimmickDefinition : ScriptableObject
{
    public string Id => name; 
}