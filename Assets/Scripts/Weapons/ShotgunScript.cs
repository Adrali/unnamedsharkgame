using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunScript : AWeapon
{
    private Vector3 spreadUp, spredDown;

    private void Awake()
    {
        Init();
        shootDelay = 1.2f;
        bulletSpeed = 20f;
        damages = 2;
        playerCombat = GetComponent<PlayerCombatScript>();
        bulletPrefab = playerCombat.getBulletPrefab();

        spreadUp = (Vector3.right + 0.2f * Vector3.up).normalized;
        spredDown = (Vector3.right + 0.2f * Vector3.down).normalized;
    }
    protected override void Shoot()
    {
        currentBullet = Instantiate(bulletPrefab);
        currentBullet.GetComponent<BulletScript>().Initialize(transform.position, Vector3.right * bulletDirection, bulletSpeed, 0.4f, damages);
        currentBullet = Instantiate(bulletPrefab);
        currentBullet.GetComponent<BulletScript>().Initialize(transform.position, spreadUp * bulletDirection, bulletSpeed, 0.4f, damages);
        currentBullet = Instantiate(bulletPrefab);
        currentBullet.GetComponent<BulletScript>().Initialize(transform.position, spredDown * bulletDirection, bulletSpeed, 0.4f, damages);
    }
}
