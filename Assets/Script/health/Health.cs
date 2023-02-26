using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
  [SerializeField] private GameObject RED;
  [SerializeField] private float startingHealth;
  [SerializeField] private Behaviour[] components;
  [SerializeField] private int life;
  public float currentLife{ get; private set;}
  public float currentHealth{ get; private set;}
  private bool dead;
  private Animator anim;

    [SerializeField] private AudioClip hurtSound;
    [SerializeField] private float hurtCooldown;
    private float hurtCooldownTimer = Mathf.Infinity;




  private void Awake(){
    currentHealth = startingHealth;
    currentLife = life;
    anim = GetComponent<Animator>();
  }

  public void freezePlayer(){
    foreach( Behaviour component in components){
              component.enabled = false;
            }
   }

   
   private void Update()
   {
        hurtCooldownTimer += Time.deltaTime;
    
   }


  public void Respawn(){
    AddHealth(startingHealth);
    anim.SetBool("isDead", false);
    anim.Play("Player_IdelDown");
    dead = false;
    RED.SetActive(true);
    foreach( Behaviour component in components){
              component.enabled = true;
            }
    
  }

  public bool isZero(){
    if(currentLife > 0){
      return false;
    } 
    return true;
  }

  public void AddHealth(float _hp){
    currentHealth = Mathf.Clamp(currentHealth + _hp, 0, startingHealth);
  }
  

  public void TakeDamage(float _damage){
    currentHealth = Mathf.Clamp(currentHealth - _damage, 0, startingHealth);
    if(currentHealth > 0){
        anim.SetTrigger("isHurt");
        if(hurtCooldownTimer >= hurtCooldown){
          SoundManager.instance.PlatSound(hurtSound);
        }

    } else {
        if(!dead && !isZero()){
            anim.SetBool("isDead", true);
            RED.SetActive(false);
            if(hurtCooldownTimer >= hurtCooldown){
              SoundManager.instance.PlatSound(hurtSound);
            }
            currentLife = Mathf.Clamp(currentLife - 1, 0, life);
            foreach( Behaviour component in components){
              component.enabled = false;
            }
            dead = true;
        }

    }
  }
}
