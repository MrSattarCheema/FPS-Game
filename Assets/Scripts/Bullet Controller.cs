using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    Rigidbody rb;
    public float movingSpeed, lifeTime;
    public ParticleSystem lasertParticleEffect;
    public int demagePower = 1;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = movingSpeed * transform.forward;
        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0)
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            other.gameObject.GetComponent<EnemyHealth>().DamageEnemy(demagePower);
        }
        Destroy(this.gameObject);
        Instantiate(lasertParticleEffect, transform.position + (transform.forward * (-movingSpeed * Time.deltaTime)), transform.rotation);
    }
}
