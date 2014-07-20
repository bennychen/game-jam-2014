using UnityEngine;

public class SwineNpc : MonoBehaviour
{
	private void Awake()
	{
		_rotateSpeed = Random.Range(_minRotateSpeed, _maxRotateSpeed);
		_rotateShift = Random.Range(-20, 20);
		_xShift = Random.Range(-0.05f, 0.05f);
		_yShift = Random.Range(-0.05f, 0.05f);
		_originalLocalPosition = transform.localPosition;
		_timeShift = Random.Range(0, 10);
	}

	private void Update()
	{
		transform.eulerAngles = Vector3.forward * Mathf.Lerp(-30 + _rotateShift, 30 + _rotateShift, 
		                                                     Mathf.PingPong(Time.time * _rotateSpeed, 1));
		transform.localPosition = _originalLocalPosition + new Vector3(
			Mathf.Lerp(-0.1f + _xShift, 0.1f + _yShift, Mathf.PingPong(Time.time * 10 + _timeShift, 1)),
			Mathf.Lerp(-0.1f + _yShift, 0.1f + _yShift, Mathf.PingPong(Time.time * 10 + _timeShift, 1)),
			0) * 0.5f;
	}

	[SerializeField]
	private float _minRotateSpeed = 1;

	[SerializeField]
	private float _maxRotateSpeed = 2;

	private float _rotateSpeed;
	private float _rotateShift;
	private Vector3 _originalLocalPosition;
	private float _xShift;
	private float _yShift;
	private float _timeShift;
}