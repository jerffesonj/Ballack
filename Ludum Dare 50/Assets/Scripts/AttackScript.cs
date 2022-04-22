using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackScript : MonoBehaviour
{
    [SerializeField] List<Transform> locations;

    [SerializeField] GameObject damageArea;
    [SerializeField] float damageValue;
    [SerializeField] AudioClip swordSound;

    AudioSource audioSource;
    Transform locationParent;
    CharacterAnimation playerAnimation;
    PlayerMovement playerMovement;
    Hp playerHp;
    bool isAttacking;

    public bool IsAttacking { get => isAttacking; }

    private void Start()
    {
        audioSource = GameController.Instance.AudioSource;
        locationParent = locations[0].transform.parent;
        playerHp = GetComponent<Hp>();
        playerAnimation = GetComponent<CharacterAnimation>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    void Update()
    {
        Quaternion rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        locationParent.rotation = rotation;
    }
   
    IEnumerator AttackCoroutine(int index)
    {
        audioSource.PlayOneShot(swordSound);
        
        damageArea.transform.SetParent(locations[index]);

        damageArea.transform.localPosition = Vector2.zero;

        isAttacking = true;
        damageArea.SetActive(true);
        yield return new WaitForSeconds(0.4f);
        damageArea.SetActive(false);

        isAttacking = false;
    }

    public void Attack(int index)
    {
        StartCoroutine(AttackCoroutine(index));
    }

    public void Attack()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            if (playerHp.CurrentStamina >= 10)
            {
                playerHp.RemoveStamina(10);
                playerAnimation.CheckAttackAnimSide();

                Attack(playerMovement.Side);
            }
        }
    }
}
