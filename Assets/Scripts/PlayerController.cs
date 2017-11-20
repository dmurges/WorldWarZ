using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.Networking;


public class PlayerController : MonoBehaviour
{
	[Tooltip("The local player instance. Use this to know if the local player is represented in the Scene")]
	public static GameObject LocalPlayerInstance;
	private Character character; // A reference to the ThirdPersonCharacter on the object
	private Transform cam;                  // A reference to the main camera in the scenes transform
	private Vector3 camForward;             // The current forward direction of the camera
	private Vector3 move;
	private bool jump;                      // the world-relative desired move direction, calculated from the camForward and user input.
	private bool attack;                    
	private bool skill1;
	private bool skill2;
	private bool skill3;


	private void Start()
	{
		// get the transform of the main camera
		if (Camera.main != null)
		{
			cam = Camera.main.transform;
		}
		else
		{
			Debug.LogWarning(
				"Warning: no main camera found. Third person character needs a Camera tagged \"MainCamera\", for camera-relative controls.", gameObject);
			// we use self-relative controls in this case, which probably isn't what the user wants, but hey, we warned them!
		}

		// get the third person character ( this should never be null due to require component )
		character = GetComponent<PlayerCharacter>();
	}


	private void Update()
	{

		if (!jump)
		{
			jump = CrossPlatformInputManager.GetButtonDown("Jump");
		}
		if (!attack)
		{
			attack = CrossPlatformInputManager.GetButtonDown("Attack");
		}
		if (!skill1)
		{
			skill1 = CrossPlatformInputManager.GetButtonDown("Skill1");
		}
		if (!skill2)
		{
			skill2 = CrossPlatformInputManager.GetButtonDown("Skill2");
		}
		if (!skill3)
		{
			skill3 = CrossPlatformInputManager.GetButtonDown("Skill3");
		}

	}


	// Fixed update is called in sync with physics
	private void FixedUpdate()
	{


		// read inputs
		float h = CrossPlatformInputManager.GetAxis("Horizontal");
		float v = CrossPlatformInputManager.GetAxis("Vertical");
		bool crouch = Input.GetKey(KeyCode.C);

		// calculate move direction to pass to character
		if (cam != null)
		{
			// calculate camera relative direction to move:
			camForward = Vector3.Scale(cam.forward, new Vector3(1, 0, 1)).normalized;
			move = v*camForward + h*cam.right;
		}
		else
		{
			// we use world-relative directions in the case of no main camera
			move = v*Vector3.forward + h*Vector3.right;
		}
		#if !MOBILE_INPUT
		// walk speed multiplier
		if (Input.GetKey(KeyCode.LeftShift)) move *= 0.5f;
		#endif

		// pass all parameters to the character control script
		character.Move(move, crouch, jump, attack, skill1, skill2, skill3);
		character.UpdateAnimator (move, attack, skill1, skill2, skill3);
		jump = false;
		attack = false;
		skill1 = false;
		skill2 = false;
		skill3 = false;
	}
}
