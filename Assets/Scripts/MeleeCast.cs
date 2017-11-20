using UnityEngine;
using System.Collections;

public class MeleeCast : Skill
{
    // Use this for initialization
    public override void Start()
    {
        base.Start();
        base.changeInLife = 200.0f; // set damage value
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
            skillCollider.enabled = true; // enable collider
        }
    }

    public override void UpdateAnimator()
    {
        if (!isExecuting)
        {
            animator.SetTrigger("Skill3");
            GetComponent<ParticleSystem>().Play();
            Invoke("DisableCollision", 0.5f); // disable collider after 0.5s
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
            characterComponent.TakeImpact(); // impact animation
            characterObject.GetComponent<Health>().DecreaseHealth(changeInLife);
        }
    }
}