using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Health : MonoBehaviour
{
    public float health = 100.0f;
    public bool isDead = false;

    public void DecreaseHealth(float value)
    {
        health -= value;
        print("decrease: " + health);
        if (health <= 0 && !isDead)
        {
            Character character = GetComponent<Character>();
            character.Die();    // play animation

			if (GetComponent<ZombieCharacter> ()) {
                // if zombie dies add to score
				GameManager gameManager = FindObjectOfType<GameManager>();
				gameManager.AddNumKills();

			}
			isDead = true;
			Invoke("DestroyCharacter", 5.0f); // destroy after 5 seconds for animation to play
        }
    }

    void DestroyCharacter() {
		if (GetComponent<PlayerCharacter> ()) {
			// load scene
			SceneManager.LoadScene("LoseScene");
		}
        Destroy(this.gameObject);
    }

    public void DestroyObject()
    {
        Destroy(gameObject);
    }
}

