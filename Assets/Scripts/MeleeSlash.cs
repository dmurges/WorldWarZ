using UnityEngine;
using System.Collections;

public class MeleeSlash : Skill
{
    // Use this for initialization
    public override void Start()
    {
        base.Start();
        base.changeInLife = 100.0f;
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
            skillCollider.enabled = true;
        }
    }

    public override void UpdateAnimator()
    {
        if (!isExecuting)
        {
            animator.SetTrigger("Skill1");
            GetComponent<ParticleSystem>().Play();
            Invoke("DisableCollision", 0.5f);
            isExecuting = true;

        }

    }

    void DisableCollision()
    {
        GetComponent<SphereCollider>().enabled = false;
    }

    void OnTriggerEnter(Collider collider)
    {
        GameObject characterObject = collider.gameObject;
        ZombieCharacter characterComponent = characterObject.GetComponent<ZombieCharacter>();
        if (characterComponent)
        {
            this.collidedCharacter = characterObject;
            characterComponent.TakeImpact();
            characterObject.GetComponent<Health>().DecreaseHealth(changeInLife);
            Invoke ("PushBack", 0.3f);
        }
    }

    void PushBack()
    {
        Rigidbody rigidBody = collidedCharacter.GetComponent<Rigidbody>();
        rigidBody.AddForce((collidedCharacter.transform.position - transform.position) * 5000f);
    }
}