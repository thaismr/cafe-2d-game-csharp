using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MainMenu : MonoBehaviour 
{
	public void BackToMenu() 
	{
        SceneManager.LoadScene ("Menu");
	}

	public void PlayGame() 
	{
		GameManager.GM.Restart ();

		SceneManager.LoadScene ("Jogo");
	}

	public void PlayTutorial() 
	{
		Debug.Log ("PlayTutorial");

		SceneManager.LoadScene ("Tutorial");
	}

	public void PlayGlo() 
	{
		Debug.Log ("PlayGlo");

		SceneManager.LoadScene ("Glossário");
	}


	// PLAY PANEL

	public void PlayMenu()
	{
        GameManager.isPaused = false;

		Time.timeScale = 1;

		Cursor.lockState = CursorLockMode.Locked;

		Cursor.visible = false;
	}


	// PLAY AGAIN

	public void PlayAgain()
	{
		GameManager.GM.Restart ();

		SceneManager.LoadScene ("Jogo");
	}



	public void QuitGame()
	{
		#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
		#else
		Application.Quit();
		#endif
	}
}
