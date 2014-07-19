using UnityEngine;

public enum FenceType
{
	Normal,
	Fixed,
	AutoSpin,
}

public class Fence : MonoBehaviour
{
	public bool IsHorizontal
	{
		get
		{
			return transform.eulerAngles.z % 180 < 90;
		}
	}

	public void InstantOrthographicSpin()
	{
		transform.eulerAngles = new Vector3(0, 0, IsHorizontal ? 90 : 0);
	}

	private void Awake()
	{
		_uiItem = GetComponent<tk2dUIItem>();
	}

	private void OnEnable()
	{
		_uiItem.OnClick += HandleOnClick;
	}

	private void OnDisable()
	{
		_uiItem.OnClick -= HandleOnClick;
	}

	private void HandleOnClick()
	{
		if (_type == FenceType.Normal)
		{
			_uiItem.enabled = false;
			Go.to(transform, 0.3f, new GoTweenConfig().localRotation(
				new Vector3(0, 0, IsHorizontal ? 90 : 0)).onComplete((tween)=>
			      {
					_uiItem.enabled = true;
				  }));
		}
	}

	private void Update()
	{
		if (_type == FenceType.AutoSpin)
		{
			transform.rotation *= Quaternion.Euler(new Vector3(0, 0, Time.deltaTime * 100));
		}
	}
	
	private tk2dUIItem _uiItem;

	[SerializeField]
	private FenceType _type;
}
