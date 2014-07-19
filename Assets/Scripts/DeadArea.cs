using UnityEngine;

public class DeadArea : MonoBehaviour
{
	void OnTriggerEnter2D(Collider2D other) 
	{
		Time.timeScale = 0;
		Debug.Log("player dies");
	}
}