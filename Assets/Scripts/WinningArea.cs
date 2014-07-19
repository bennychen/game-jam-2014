using UnityEngine;

public class WinningArea : MonoBehaviour
{
	void OnTriggerEnter2D(Collider2D other) 
	{
		LevelEnd.IsWinning = true;
		Application.LoadLevel("levelend");
	}
}