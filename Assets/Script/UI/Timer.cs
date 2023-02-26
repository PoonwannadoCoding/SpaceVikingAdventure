using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Timer : MonoBehaviour
{
    [SerializeField] private GameObject RED;
    float currentTime;
    public float startingTime = 10f;
    
    [SerializeField] Text countdownText;
    private Health playerHealth;

   
    void Start()
    {
        currentTime = startingTime;  
        playerHealth = GetComponent<Health>();

    }

    // Update is called once per frame
    void Update()
    {
        currentTime -= 1* Time.deltaTime;
        countdownText.text = currentTime.ToString("0");
        if(currentTime <= 0){
            currentTime = 0;
            RED.SetActive(false);
            playerHealth.TakeDamage(999);

        }
    }
}
