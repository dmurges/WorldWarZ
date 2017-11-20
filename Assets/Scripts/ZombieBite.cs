using UnityEngine;
using System.Collections;

public class ZombieBite : Skill
{
    // Use this for initialization
    public override void Start()
    {
        base.Start();
        base.changeInLife = 20.0f;
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();
    }

    public override void HandlePhysics()
    {
        if (!isExecuting)
        {
            Invoke("EnableCollision", 1.0f);
        }
    }

    public override void UpdateAnimator()
    {
        if (!isExecuting)
        {
            animator.SetTrigger("Skill1");
            isExecuting = true;
        }
    }

    void EnableCollision()
    {
        skillCollider.enabled = true;
        Invoke("DisableCollision", 0.1f);
    }


    void DisableCollision()
    {
        skillCollider.enabled = false;
    }

    void OnTriggerEnter(Collider collider)
    {
        GameObject characterObject = collider.gameObject;
        PlayerCharacter characterComponent = characterObject.GetComponent<PlayerCharacter>();
        if (characterComponent)
        {
            this.collidedCharacter = characterObject;
            characterComponent.TakeImpact();
            characterObject.GetComponent<Health>().DecreaseHealth(changeInLife);
            //			Invoke ("PushBack", 0.3f);
        }
    }

    void PushBack()
    {
        Rigidbody rigidBody = collidedCharacter.GetComponent<Rigidbody>();
        rigidBody.AddForce((collidedCharacter.transform.position - transform.position) * 5000f);
    }
}