using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    [SerializeField] float damage;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<Hp>().RemoveHp(damage);
            Destroy(gameObject);
        }
        if (collision.CompareTag("Walls"))
        {
            Destroy(gameObject);
        }
    }

    public float Damage { set => damage = value; }
}
