using UnityEngine;
[System.Serializable]
public struct DashParameters 
{ 
	[SerializeField] float speed;
	[SerializeField] float control;
	[SerializeField] float cost ;
	[SerializeField] float time;

	public float Speed => speed;
	public float Control => control;
	public float Cost => cost; 
	public float Time => time; 

	public DashParameters(float speed, float control, float cost, float time) 
	{ 
		this.speed = speed;
		this.control = control; 
		this.cost = cost; 
		this.time = time;
	} 
} 

[CreateAssetMenu(menuName = "Ability/Dash")]
public class DashAbility : Ability
{ 
	[SerializeField] DashParameters dashSettings;
	public override void HandleAbility(Player player) 
	{
		player.Dash(dashSettings); 
	}
}