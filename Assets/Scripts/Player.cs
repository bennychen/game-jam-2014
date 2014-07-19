using UnityEngine;

public class Player : MonoBehaviour
{
	private void Start()
	{
		_lastPosition = transform.position;
	}

	private void FixedUpdate()
	{
		if ((transform.position - _lastPosition).sqrMagnitude < Mathf.Epsilon)
		{
			_playerStuckTime += Time.fixedDeltaTime;
		}
		else
		{
			_playerStuckTime = 0;
		}

		rigidbody2D.AddForce(new Vector2(transform.right.x * _speedFactor, 
		                                 transform.right.y * _speedFactor));

		_lastPosition = transform.position;

		if (_playerStuckTime > 0.5f)
		{
			_needToChangeRotation = true;
		}

		if (_needToChangeRotation)
		{
			ChangeDirection();
		}
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		_needToChangeRotation = true;
		_clockwise = Random.Range(0, 100) % 2 == 0 ? 1 : -1;
	}

	private void OnCollisionExit2D(Collision2D collision)
	{
		float dot = Mathf.Abs(Vector2.Dot(transform.right, Vector2.right));
		if (dot < 0.1f || dot > 0.9f)
		{
			_needToChangeRotation = false;
		}
	}

	private void OnDrawGizmos()
	{
		Color backColor = Gizmos.color;
		Gizmos.color = Color.green;
		Gizmos.DrawRay(new Ray(transform.position, 
		                       transform.right)); 
		Gizmos.color = backColor;
	}

	private void ChangeDirection()
	{
		Quaternion rot = Quaternion.identity;
		if (Random.Range(0, 100) % 10 == 0)
		{
			// add some noise
			rot = Quaternion.Euler(0, 0, -_clockwise * 2 * _rotateFactor * Time.fixedDeltaTime);
		}
		else
		{
			rot = Quaternion.Euler(0, 0, _clockwise * Random.Range(_rotateFactor * 0.5f, _rotateFactor * 2f) * Time.fixedDeltaTime);
		}
		transform.localRotation *= rot;
    }

	[SerializeField]
	private float _speedFactor = 1f;

	[SerializeField]
	private float _rotateFactor = 60f;

	private bool _needToChangeRotation = false;
	private int _clockwise;
	private Vector3 _lastPosition;
	private float _playerStuckTime;
}