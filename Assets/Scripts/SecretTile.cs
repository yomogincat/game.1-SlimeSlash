using UnityEngine;
using UnityEngine.Tilemaps;

public class SecretTile : MonoBehaviour
{
	private Tilemap tilemap;

	private bool isPlayer;

	[SerializeField]
	private float alpha;

	private void Awake()
	{
		tilemap = GetComponent<Tilemap>();
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		isPlayer = other.gameObject.CompareTag("Player");
	}

	private void Update()
	{
		if (isPlayer)
		{
			tilemap.color = new Color(1f, 1f, 1f, alpha);
		}
		else
		{
			tilemap.color = new Color(1f, 1f, 1f, 1f);
		}
	}
}
