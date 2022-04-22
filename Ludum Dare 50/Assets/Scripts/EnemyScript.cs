using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using UnityEngine.AI;

public class EnemyScript : MonoBehaviour
{
    public Transform player;
    public GameObject enemySprite;

    NavMeshAgent navmesh;

    public float minDistance;

    AttackScript attackScript;
    ThrowProjectile throwProjectileScript;

    Hp hp;

    int side;

    public bool attacking;
    public float timeAttackCD;

    CharacterAnimation characterAnimation;
    public enum EnemyType
    {
        Melee,
        Ranged
    }

    public EnemyType enemyType;
    // Start is called before the first frame update
    void Start()
    {
        characterAnimation = GetComponent<CharacterAnimation>();
        if (enemyType == EnemyType.Melee)
        {
            attackScript = GetComponent<AttackScript>();

        }
        else if (enemyType == EnemyType.Ranged)
        {
            throwProjectileScript = GetComponent<ThrowProjectile>();
        }

        hp = GetComponent<Hp>();
        player = GameController.Instance.Player.transform;
        
        

        navmesh = GetComponent<NavMeshAgent>();
        navmesh.updateRotation = false;
        navmesh.updateUpAxis = false;
        navmesh.isStopped = false;

    }


    // Update is called once per frame
    void Update()
    {
        if (attacking)
        {
            timeAttackCD += Time.deltaTime;
            if(timeAttackCD >= 2)
            {
                timeAttackCD = 0;
                attacking = false;
            }
        }

        navmesh.SetDestination(player.position);

        Vector3 diff = player.position - transform.position;
        diff.Normalize();
        float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rot_z);

        Quaternion rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        enemySprite.transform.rotation = rotation;

        if (attackScript)
        {
            if (!attackScript.IsAttacking)
                CheckSide();
        }
        else
        {
            if(!throwProjectileScript.IsShooting)
                CheckSide();
        }

        if (enemyType == EnemyType.Melee)
        {
            MeleeFollow();
        }
        else if (enemyType == EnemyType.Ranged)
        {
            RangeFollow();
        }

        

    }

    void MeleeFollow()
    {
        if (navmesh.remainingDistance <= minDistance)
        {
            if (!attacking)
            {
                attacking = true;
                navmesh.isStopped = true;

                if (!attackScript.IsAttacking)
                {
                    if (this.transform.position.y < player.position.y)
                    {
                        side = 0;
                        if (characterAnimation.SpineAnimationState.GetCurrent(0).Animation.Name == characterAnimation.attackBackAnimation)
                        {
                            return;
                        }
                        characterAnimation.AnimSide(characterAnimation.attackBackAnimation, 1, false);

                        attackScript.Attack(0);
                    }
                    else if (this.transform.position.y > player.position.y)
                    {
                        side = 1;
                        if (characterAnimation.SpineAnimationState.GetCurrent(0).Animation.Name == characterAnimation.attackFrontAnimation)
                        {
                            return;
                        }
                        characterAnimation.AnimSide(characterAnimation.attackFrontAnimation, 1, false);

                        attackScript.Attack(1);
                    }
                    else if (this.transform.position.x < player.position.x)
                    {
                        side = 3;
                        if (characterAnimation.SpineAnimationState.GetCurrent(0).Animation.Name == characterAnimation.attackSideAnimation)
                        {
                            return;
                        }
                        characterAnimation.AnimSide(characterAnimation.attackSideAnimation, 1, false);

                        attackScript.Attack(3);
                    }
                    else if (this.transform.position.x > player.position.x)
                    {
                        side = 2;
                        if (characterAnimation.SpineAnimationState.GetCurrent(0).Animation.Name == characterAnimation.attackSideAnimation)
                        {
                            return;
                        }
                        characterAnimation.AnimSide(characterAnimation.attackSideAnimation, -1, false);

                        attackScript.Attack(2);
                    }
                }
            }
        }
        else
        {
            if (!attackScript.IsAttacking)
                navmesh.isStopped = false;
        }
    }

    void CheckSide()
    {
        if (player.position.x >= this.transform.position.x)
        {
            if (player.position.y < this.transform.position.y)
            {
                side = 1;
                if (characterAnimation.SpineAnimationState.GetCurrent(0).Animation.Name == characterAnimation.runFrontAnimation)
                {
                    return;
                }
                characterAnimation.AnimSide(characterAnimation.runFrontAnimation, 1, true);
            }
            else if (player.position.y > this.transform.position.y)
            {
                side = 0;
                if (characterAnimation.SpineAnimationState.GetCurrent(0).Animation.Name == characterAnimation.runBackAnimation)
                {
                    return;
                }
                characterAnimation.AnimSide(characterAnimation.runBackAnimation, 1, true);
                return;
            }
            else
            {
                side = 3;
                if (characterAnimation.SpineAnimationState.GetCurrent(0).Animation.Name == characterAnimation.runSideAnimation)
                {
                    return;
                }
                characterAnimation.AnimSide(characterAnimation.attackSideAnimation, 1, true);

            }
        }
        else if (player.position.x < this.transform.position.x)
        {
            if (player.position.y < this.transform.position.y)
            {
                side = 1;
                if (characterAnimation.SpineAnimationState.GetCurrent(0).Animation.Name == characterAnimation.runFrontAnimation)
                {
                    return;
                }
                characterAnimation.AnimSide(characterAnimation.runFrontAnimation, 1, false);
            }
            else if (player.position.y > this.transform.position.y)
            {
                side = 0;
                if (characterAnimation.SpineAnimationState.GetCurrent(0).Animation.Name == characterAnimation.runBackAnimation)
                {
                    return;
                }
                characterAnimation.AnimSide(characterAnimation.runBackAnimation, 1, true);
                return;
            }
            else
            {
                side = 2;
                if (characterAnimation.SpineAnimationState.GetCurrent(0).Animation.Name == characterAnimation.runSideAnimation)
                {
                    return;
                }
                characterAnimation.AnimSide(characterAnimation.attackSideAnimation, 1, true);
                return;
            }

        }
    }
    void RangeFollow()
    {
        if (navmesh.remainingDistance <= minDistance)
        {
            navmesh.isStopped = true;

            if (!throwProjectileScript.IsShooting)
            {
                if (this.transform.position.x < player.position.x)
                {
                    throwProjectileScript.Shoot();
                    if (characterAnimation.SpineAnimationState.GetCurrent(0).Animation.Name == characterAnimation.attackSideAnimation)
                    {
                        return;
                    }
                    characterAnimation.AnimSide(characterAnimation.attackSideAnimation, 1, false);

                }
                else
                {
                    throwProjectileScript.Shoot();
                    if (characterAnimation.SpineAnimationState.GetCurrent(0).Animation.Name == characterAnimation.attackSideAnimation)
                    {
                        return;
                    }
                    characterAnimation.AnimSide(characterAnimation.attackSideAnimation, -1, false);
                }
            }
        }
        else
        {
            if (!throwProjectileScript.IsShooting)
                navmesh.isStopped = false;
        }
    }
}
