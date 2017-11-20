using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour {


	#region Public Properties


	[Tooltip("UI Text to display Player's Name")]
	public Text PlayerNameText;


	[Tooltip("UI Slider to display Player's Health")]
	public Slider PlayerHealthSlider;

	[Tooltip("Pixel offset from the player target")]
	public Vector3 ScreenOffset = new Vector3(0f,30f,0f);

	[Tooltip("UI Text to display Player's Health")]
	public Text PlayerHealthText;

	[Tooltip("The green part of the Player's Health Bar")]
	public Image HealthBarFill;
	#endregion


	#region Private Properties

	Character _target;
	float _characterHeight = 0f;
	Transform _targetTransform;
	Vector3 _targetPosition;
	static float HEIGHT_CONSTANT = 1f;
	#endregion


	#region MonoBehaviour Messages
	void Awake(){
		this.GetComponent<Transform>().SetParent (GameObject.Find("Canvas").GetComponent<Transform>());
	}


	void Update()
	{
		// Destroy itself if the target is null, It's a fail safe when Photon is destroying Instances of a Player over the network
		if (_target == null) {
			Destroy(this.gameObject);
			return;
		}

		// Reflect the Player Health
		if (PlayerHealthSlider != null && PlayerHealthText != null && HealthBarFill != null) {
			float currentHealth = _target.getHealth ();
			float maxHealth = PlayerHealthSlider.maxValue;
			float healthPercentage = currentHealth / maxHealth;
			Color healthColor = new Color (1 - healthPercentage, healthPercentage, 0, 1);

			PlayerHealthSlider.value = currentHealth;

			PlayerHealthText.color = healthColor;
			PlayerHealthText.text =  currentHealth.ToString();

			HealthBarFill.color = healthColor;

			if (currentHealth <= 0) {
				HealthBarFill.color = Color.clear;
			}
			
		}


			
	}

	#endregion


	#region Public Methods

	public void SetTarget(Character target){
		if (target == null) {
			Debug.LogError("<Color=Red><a>Missing</a></Color> PlayMakerManager target for PlayerUI.SetTarget.",this);
			return;
		}

		// Cache references for efficiency
		_target = target;

		// Set transform for the player health bar so that it is above the player
		_characterHeight = _target.GetComponent<CapsuleCollider> ().height + target.GetComponent<CapsuleCollider>().center.y + HEIGHT_CONSTANT ;
		_targetTransform = _target.GetComponent<Transform> ();


		if (PlayerHealthText != null) {
			PlayerHealthText.text = _target.getHealth ().ToString();
		}

		// Set the maximum value the slider can take depending on the player's max health
		PlayerHealthSlider.maxValue = _target.getHealth();
		PlayerHealthSlider.minValue = 0f;

	}

	void LateUpdate () {
		// #Critical
		// Follow the Target GameObject on screen.
		if (_targetTransform!=null)
		{
			_targetPosition = _targetTransform.position;
			_targetPosition.y += _characterHeight;
			this.transform.position = Camera.main.WorldToScreenPoint (_targetPosition) + ScreenOffset;
		}
	}


	#endregion


}