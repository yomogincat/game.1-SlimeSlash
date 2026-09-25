using System.Collections.Generic;
using UnityEngine;

public class AbilityManager : MonoBehaviour
{
	private int abilitySlotCount = 1;

	[SerializeField]
	private List<Ability> abilities = new List<Ability>();

	[SerializeField]
	private List<Ability> equippedAbilities = new List<Ability>();

	[SerializeField]
	private Player player;

	public List<Ability> EquippedAbilities => equippedAbilities;

	public void AddAbility(Ability ability)
	{
		if (!abilities.Contains(ability))
		{
			abilities.Add(ability);
		}
	}

	public void EquipAbility(Ability ability)
	{
		if (abilities.Contains(ability) && !equippedAbilities.Contains(ability) && equippedAbilities.Count < abilitySlotCount)
		{
			equippedAbilities.Add(ability);
		}
	}

	public bool HasAbility(Ability ability)
	{
		return abilities.Contains(ability);
	}

	public void UseAbility(int slot)
	{
		if (equippedAbilities.Count > slot)
		{
			equippedAbilities[slot].HandleAbility(player);
		}
	}
}
