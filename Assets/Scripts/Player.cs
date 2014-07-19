using UnityEngine;

public class Player : MonoBehaviour
{
	private void Start()
	{
	}

	private void FixedUpdate()
	{
		rigidbody2D.AddForce(new Vector2(transform.right.x * _speedFactor, 
		                                 transform.right.y * _speedFactor));

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
		_needToChangeRotation = false;
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
		if (Random.Range(0, 100) % 5 == 0)
		{
			// add some noise
			rot = Quaternion.Euler(0, 0, _clockwise * 2 * _rotateFactor * Time.fixedDeltaTime);
		}
		else
		{
			rot = Quaternion.Euler(0, 0, _clockwise * _rotateFactor * Time.fixedDeltaTime);
		}
		transform.localRotation *= rot;
    }

	[SerializeField]
	private float _speedFactor = 1f;

	[SerializeField]
	private float _rotateFactor = 60f;

	private bool _needToChangeRotation = false;
	private int _clockwise;
}