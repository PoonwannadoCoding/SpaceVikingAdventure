using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
public class enemy_health : MonoBehaviour
{
    [SerializeField] private float startingHealth;
    public float currentHealth{ get; private set;}
    private bool dead;
    private Animator anim;
    [SerializeField] private Behaviour[] components;

    [SerializeField] private AudioClip monsterSound;
    [SerializeField] private float monsterCooldown;

    [SerializeField] private float deadCooldown;

    private float monsterCooldownTimer = Mathf.Infinity;
    private float deadCooldownTimer = Mathf.Infinity;



    


    private void Awake(){
    currentHealth = startingHealth;
    anim = GetComponent<Animator>();
  }

 
  private void Update()
  {
    monsterCooldownTimer += Time.deltaTime;
    deadCooldownTimer += Time.deltaTime;

  }

    public void TakeDamage(float _damage){
    currentHealth = Mathf.Clamp(currentHealth - _damage, 0, startingHealth);
    if(currentHealth > 0){

        anim.SetTrigger("spooky_hurt");
        if(monsterCooldownTimer >= monsterCooldown){
          monsterCooldownTimer = 0;
          SoundManager.instance.PlatSound(monsterSound);
        }
      

    } else {
        if(!dead){
          
            anim.SetBool("spooky_dead", true);
            if(deadCooldownTimer >= deadCooldown){
              deadCooldownTimer = 0;
              SoundManager.instance.PlatSound(monsterSound);
            }
            foreach( Behaviour component in components){
              component.enabled = false;
            }
            
            dead = true;
        }

    }
  }

  private void Deactivate(){
    gameObject.SetActive(false);
  }
}
