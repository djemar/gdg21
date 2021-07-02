using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float moveSpeed = 10f;

    public float range = 10f;
    public float damage = 10f;

    public float blastRadius = 3f;
    public float explosionForce = 100f;

    private Vector3 initialPos;
    private float totalDistance;

    void Start(){
    }

    private Vector3 shootDir;
    public void Setup(Vector3 shootDir){
        Debug.Log("Setting up the bullet");
        this.shootDir = shootDir;
        this.initialPos = transform.position;
        this.totalDistance = 0;

    }

    // Update is called once per frame
    void Update()
    {
        transform.position +=  shootDir * moveSpeed * Time.deltaTime;

        totalDistance = (transform.position - initialPos).magnitude;

        if(totalDistance >= range){
            Destroy(gameObject);
        }


    }

    private void OnTriggerEnter(Collider collider){
        Debug.Log("Collided with object " + collider.name);

        Collider[] colliders = Physics.OverlapSphere(transform.position, blastRadius);

        if(colliders.Length == 0)
            Debug.Log("Empty array!");

        foreach(Collider nearbyObject in colliders){
            Enemy enemy = nearbyObject.GetComponent<Enemy>();
            if(enemy != null){
                Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
                if(rb != null){
                    rb.AddExplosionForce(explosionForce, transform.position, blastRadius);
                }
            }
        }

        colliders = Physics.OverlapSphere(transform.position, blastRadius/2);

        foreach(Collider nearbyObject in colliders){
            Enemy enemy = nearbyObject.GetComponent<Enemy>();
            if(enemy != null){
                enemy.TakeDamage();
            }
        }
        /*
        Target target = collider.GetComponent<Target>();
        if(target != null){
            target.Hit(damage);
        }
        */
        Destroy(gameObject);
    }
}
