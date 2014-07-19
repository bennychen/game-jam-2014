using UnityEngine;

public class Teleport : MonoBehaviour
{
	private void Awake()
	{
		_pair = transform.parent.GetComponent<TeleportPair>();
		if (_pair == null)
		{
			Debug.LogError("Teleport must parent to a TeleportPair.");
		}
	}

	private void OnTriggerEnter2D(Collider2D other) 
	{
		if (_pair != null && other.tag.Equals("Player"))
		{
			_pair.DoTeleport(other.GetComponent<Player>(), this);
		}
	}

	private TeleportPair _pair;
}