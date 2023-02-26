using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spooky_enemy : MonoBehaviour
{
   [SerializeField] private float attackCooldown;
   [SerializeField] private int damage;
   [SerializeField] private BoxCollider2D boxCollider;
   [SerializeField] private LayerMask playerLayer;
   [SerializeField] private float range;
   [SerializeField] private float HP;
   [SerializeField] private float colliderDistance;
    private float attackCooldownTimer = Mathf.Infinity;

    private Animator anim;
    private Health playerHealth;

    private spooky_patrol enemyPatrol;
    [SerializeField] private AudioClip attackSound;
    
    
    private void Awake()
    {
        anim = GetComponent<Animator>();
        enemyPatrol = GetComponent<spooky_patrol>();
    }

    // Update is called once per frame
    private void Update()
    {
        attackCooldownTimer += Time.deltaTime;
        if(PlayerInsight()){
            if(attackCooldownTimer >= attackCooldown){
               
                attackCooldownTimer = 0;
                anim.SetTrigger("spooky_attack");
                SoundManager.instance.PlatSound(attackSound);
                

            } 
        }

        if(enemyPatrol != null)
            enemyPatrol.enabled = !PlayerInsight();
        
    }



    private bool PlayerInsight()
    {

        RaycastHit2D hit = Physics2D.BoxCast(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance, 
        new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z)
        , 0, Vector2.left, 0, playerLayer);

        if(hit.collider != null){
            playerHealth = hit.transform.GetComponent<Health>();
        }

        return hit.collider != null;
    }

    private void DamagePlayer(){
        if(PlayerInsight()){
            playerHealth.TakeDamage(damage);
        }
    }


    private void OnDrawGizmos(){
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance, 
        new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z));
    }

}
