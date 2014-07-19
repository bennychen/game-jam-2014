using UnityEngine;

public class HomeScreen : MonoBehaviour
{
	public void StartGame()
	{
		Application.LoadLevel("level001");
	}

	[SerializeField]
	private tk2dUIItem _startButton;
}