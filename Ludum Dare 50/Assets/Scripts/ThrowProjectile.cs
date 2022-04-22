using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowProjectile : MonoBehaviour
{
    [SerializeField] GameObject bullet;
    [SerializeField] float bulletSpeed;
    [SerializeField] float damage;
    bool isShooting;
    public bool IsShooting { get => isShooting; }

    public void Shoot()
    {
        if (!IsShooting)
        {
            StartCoroutine(ShootCoroutine());
        }
    }

    IEnumerator ShootCoroutine()
    {
        isShooting = true;

        GameObject bulletClone = Instantiate(bullet, transform);
        bulletClone.transform.SetParent(null);
        bulletClone.GetComponent<BulletScript>().Damage = damage;
        bulletClone.GetComponent<Rigidbody2D>().velocity = Vector2.zero;

        bulletClone.GetComponent<Rigidbody2D>().AddForce(this.transform.right * bulletSpeed);
        yield return new WaitForSeconds(2);

        Destroy(bulletClone.gameObject);
        isShooting = false;
    }
}
