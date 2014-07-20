using UnityEngine;

public class DeadArea : MonoBehaviour
{
	void OnTriggerEnter2D(Collider2D other) 
	{
		LevelEnd.IsWinning = false;
		Application.LoadLevel("levelend");
	}
}