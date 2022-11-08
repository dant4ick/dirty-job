using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeWeapon : Weapon
{
    [SerializeField] private Transform firePoint;
    [SerializeField] private Camera mainCamera;

    protected float Spread { get; set; }
    protected float Penetration { get; set; }
    protected int NumberOfBullets { get; set; }

    // Start is called before the first frame update
    protected virtual void Start()
    {
        effects += BulletHit;
    }

    protected void BulletHit(Enemy enemy)
    {
        enemy.TakeDamage(AttackDamage);
    }

    public override void Attack()
    {
        Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);

        for (int bullet = 0; bullet < NumberOfBullets; bullet++)
        {
            Vector3 offset = new Vector2(Random.Range(0f, Spread), Random.Range(-Spread, Spread));
            RaycastHit2D hitInfo = Physics2D.Raycast(firePoint.position, mousePosition + offset);

            Debug.DrawRay(firePoint.position, mousePosition + offset, Color.black, 10f);

            if (hitInfo)
            {
                Enemy enemy = hitInfo.transform.GetComponent<Enemy>();

                if (enemy != null)
                {
                    effects(enemy);
                }
            }
        }
    }
}
