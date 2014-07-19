using UnityEngine;

public class TeleportPair : MonoBehaviour
{
	public bool CanTeleport(Teleport teleport)
	{
			return Time.fixedTime > _lastTeleportTime + _cooldownTime && (_bidirectional || _numOfTeleport <= 0);
	}

	public void DoTeleport(Player player, Teleport teleport)
	{
		Teleport connection = GetConnection(teleport);
		if (CanTeleport(teleport) && connection != null)
		{
			player.transform.position = connection.transform.position;
			_lastTeleportTime = Time.fixedTime;
			++_numOfTeleport;
		}
	}

	public Teleport GetConnection(Teleport teleport)
	{
		if (teleport == _teleportA)
		{
			return _teleportB;
		}
		else if (teleport == _teleportB)
		{
			return _teleportA;
		}
		else
		{
			return null;
		}
	}

	private void OnDrawGizmos()
	{
		if (_teleportA != null && _teleportB != null)
		{
			Color backColor = Gizmos.color;
			Gizmos.color = Color.yellow;
			Gizmos.DrawLine(_teleportA.transform.position, _teleportB.transform.position);
			Gizmos.color = backColor;
		}
	}

	[SerializeField]
	private Teleport _teleportA;

	[SerializeField]
	private Teleport _teleportB;

	[SerializeField]
	private bool _bidirectional;

	[SerializeField]
	private float _cooldownTime = 0.3f;

	private int _numOfTeleport;
	private float _lastTeleportTime;
}