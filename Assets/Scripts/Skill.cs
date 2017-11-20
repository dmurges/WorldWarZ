using UnityEngine;
using System.Collections;

public abstract class Skill : MonoBehaviour {
	
	internal Animator animator;
	internal SphereCollider skillCollider;
	internal GameObject collidedCharacter;
    internal float timeLastTriggered = 0f;
    public float cooldown = 2.0f;
    internal float changeInLife;
    public bool isExecuting = false;

    public abstract void HandlePhysics ();
	public abstract void UpdateAnimator ();

	public virtual void Start () {
		animator = GetComponentInParent<Animator> ();
		skillCollider = GetComponent<SphereCollider> ();
	}

    public virtual void Update()
    {
        timeLastTriggered += Time.deltaTime;
        if (timeLastTriggered >= cooldown)
        {
            isExecuting = false;
            timeLastTriggered = 0;
        }
    }
}
