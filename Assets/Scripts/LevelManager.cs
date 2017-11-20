using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class LevelManager : MonoBehaviour {

	public GameObject pauseMenu;

	private void Start()
	{
		pauseMenu.SetActive (false);
	}

	public void TogglePauseMenu()
	{
		pauseMenu.SetActive (!pauseMenu.activeSelf);
	}

	public void ToMenu()
	{
		SceneManager.LoadScene ("MainMenu");
	}
}
