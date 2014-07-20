using UnityEngine;
using System.Collections;

public enum FenceType
{
	Normal,
	Fixed,
	AutoSpin,
	AutoRotateBack,
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
		_originalRotation = transform.eulerAngles.z;
		if (_type == FenceType.AutoRotateBack)
		Debug.Log(_originalRotation);
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
			StartRotateAnim();
		}
		else if (_type == FenceType.AutoRotateBack)
		{
			if (Mathf.Abs(transform.eulerAngles.z - _originalRotation) < 10)
			{
				StartRotateAnim();
				StartCoroutine(RotateBackInSeconds(_openTime));
			}
		}
	}

	private void StartRotateAnim()
	{
		_uiItem.enabled = false;
		Go.to(transform, 0.3f, new GoTweenConfig().localRotation(
			new Vector3(0, 0, IsHorizontal ? 90 : 0)).onComplete((tween)=>
		{
			_uiItem.enabled = true;
		}));
	}

	private IEnumerator RotateBackInSeconds(float seconds)
	{
		yield return new WaitForSeconds(seconds);
		StartRotateAnim();
	}

	private void Update()
	{
		if (_type == FenceType.AutoSpin)
		{
			transform.rotation *= Quaternion.Euler(new Vector3(0, 0, Time.deltaTime * 100));
		}
	}
	
	private tk2dUIItem _uiItem;
	private float _originalRotation;

	[SerializeField]
	private FenceType _type;

	[SerializeField]
	private float _openTime = 3f;
}
