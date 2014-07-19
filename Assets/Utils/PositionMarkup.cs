using UnityEngine;

public class PositionMarkup : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        Color originalColor = Gizmos.color;
        Gizmos.color = _color;
        Gizmos.DrawSphere(transform.position, 0.2f);
        Gizmos.color = originalColor;
    }

    [SerializeField]
    private Color _color;
}