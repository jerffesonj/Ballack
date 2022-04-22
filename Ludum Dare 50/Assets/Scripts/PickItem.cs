using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickItem : MonoBehaviour
{
    [SerializeField] Type type;
    enum Type
    {
        Hp,
        Stamina
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if(type == Type.Hp)
            {
                collision.GetComponent<Hp>().AddHp(1);
            }
            if (type == Type.Stamina)
            {
                collision.GetComponent<Hp>().AddStamina(10);
            }

            Destroy(gameObject);
        }
    }
}
