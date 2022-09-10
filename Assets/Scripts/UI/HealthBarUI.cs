using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    public GameObject healthPrefab;
    public Transform bloodPoint;
    public bool alwaysVisible;
    public float visibleTime;

    private float timeLimit;

    Image healthSlider;
    Transform UIBar;
    Transform camera;

    CharacterStats characterStats;
    private void Awake()
    {
        characterStats = GetComponent<CharacterStats>();
        characterStats.UpdateHealthBarOnAttack += UpdateHealthBar;
    }

    private void OnEnable()
    {
        camera = Camera.main.transform;

        //根据Canvas模式查找指定Canvas
        foreach(Canvas canvas in FindObjectsOfType<Canvas>())
        {
            if(canvas.renderMode == RenderMode.WorldSpace)
            {
                UIBar = Instantiate(healthPrefab, canvas.transform).transform;
                healthSlider = UIBar.GetChild(0).GetComponent<Image>();
                UIBar.gameObject.SetActive(alwaysVisible);
            }
        }
    }

    void Start()
    {

    }

    private void UpdateHealthBar(int currHealth, int maxHealth)
    {
        if(currHealth <= 0)
        {
            Destroy(UIBar.gameObject);
            return;
        }
        UIBar.gameObject.SetActive(true);
        float sliderPercent = (float)currHealth / maxHealth;
        healthSlider.fillAmount = sliderPercent;
        timeLimit = visibleTime;
    }

    void OnFreshObjPoint()
    {
        if(UIBar != null)
        {
            UIBar.position = bloodPoint.position;
            UIBar.forward = -camera.forward;
            if(timeLimit <= 0 && !alwaysVisible)
            {
                UIBar.gameObject.SetActive(false);
            }
            else
            {
                timeLimit -= Time.deltaTime;
            }
        }
    }

    private void LateUpdate()
    {
        OnFreshObjPoint();
    }

    void Update()
    {
        
    }
    
}
