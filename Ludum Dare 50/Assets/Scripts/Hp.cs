using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;


public class Hp : MonoBehaviour
{
    [SerializeField] float maxHp = 100;
    [SerializeField] float currentHp;
    [SerializeField] float maxStamina = 100;
    [SerializeField] float currentStamina;
    [SerializeField] GameObject impactVFX;

    bool hitted;
    bool invulnerable;
    public float CurrentHp { get => currentHp; }
    public float MaxHp { get => maxHp; }
    public bool Hitted { get => hitted; set => hitted = value; }
    public float MaxStamina { get => maxStamina; }
    public float CurrentStamina { get => currentStamina; }
    public int Score { get => score; set => score = value; }

    [SerializeField] int score;

    [SerializeField] GameObject hpDrop;
    [SerializeField] GameObject staminaDrop;

    [SerializeField] AudioClip damageSound;
    [SerializeField] AudioClip deadSound;

    Coroutine hpRoutine;
    Coroutine staminaRoutine;

    CharacterAnimation characterAnimation;

    // Start is called before the first frame update
    void Start()
    {
        currentHp = maxHp;
        currentStamina = maxStamina;
        characterAnimation = GetComponent<CharacterAnimation>();
    }

    // Update is called once per frame
    void Update()
    {
        currentStamina += 0.01f;
        if (currentStamina >= maxStamina)
        {
            currentStamina = maxStamina;
        }
    }

    public void RemoveHpGame(float value)
    {
        if (currentHp <= 0)
            return;

        currentHp -= value;
        if (CurrentHp <= 0)
        {
            currentHp = 0;
            characterAnimation.SpineAnimationState.SetAnimation(0, characterAnimation.deathAnimation, false);

            StartCoroutine(PlayerCoroutine());
        }
    }
    public void RemoveStamina(float value)
    {
        currentStamina -= value;
        if(CurrentStamina <= 0)
        {
            currentStamina = 0;
        }
    }

    public void RemoveHp(float value)
    {
        if (invulnerable)
            return;
         

        currentHp -= value;

        StartCoroutine(HitRoutine());
        if (CompareTag("Player"))
        {
            GameController.Instance.AudioSource.PlayOneShot(damageSound);
            StartCoroutine(InvulnerableCoroutine());

            int side = GetComponent<PlayerMovement>().Side;

            switch (side)
            {
                case 0:
                    if (characterAnimation.SpineAnimationState.GetCurrent(0).Animation.Name == characterAnimation.hitBackAnimation)
                    {
                        return;
                    }
                    characterAnimation.DashAttackHitAnimSide(characterAnimation.hitBackAnimation, 1);

                    break;

                case 1:

                    if (characterAnimation.SpineAnimationState.GetCurrent(0).Animation.Name == characterAnimation.hitFrontAnimation)
                    {
                        return;
                    }
                    characterAnimation.DashAttackHitAnimSide(characterAnimation.hitFrontAnimation, 1);


                    break;

                case 2:
                    if (characterAnimation.SpineAnimationState.GetCurrent(0).Animation.Name == characterAnimation.hitSideAnimation)
                    {
                        return;
                    }
                    characterAnimation.DashAttackHitAnimSide(characterAnimation.hitSideAnimation, 1);


                    break;
                case 3:
                    if (characterAnimation.SpineAnimationState.GetCurrent(0).Animation.Name == characterAnimation.hitSideAnimation)
                    {
                        return;
                    }
                    characterAnimation.DashAttackHitAnimSide(characterAnimation.hitSideAnimation, 1);


                    break;
            }
        }


        if (CurrentHp <= 0)
        {
            currentHp = 0;
            characterAnimation.SpineAnimationState.SetAnimation(0, characterAnimation.deathAnimation, false);


            if (CompareTag("Player"))
            {
                StartCoroutine(PlayerCoroutine());
            }

            if (CompareTag("Enemy"))
            {
                GameController.Instance.AudioSource.PlayOneShot(deadSound);

                GameController.Instance.Score += score;
                StartCoroutine(DeadEnemy());
                // gameObject.SetActive(false);
            }
        }
    }

    IEnumerator PlayerCoroutine()
    {
        GetComponent<PlayerMovement>().enabled = false;
        GetComponent<Collider2D>().enabled = false;

        yield return new WaitForSeconds(0.5f);

        GameController.Instance.Canvas.GameOver.SetActive(true);
        GameController.Instance.GameOver = true;
        Time.timeScale = 0;
    }

    IEnumerator DeadEnemy()
    {
        GetComponent<Collider2D>().enabled = false;
        GameObject hpDropClone = Instantiate(hpDrop, this.transform);
        hpDropClone.transform.SetParent(null);

        GameObject staminaDropClone = Instantiate(staminaDrop, this.transform);
        staminaDropClone.transform.SetParent(null);

        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }

    

    IEnumerator HitRoutine()
    {
        if (!Hitted)
        {
            Hitted = true;
            yield return new WaitForSeconds(0.4f);
            Hitted = false;
        }
    }

    IEnumerator InvulnerableCoroutine()
    {
        invulnerable = true;
        yield return new WaitForSeconds(0.4f);
        invulnerable = false;

    }
    
    public void AddHp(float value)
    {
        currentHp += value;
        if (CurrentHp >= maxHp)
        {
            currentHp = maxHp;
        }
    }
    public void AddStamina(float value)
    {
        currentStamina += value;
        if (CurrentStamina >= maxStamina)
        {
            currentStamina = maxStamina;
        }
    }
}
