using UnityEngine;

public static class Game
{
	public static int CurrentLevelIndex
	{
		get
		{
			return PlayerPrefs.GetInt("current_level", 0);
		}
		set
		{
			PlayerPrefs.SetInt("current_level", Mathf.Clamp(value, 0, _levels.Length - 1));
		}
	}

	public static bool HasNextLevel
	{
		get
		{
			return CurrentLevelIndex < _levels.Length - 1;
		}
	}

	public static void LoadCurrentLevel()
	{
		Application.LoadLevel(_levels[CurrentLevelIndex]);
	}

	public static void LoadNextLevel()
	{
		if (HasNextLevel)
		{
			++CurrentLevelIndex;
			LoadCurrentLevel();
		}
	}

	private static string[] _levels = new string[] {
		"level001",
		"level002"
	};
}