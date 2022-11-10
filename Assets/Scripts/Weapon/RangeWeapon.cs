using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeWeapon : Weapon
{
    [SerializeField] private Transform firePoint;
    [SerializeField] private Camera mainCamera;

    protected float Spread { get; set; }
    protected float Penetration { get; set; }
    protected int NumberOfBulletsPerShot { get; set; }

    protected int MaxAmmo { get; set; }
    protected int CurrentAmmo { get; set; }
    protected float ReloadTime { get; set; }
    private bool isReloading = false;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        CurrentAmmo = MaxAmmo;

        effects += BulletHit;
    }

    protected void BulletHit(Enemy enemy)
    {
        enemy.TakeDamage(AttackDamage);
    }

    public override void Attack()
    {
        if (isReloading)
        {
            return;
        }

        if (Time.time < LastTimeAttack + FireRate)
        {
            return;
        }

        if (CurrentAmmo == 0)
        {
            StartCoroutine(Reload());
            return;
        }

        CurrentAmmo--;

        Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);

        for (int bullet = 0; bullet < NumberOfBulletsPerShot; bullet++)
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

            LastTimeAttack = Time.time;
        }
    }

    IEnumerator Reload()
    {
        isReloading = true;

        yield return new WaitForSeconds(ReloadTime);
        CurrentAmmo = MaxAmmo;

        isReloading = false;
    }
}
