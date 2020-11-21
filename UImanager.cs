using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UImanager : MonoBehaviour
{

    public static UImanager instance;

    public Slider MPBar;
    public Slider staminaBar;
    public ProgressBarPro HPBar;
    public float StaminaBarLerpSpeed,currentStamina,Maxstamina;


    private void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        currentStamina = PlayerstateController.instance.currentStamina;
        Maxstamina = PlayerstateController.instance.maxStamina;
        HPBar.SetValue(10f, 100f);
    }

    // Update is called once per frame
    void Update()
    {
        if(currentStamina != staminaBar.value)
        {
            staminaBar.value = Mathf.Lerp(staminaBar.value, currentStamina, Time.deltaTime * StaminaBarLerpSpeed);
        }


    }
}
