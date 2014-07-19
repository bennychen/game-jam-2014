using UnityEngine;

public class LevelEnd : MonoBehaviour
{
	public static bool IsWinning { get; set; }

	public void RetartGame()
	{
		Application.LoadLevel("level001");
	}

	private void OnEnable()
	{
		Debug.Log("player winning = " + IsWinning);
		_winningText.text = IsWinning ? "You Wins" : "You failed";
	}

	
	[SerializeField]
	private tk2dUIItem _restartButton;

	[SerializeField]
	private tk2dTextMesh _winningText;
}