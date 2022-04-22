using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DashScript : MonoBehaviour
{

    Hp playerHp;

    public float dashSpeed;
    public float dashTime;
    public bool isDashing;
    public GameObject dashVfx;

    NavMeshAgent navmesh;
    Rigidbody2D rb;
    CharacterAnimation playerAnimation;
    PlayerMovement playerMovement;
    AttackScript attackScript;

    // Start is called before the first frame update
    void Start()
    {
        playerHp = GetComponent<Hp>();
        navmesh = GetComponent<NavMeshAgent>();
        playerAnimation = GetComponent<CharacterAnimation>();
        playerMovement = GetComponent<PlayerMovement>();
        attackScript = GetComponent<AttackScript>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!attackScript.IsAttacking)
        {
            if (!playerHp.Hitted)
            {
                Dash();
            }
        }
    }

    public void Dash()
    {
        if (isDashing)
        {
            navmesh.enabled = false;
            dashTime += Time.deltaTime;

            if (dashTime >= 0.45f)
            {
                rb.velocity = Vector2.zero;
                isDashing = false;
                dashTime = 0;
                navmesh.enabled = true;
                dashVfx.SetActive(false);
            }
        }

        if (Input.GetButton("Fire2"))
        {
            if (playerHp.CurrentStamina >= 15)
            {
                CheckSideDash();
            }
        }
    }

    void CheckSideDash()
    {
        if (!isDashing)
        {
            isDashing = true;
            playerHp.RemoveStamina(10);

            Vector3 rotationEuler = new Vector3(0, 0, 0);
            playerAnimation.CheckDashSide(playerMovement.Side);
            switch (playerMovement.Side)
            {
                case 0:
                    DashMovement(Vector2.up, rotationEuler, 90);
                    break;
                case 1:
                    DashMovement(Vector2.down, rotationEuler, -90);
                    break;
                case 2:
                    DashMovement(Vector2.left, rotationEuler, 180);
                    break;
                case 3:
                    DashMovement(Vector2.right, rotationEuler, 0);
                    break;
            }
        }
    }

    void DashMovement(Vector2 vector, Vector3 rotationEuler, int rotationZValue)
    {
        rb.AddForce(vector * dashSpeed);

        dashVfx.transform.position = this.transform.position - new Vector3(0, 0.1f, 0); ;

        rotationEuler = new Vector3(0, 0, rotationZValue);

        dashVfx.transform.rotation = Quaternion.Euler(rotationEuler);
        StartCoroutine(DashEnum(vector));
    }

    IEnumerator DashEnum(Vector2 vector)
    {
        yield return new WaitForSeconds(0.1f);
        dashVfx.SetActive(true);
        dashVfx.GetComponent<Rigidbody2D>().AddForce(vector * dashSpeed);
    }
}
