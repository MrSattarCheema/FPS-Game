using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealthController : MonoBehaviour
{
    public static PlayerHealthController Instance;
    public int maxHealth;
    public int currentHealth;
    private void Awake()
    {
        Instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        UiController.instance.healthSlider.maxValue = maxHealth;
        UiController.instance.healthSlider.value = currentHealth;
        UiController.instance.healthTxt.text = "Health: " + currentHealth * 100 / maxHealth + "%";
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void DemagePlayer(int demagePlayer)
    {
        currentHealth -= demagePlayer;
        if (currentHealth <= 0)
        {
            gameObject.SetActive(false);
            GameManager.Instance.loadCurrentScene();
        }
        UiController.instance.healthSlider.value = currentHealth;
        UiController.instance.healthTxt.text = "Health: " + currentHealth * 100 / maxHealth + "%";
    }
    public void IncreaseHealth(int amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
        UiController.instance.healthSlider.value = currentHealth;
        UiController.instance.healthTxt.text = "Health: " + currentHealth * 100 / maxHealth + "%";
    }
}
