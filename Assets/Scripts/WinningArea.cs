using UnityEngine;

public class WinningArea : MonoBehaviour
{
	void OnTriggerEnter2D(Collider2D other) 
	{
		Time.timeScale = 0;
		Debug.Log("player wins");
	}
}