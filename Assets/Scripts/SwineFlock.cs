using UnityEngine;
using GameJam.Utils;

public class SwineFlock : MonoBehaviour
{
	private void Update()
	{
		if (_flockA != null)
		{
			_flockA.position += Vector3.up * Time.deltaTime * _speed;
		}
		if (_flockB != null)
		{
			_flockB.position += Vector3.up * Time.deltaTime * _speed;
		}
		if (_flockA.localPosition.y > _localPositionValueToTriggerReset)
		{
			_flockA.SetPositionY(_flockB.position.y - _halfSize);
		}
		else if (_flockB.localPosition.y > _localPositionValueToTriggerReset)
		{
			_flockB.SetPositionY(_flockA.position.y - _halfSize);
		}
	}
	
	[SerializeField]
	private float _speed = 1;
	
	[SerializeField]
	private float _localPositionValueToTriggerReset;
	
	[SerializeField]
	private float _halfSize;
	
	[SerializeField]
	private Transform _flockA;
	
	[SerializeField]
	private Transform _flockB;
}