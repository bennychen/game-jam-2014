using UnityEngine;

public class LevelEnd : MonoBehaviour
{
	public static bool IsWinning { get; set; }

	public void RetartGame()
	{
		Game.LoadCurrentLevel();
	}

	public void NextGame()
	{
		Game.LoadNextLevel();
	}

	private void OnEnable()
	{
		_comic.SetSprite(IsWinning ? "success" : "fail");
		_comic.scale = Vector3.one * (IsWinning ? 0.3f : 0.7f);
	}

	
	[SerializeField]
	private tk2dUIItem _restartButton;

	[SerializeField]
	private tk2dUIItem _nextButton;

	[SerializeField]
	private tk2dSprite _comic;
}