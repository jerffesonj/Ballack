using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Spine.Unity;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] GameObject passosVfx;
    [SerializeField] AudioClip passosSfx;

    Vector3 movement;
    
    Rigidbody2D rb;
    NavMeshAgent navmesh;
    AttackScript attackScript;
    Hp hp;
    DashScript playerDash;

    GameObject playerSprite;
    Coroutine passosCoroutine;
    int side = 1;

    public int Side { get => side; set => side = value; }

    void Start()
    {
        playerSprite = transform.GetChild(0).gameObject;

        Quaternion rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        playerSprite.transform.rotation = rotation;

        rb = GetComponent<Rigidbody2D>();
        hp = GetComponent<Hp>();
        attackScript = GetComponent<AttackScript>();
        playerDash = GetComponent<DashScript>();
        navmesh = GetComponent<NavMeshAgent>();
        navmesh.updateRotation = false;
        navmesh.updateUpAxis = false;
    }
    private void Update()
    {
        if (GameController.Instance.Paused)
        {
            return;
        }

        Quaternion rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        playerSprite.transform.rotation = rotation;

        if (!attackScript.IsAttacking)
        {
            if (!playerDash.isDashing)
            {
                attackScript.Attack();
            }
            Move();
        }
    }
    
    private void Move()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        movement = new Vector3(horizontal, vertical, 0);

        if (!playerDash.isDashing)
        {
            navmesh.SetDestination(movement + this.transform.position);

            if (passosCoroutine == null)
            {
                if (horizontal != 0 || vertical != 0)
                {
                    passosCoroutine = StartCoroutine(PassosVfx());
                }
            }
        }
    }

    IEnumerator PassosVfx()
    {
        yield return new WaitForSeconds(0.4f);
        passosVfx.transform.position = this.transform.position - new Vector3(0,0.5f,0);
        GameController.Instance.AudioSource.PlayOneShot(passosSfx,0.5f);
        passosVfx.SetActive(true);

        yield return new WaitForSeconds(0.2f);
        passosVfx.SetActive(false);
        passosCoroutine = null;
    }
}
