using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int currenHealth = 5;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void DamageEnemy(int demagePower)
    {
        currenHealth -= demagePower;
        if (currenHealth == 0)
        {
            Destroy(gameObject);
        }
    }
}
