using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public GameObject bullet;
    public bool canAutoFire;
    public float fireRate;
    [HideInInspector] public float fireCounter;
    public int ammoAmount;
    // Start is called before the first frame update
    void Start()
    {
        UiController.instance.ammoTxt.text = "Ammo: " + ammoAmount;
    }

    // Update is called once per frame
    void Update()
    {
        if (fireCounter > 0)
        {
            fireCounter -= Time.deltaTime;
        }
    }
}
