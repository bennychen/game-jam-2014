using UnityEngine;

public class AutoFlash : MonoBehaviour
{
	private void Awake()
	{
		_sprite = GetComponent<tk2dSprite>();
		Go.to(_sprite, 0.5f, new GoTweenConfig().colorProp("color", new Color(1, 1, 1, 0)).setIterations(-1));
	}

	private void OnDestroy()
	{
		Go.killAllTweensWithTarget(_sprite);
	}

	private tk2dSprite _sprite;
}