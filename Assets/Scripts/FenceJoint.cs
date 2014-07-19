using UnityEngine;

public class FenceJoint : MonoBehaviour
{
	private void OnDrawGizmos()
	{
		Color backColor = Gizmos.color;
		Gizmos.color = Color.red;
		Gizmos.DrawSphere(transform.position, 0.1f);
		Gizmos.color = backColor;
	}
}