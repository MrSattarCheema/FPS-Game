using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiController : MonoBehaviour
{
    public Slider healthSlider;
    public TextMeshProUGUI healthTxt, ammoTxt;
    public static UiController instance;
    public Image demageEffect;
    public float demageAlpha = .25f, demageFadeSpeed = 2f;
    // Start is called before the first frame update
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (demageEffect.color.a != 0)
        {
            demageEffect.color = new Color(demageEffect.color.r, demageEffect.color.g, demageEffect.color.b, Mathf.MoveTowards(demageEffect.color.a, 0f, demageFadeSpeed * Time.deltaTime));
        }
    }
    public void showDemage()
    {
        demageEffect.color = new Color(demageEffect.color.r, demageEffect.color.g, demageEffect.color.b, demageAlpha);
    }
}
