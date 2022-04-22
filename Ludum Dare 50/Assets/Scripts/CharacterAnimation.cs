using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;
using Spine;

public class CharacterAnimation : MonoBehaviour
{
    [Header("Front Anim")]
    [SpineAnimation] public string idleFrontAnimation;
    [SpineAnimation] public string runFrontAnimation;
    [SpineAnimation] public string attackFrontAnimation;
    [SpineAnimation] public string dashFrontAnimation;
    [SpineAnimation] public string hitFrontAnimation;

    [Header("Side Anim")]
    [SpineAnimation] public string idleSideAnimation;
    [SpineAnimation] public string runSideAnimation;
    [SpineAnimation] public string attackSideAnimation;
    [SpineAnimation] public string dashSideAnimation;
    [SpineAnimation] public string hitSideAnimation;

    [Header("Back Anim")]
    [SpineAnimation] public string idleBackAnimation;
    [SpineAnimation] public string runBackAnimation;
    [SpineAnimation] public string attackBackAnimation;
    [SpineAnimation] public string dashBackAnimation;
    [SpineAnimation] public string hitBackAnimation;
    [SpineAnimation] public string deathAnimation;



    SkeletonAnimation skeletonAnimation;
    Spine.Skeleton skeleton;
    Spine.AnimationState spineAnimationState;

    PlayerMovement playerMovement;
    Hp playerHp;
    AttackScript attackScript;
    DashScript playerDash;

    public Spine.AnimationState SpineAnimationState { get => spineAnimationState; }

    private void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerHp = GetComponent<Hp>();
        skeletonAnimation = transform.GetChild(0).GetComponent<SkeletonAnimation>();
        skeleton = skeletonAnimation.Skeleton;
        spineAnimationState = skeletonAnimation.AnimationState;
        attackScript = GetComponent<AttackScript>();
        playerDash = GetComponent<DashScript>();
        spineAnimationState.SetAnimation(0, idleFrontAnimation, true);
    }
    private void Update()
    {
        if (!GameController.Instance.Paused)
        {
            if (!GameController.Instance.GameOver)
            {
                if (attackScript)
                {
                    if (!attackScript.IsAttacking)
                    {
                        if (playerDash)
                        {
                            if (!playerDash.isDashing)
                            {
                                ReturnToIdleAnim();
                                CheckWalkSide();
                            }
                        }
                    }
                }
            }
        }
    }

    public void CheckAttackAnimSide()
    {
        switch (playerMovement.Side)
        {
            case 0:
                if (spineAnimationState.GetCurrent(0).Animation.Name == attackFrontAnimation)
                    return;
                DashAttackHitAnimSide(attackBackAnimation, 1);
                break;

            case 1:
                if (spineAnimationState.GetCurrent(0).Animation.Name == attackFrontAnimation)
                    return;
                DashAttackHitAnimSide(attackFrontAnimation, 1);

                break;
            case 2:
                if (spineAnimationState.GetCurrent(0).Animation.Name == attackSideAnimation)
                    return;
                DashAttackHitAnimSide(attackSideAnimation, -1);
                break;
            case 3:
                if (spineAnimationState.GetCurrent(0).Animation.Name == attackSideAnimation)
                    return;
                DashAttackHitAnimSide(attackSideAnimation, 1);
                break;
        }
    }

    public void AnimSide(string animationName, int flipSide, bool loopTime)
    {
        skeleton.ScaleX = flipSide;
        if (!playerHp.Hitted)
            spineAnimationState.SetAnimation(0, animationName, loopTime);
    }

    public void CheckDashSide(int side)
    {
        switch (side)
        {
            case 0:
                if (spineAnimationState.GetCurrent(0).Animation.Name == dashBackAnimation)
                    return;
                DashAttackHitAnimSide(dashBackAnimation, 1);
                break;
            case 1:
                if (spineAnimationState.GetCurrent(0).Animation.Name == dashFrontAnimation)
                    return;
                DashAttackHitAnimSide(dashFrontAnimation, 1);
                break;
            case 2:
                if (spineAnimationState.GetCurrent(0).Animation.Name == dashSideAnimation)
                    return;
                DashAttackHitAnimSide(dashSideAnimation, -1);
                break;
            case 3:
                if (spineAnimationState.GetCurrent(0).Animation.Name == dashSideAnimation)
                    return;
                DashAttackHitAnimSide(dashSideAnimation, 1);
                break;
        }
    }
    public void DashAttackHitAnimSide(string animationName, int flipSide)
    {
        skeleton.ScaleX = flipSide;
        spineAnimationState.SetAnimation(0, animationName, false);
    }

    void CheckWalkSide()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        if (vertical > 0)
        {
            playerMovement.Side = 0;
            if (spineAnimationState.GetCurrent(0).Animation.Name == runBackAnimation)
            {
                return;
            }
            AnimSide(runBackAnimation, 1, true);
            return;
        }
        else if (vertical < 0)
        {
            playerMovement.Side = 1;
            if (spineAnimationState.GetCurrent(0).Animation.Name == runFrontAnimation)
            {
                return;
            }
            AnimSide(runFrontAnimation, 1, true);
        }
        else if (horizontal > 0)
        {
            playerMovement.Side = 3;
            if (spineAnimationState.GetCurrent(0).Animation.Name == runSideAnimation)
            {
                return;
            }
            AnimSide(runSideAnimation, 1, true);
            return;
        }
        else if (horizontal < 0)
        {
            playerMovement.Side = 2;
            if (spineAnimationState.GetCurrent(0).Animation.Name == runSideAnimation)
            {
                return;
            }
            AnimSide(runSideAnimation, -1, true);
            return;
        }
    }

    public void ReturnToIdleAnim()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        if (!playerHp.Hitted)
        {
            if (horizontal == 0 && vertical == 0)
            {
                if (spineAnimationState.GetCurrent(0).Animation.Name == idleFrontAnimation)
                {
                    return;
                }
                playerMovement.Side = 1;
                skeleton.ScaleX = 1;
                spineAnimationState.SetAnimation(0, idleFrontAnimation, true);
            }
        }
    }
}
