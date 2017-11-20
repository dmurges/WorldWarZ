using UnityEngine;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Character))]
public class AICharacterControl : MonoBehaviour
{
    public NavMeshAgent agent { get; private set; }            // the navmesh agent required for the path finding
    public Character character { get; private set; }           // the character we are controlling
    public Transform target;                                   // target to follow

    private bool jump;
    private bool crouch;
    private bool attack;
    private bool skill1;
    private bool skill2;
    private bool skill3;

    private void Start()
    {
        agent = GetComponentInChildren<NavMeshAgent>();
        character = GetComponent<Character>();
        if (FindObjectOfType<PlayerCharacter>())
        {
            this.target = FindObjectOfType<PlayerCharacter>().transform;
        }
        else {
            this.target = FindObjectOfType<SphereCollider>().transform;

        }
        agent.updateRotation = false;
        agent.updatePosition = true;
    }


    private void Update()
    {
        // set the destination for every frame
        if (target != null)
            agent.SetDestination(target.position);

    }
    private void FixedUpdate()
    {
        if (agent.remainingDistance > agent.stoppingDistance)
        {
            // not at stopping distance yet
            character.Move(agent.desiredVelocity, crouch, jump, attack, skill1, skill2, skill3);
            character.UpdateAnimator(agent.desiredVelocity, attack, skill1, skill2, skill3);
        }
        else
        {
            // at stopping distance
            if (character.GetComponent<Health>().health > 0)
            {
                // only attack when health is larger than 0
                // this is to account for crawling animation before they die

                // two types of attack, randomly executed
                float randomIndex = Random.Range(0, 2);
                if (randomIndex == 0)
                {
                    attack = true;
                }
                else if (randomIndex == 1)
                {
                    skill1 = true;
                }
            }
            character.Move(Vector3.zero, crouch, jump, attack, skill1, skill2, skill3);
            character.UpdateAnimator(Vector3.zero, attack, skill1, skill2, skill3);
        }

        // reset all input
        crouch = false;
        attack = false;
        skill1 = false;
        skill2 = false;
        skill3 = false;
    }


    public void SetTarget(Transform target)
    {
        this.target = target;
    }
}
