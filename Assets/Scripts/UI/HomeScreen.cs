using UnityEngine;

public class HomeScreen : MonoBehaviour
{
	public void StartGame()
	{
		Game.LoadCurrentLevel();
	}

	[SerializeField]
	private tk2dUIItem _startButton;
}