using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    Rigidbody rb;
    public float movingSpeed, lifeTime;
    public ParticleSystem lasertParticleEffect;
    public int demagePower = 1;
    public bool demageEnemy, demagePlayer;
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
        if (other.gameObject.tag == "Enemy" && demageEnemy)
        {
            other.gameObject.GetComponent<EnemyHealth>().DamageEnemy(demagePower);
        }
        if (other.gameObject.tag == "Player" && demagePlayer)
        {
            //Destroy(other.gameObject);
        }
        if ((other.tag == "Player" && demageEnemy) || (other.tag == "Enemy" && demagePlayer))
        {
            return;
        }
        Destroy(this.gameObject);
        Instantiate(lasertParticleEffect, transform.position + (transform.forward * (-movingSpeed * Time.deltaTime)), transform.rotation);
    }
}
