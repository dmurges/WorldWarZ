using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class MenuScript : MonoBehaviour {


	public Canvas quitMenu;
	public Button startText;
	public Button exitText;


	void Start () {
		quitMenu = quitMenu.GetComponent<Canvas> ();
		startText = startText.GetComponent<Button> ();
		exitText = exitText.GetComponent<Button> ();
		quitMenu.enabled = false;
        Text highScore = GameObject.Find("HighScore").GetComponent<Text>();
        highScore.text = "HIGH SCORE: " + PlayerPrefs.GetFloat("high_score");
	}
	

	void Update () {
	
	}

	public void ExitPress()
	{
		quitMenu.enabled = true;
		startText.enabled = false;
		exitText.enabled = false;
	}

	public void NoPress ()
	{
		quitMenu.enabled = false;
		startText.enabled = true;
		exitText.enabled = true;
	}

	public void LoadCharacterMode ()
	{
		SceneManager.LoadScene ("1_training");
	}

    public void LoadBallMode()
    {
        SceneManager.LoadScene("2_training");
    }

    public void ExitGame ()
	{
		Application.Quit ();
	}

	public void ObjectiveButton(){
		SceneManager.LoadScene ("ObjectiveScene");
	}

	public void BackButton(){
		SceneManager.LoadScene ("MenuScene");
	
	}
}
