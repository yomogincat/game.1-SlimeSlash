using UnityEngine;

[CreateAssetMenu(fileName = "Ability", menuName = "Scriptable Objects/Ability")]
public abstract class Ability : ScriptableObject
{
    [SerializeField] string abilityName;
    [SerializeField] Sprite icon;
    [TextArea]
    [SerializeField] string description;

    public string AbilityName => abilityName;
    public Sprite Icon => icon;
    public string Description => description;
}
