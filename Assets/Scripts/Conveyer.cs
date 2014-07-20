using UnityEngine;
using GameJam.Utils;

public class Conveyer : MonoBehaviour
{
	private void Update()
	{
		if (_conveyerA != null)
		{
			_conveyerA.position += Vector3.right * Time.deltaTime * _speed;
		}
		if (_conveyerB != null)
		{
			_conveyerB.position += Vector3.right * Time.deltaTime * _speed;
		}
		if (_conveyerA.localPosition.x > _localPositionValueToTriggerReset)
		{
			_conveyerA.SetPositionX(_conveyerB.position.x - _halfSize);
		}
		else if (_conveyerB.localPosition.x > _localPositionValueToTriggerReset)
		{
			_conveyerB.SetPositionX(_conveyerA.position.x - _halfSize);
		}
	}

	[SerializeField]
	private float _speed = 50;

	[SerializeField]
	private float _localPositionValueToTriggerReset;

	[SerializeField]
	private float _halfSize;

	[SerializeField]
	private Transform _conveyerA;

	[SerializeField]
	private Transform _conveyerB;
}