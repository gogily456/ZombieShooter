using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private void OnCollisionEnter(Collision objectWeHit)
    {
        if(objectWeHit.gameObject.CompareTag("Target"))
        {
            print("hit" + objectWeHit.gameObject.name + " !");

            createBulletImpactEffect(objectWeHit);

            Destroy(gameObject);

        }

        if (objectWeHit.gameObject.CompareTag("Wall"))
        {
            print("hit a wall");

            createBulletImpactEffect(objectWeHit);

            Destroy(gameObject);

        }


        if (objectWeHit.gameObject.CompareTag("Beer"))
        {
            print("hit beer bottle ");

            objectWeHit.gameObject.GetComponent<BeerBottle>().Shatter();

            //we will not destory the bullet on impact, it will get destoryed according to its life time 
            //for example: if we want to destory 2 bottles with 1 bullet 
        }
    }

    void createBulletImpactEffect(Collision objectWeHit)
    {
        ContactPoint contact = objectWeHit.contacts[0];

        GameObject hole = Instantiate(
            GlobalRefrences.Instance.bulletImpactEffectPrefab,
            contact.point,
            Quaternion.LookRotation(contact.normal)
            );

        hole.transform.SetParent(objectWeHit.gameObject.transform);
    }


}
