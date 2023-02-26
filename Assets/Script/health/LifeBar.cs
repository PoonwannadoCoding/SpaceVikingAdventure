using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LifeBar : MonoBehaviour
{
    [SerializeField] private Health playerHealth;
    [SerializeField] private Image totalLife;
    [SerializeField] private Image currentLife;

    

    // Start is called before the first frame update
    void Start()
    {
        totalLife.fillAmount = playerHealth.currentLife / 10;   
    }

    // Update is called once per frame
    void Update()
    {
        currentLife.fillAmount = playerHealth.currentLife / 10;   
    }
}
