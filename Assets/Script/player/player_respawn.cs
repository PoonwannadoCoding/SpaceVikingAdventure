using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class player_respawn : MonoBehaviour
{
   [SerializeField] private GameObject RED;
   private Transform currentCheckpoint;
   private Health playerHealth;
   private UIManager uiManager;

    [SerializeField] private AudioClip activeSound;
    [SerializeField] private float activeCooldown;
    private float activeCooldownTimer = Mathf.Infinity;



   private void Awake(){
    playerHealth = GetComponent<Health>();
    uiManager = FindObjectOfType<UIManager>();

   }
  
   private void Update()
   {
        activeCooldownTimer += Time.deltaTime;
      
   }

   

   public void CheckRespawn(){

      if (currentCheckpoint == null || playerHealth.isZero()){
         uiManager.GameOver();
         return ;
      }

    transform.position = currentCheckpoint.position;
    playerHealth.Respawn();
   }

   private void OnTriggerEnter2D(Collider2D collision){
      if(collision.transform.tag == "Checkpoint"){
         currentCheckpoint = collision.transform;
         collision.GetComponent<Collider2D>().enabled = false;
         collision.GetComponent<Animator>().SetBool("isActive",true);
         if(activeCooldownTimer >= activeCooldown){
            activeCooldownTimer = 0;
            SoundManager.instance.PlatSound(activeSound);
         }
      }
   }
}
