using UnityEngine;

public class GridGizmos : MonoBehaviour
{
	public static GridGizmos Instance
	{
		get
		{
			if (_instance == null)
			{	
				_instance = GameObject.FindObjectOfType<GridGizmos>();

				if (_instance == null)
				{
					GameObject go = new GameObject("Grid");
					_instance = go.AddComponent<GridGizmos>();
				}
			}
			return _instance;
		}
	}

    public void SnapTransformToGridNode(Transform transform)
    {
        float closestGridX = GetClosestGridX(transform.position.x);
        float closestGridY = GetClosetGridY(transform.position.y);
        transform.position = new Vector3(closestGridX,
            closestGridY, transform.position.z);
    }

    public void SnapTransformToHorizontalGrid(Transform transform)
    {
        float closestGridX = GetClosestGridX(transform.position.x);
        transform.position = new Vector3(closestGridX,
            transform.position.y, transform.position.z);
    }

    public void SnapTransformToVerticalGrid(Transform transform)
    {
        float closestGridY = GetClosetGridY(transform.position.y);
        transform.position = new Vector3(transform.position.x,
            closestGridY, transform.position.z);
    }

    public float GetClosestGridX(float x)
    {
        return ClampToBoundsX(Mathf.Floor(x / GridWidth + 0.5f) * GridWidth);
    }

    public float GetClosetGridY(float y)
    {
        return ClampToBoundsY(Mathf.Floor(y / GridHeight + 0.5f) * GridHeight);
    }

    public float GetLeftGridX(float x)
    {
        return ClampToBoundsX(Mathf.Floor(x / GridWidth) * GridWidth);
    }

    public float GetLowerGridY(float y)
    {
        return ClampToBoundsY(Mathf.Floor(y / GridHeight) * GridHeight);
    }

    public float GetRightGridX(float x)
    {
        return ClampToBoundsX(Mathf.Ceil(x / GridWidth) * GridWidth);
    }

    public float GetUpperGridY(float y)
    {
        return ClampToBoundsY(Mathf.Ceil(y / GridHeight) * GridHeight);
    }

    private float ClampToBoundsX(float x)
    {
        return Mathf.Clamp(x, _leftBottom.x, _leftBottom.x + _bounds.x);
    }

    private float ClampToBoundsY(float y)
    {
        return Mathf.Clamp(y, _leftBottom.y, _leftBottom.y + _bounds.y);
    }

    public Color color = Color.white;

    private void OnDrawGizmos()
    {
        Gizmos.color = color;

        for (float y = _leftBottom.y; y < _leftBottom.y + _bounds.y; y += GridHeight)
        {
            Gizmos.DrawLine(new Vector3(_leftBottom.x, GetLowerGridY(y), 0.0f),
                            new Vector3(_leftBottom.x + _bounds.x, GetLowerGridY(y), 0.0f));
        }

        for (float x = _leftBottom.x; x < _leftBottom.x + _bounds.x; x += GridWidth)
        {
            Gizmos.DrawLine(new Vector3(GetLeftGridX(x), _leftBottom.y, 0.0f),
                            new Vector3(GetLeftGridX(x), _leftBottom.y + _bounds.y, 0.0f));
        }
    }

    public float GridWidth { get { return _gridSize.x; } }
    public float GridHeight { get { return _gridSize.y; } }

    [SerializeField]
    private Vector2 _gridSize = Vector2.one;

    [SerializeField]
    private Vector2 _leftBottom = Vector2.zero;

    [SerializeField]
    private Vector2 _bounds = new Vector2(1200.0f, 800.0f);

	private static GridGizmos _instance = null;
}

