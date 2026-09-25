using UnityEngine;

[CreateAssetMenu(fileName = "Ability", menuName = "Scriptable Objects/Ability")]
public abstract class Ability : ScriptableObject
{
	[SerializeField]
	private string abilityName;

	[SerializeField]
	private Sprite icon;

	[TextArea]
	[SerializeField]
	private string description;

	public string AbilityName => abilityName;

	public Sprite Icon => icon;

	public string Description => description;

	public abstract void HandleAbility(Player player);
}
