using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public Camera playerCamera;

    //shooting
    public bool isShooting, readyToShoot;
    bool allowReset = true;
    public float shootingDelay = 2f;

    //burst mod 
    public int bullertsPerBuest = 3;
    public int burstBulletLeft;

    //spread
    public float spreadIntensity;

    //bullet
    public GameObject bulletPrebab;
    public Transform bulletSpawn;
    public float bulletVelocity = 30;
    public float bulletPrefabLifeTime = 3f;

    public enum ShootingMode
    {
        Single,
        Burst,
        Auto
    }

    public ShootingMode currentShootingMode;

    private void Awake()
    {
        readyToShoot = true;
        burstBulletLeft = bullertsPerBuest;
    }

    // Update is called once per frame
    void Update()
    {
        if(currentShootingMode == ShootingMode.Auto)
        {
            // holding down left mouse button
            isShooting = Input.GetKey(KeyCode.Mouse0);
        }
        else if(currentShootingMode == ShootingMode.Single || currentShootingMode == ShootingMode.Burst)
        {
            // click left mouse button once
            isShooting = Input.GetKeyDown(KeyCode.Mouse0);
        }
        
        if(readyToShoot && isShooting)
        {
            burstBulletLeft = bullertsPerBuest;
            fireWeapon();
        }
    }

    private void fireWeapon()
    {
        readyToShoot = false;

        Vector3 shootingDirection = calculateDirectionAndSpread().normalized;


        //Instantiate the bullet 
        GameObject bullet = Instantiate(bulletPrebab, bulletSpawn.position, Quaternion.identity);

        //posotion the bullet to face the shooting direction 
        bullet.transform.forward = shootingDirection;


        //shoot the bullet
        bullet.GetComponent<Rigidbody>().AddForce(shootingDirection *  bulletVelocity, ForceMode.Impulse);

        //destroy the bullet after some time 
        StartCoroutine(destoryBulletAfterTime(bullet, bulletPrefabLifeTime));

        //checking if we are done shooting 
        if(allowReset)
        {
            Invoke("ResetShot", shootingDelay);
            allowReset = false;
        }

        //burst Mode
        if(currentShootingMode == ShootingMode.Burst && burstBulletLeft > 1)
        {
            burstBulletLeft--;
            Invoke("FireWeapon", shootingDelay);
        }
        
    }

    private void ResetShot()
    {
        readyToShoot = true;
        allowReset = true;
    }

    public Vector3 calculateDirectionAndSpread()
    {
        //shooting from the middle of the screen to check where are we pointing at 
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        Vector3 targetPoint;
        if(Physics.Raycast(ray, out hit))
        {
            // hitting something
            targetPoint = hit.point;
        }
        else
        {
            //shooting at the air
            targetPoint = ray.GetPoint(100);
        }

        Vector3 direction = targetPoint - bulletSpawn.position;

        float x = UnityEngine.Random.Range(-spreadIntensity, spreadIntensity);
        float y = UnityEngine.Random.Range(-spreadIntensity, spreadIntensity);

        //returning the shooting direction and spread 
        return direction + new Vector3(x, y, 0);
    }

    private IEnumerator destoryBulletAfterTime(GameObject bullet, float bulletPrefabLifeTime)
    {
        yield return new WaitForSeconds(bulletPrefabLifeTime);
        Destroy(bullet);
    }
}
