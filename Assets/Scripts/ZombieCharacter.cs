using UnityEngine;
using System.Collections;

public class ZombieCharacter : Character
{
    public GameObject normalAttackObject;
    public GameObject skill1Object;
    public GameObject skill2Object;
    public GameObject skill3Object;


    public override void Move(Vector3 move, bool isCrouching, bool isJumping, bool isAttacking, bool usingSkill1, bool usingSkill2, bool usingSkill3)
    {
        base.Move(move, isCrouching, isJumping, isAttacking, usingSkill1, usingSkill2, usingSkill3);

        Skill normalAttack = normalAttackObject.GetComponent<Skill>();
        Skill skill1 = skill1Object.GetComponent<Skill>();
        Skill skill2 = skill2Object.GetComponent<Skill>();
        Skill skill3 = skill3Object.GetComponent<Skill>();

        if (isAttacking)
        {
            if (!normalAttack.isExecuting) {
                normalAttack.HandlePhysics();
            }
        }
        else if (usingSkill1)
        {
            if (!skill1.isExecuting)
            {
                skill1.HandlePhysics();
            }
        }
        else if (usingSkill2)
        {
            skill2.HandlePhysics();
        }
        else if (usingSkill3)
        {
            skill3.HandlePhysics();
        }
    }

    public override void UpdateAnimator(Vector3 move, bool isAttacking, bool usingSkill1, bool usingSkill2, bool usingSkill3)
    {
        base.UpdateAnimator(move, isAttacking, usingSkill1, usingSkill2, usingSkill3);
        Skill normalAttack = normalAttackObject.GetComponent<Skill>();
        Skill skill1 = skill1Object.GetComponent<Skill>();
        Skill skill2 = skill2Object.GetComponent<Skill>();
        Skill skill3 = skill3Object.GetComponent<Skill>();

        if (isAttacking)
        {
            normalAttack.UpdateAnimator();
        }
        else if (usingSkill1)
        {
            skill1.UpdateAnimator();
        }
        else if (usingSkill2)
        {
            skill2.UpdateAnimator();
        }
        else if (usingSkill3)
        {
            skill3.UpdateAnimator();
        }
    }
}