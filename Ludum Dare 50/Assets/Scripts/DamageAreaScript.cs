using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageAreaScript : MonoBehaviour
{
    [SerializeField] GameObject owner;
    [SerializeField] float damage;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (owner.CompareTag("Player"))
        {
            if (collision.CompareTag("Enemy"))
            {
                collision.GetComponent<Hp>().RemoveHp(damage);
            }
        }
        else if (owner.CompareTag("Enemy"))
        {
            if (collision.CompareTag("Player"))
            {
                collision.GetComponent<Hp>().RemoveHp(damage);
            }
        }
    }
}

