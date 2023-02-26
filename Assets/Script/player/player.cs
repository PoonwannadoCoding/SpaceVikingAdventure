using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class player : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float jumpSpeed;
    [SerializeField] private float dashSpeed;
    [SerializeField] private int damage;
    [SerializeField] private float climbSpeed;
    [SerializeField] private float range;
    [SerializeField] private float colliderDistance;
    [SerializeField] private BoxCollider2D boxCollider;

    [SerializeField] private AudioClip attackSound;
    [SerializeField] private AudioClip jumpSound;
    [SerializeField] private AudioClip walkSound;
    [SerializeField] private AudioClip dashSound;
    [SerializeField] private AudioClip ladderSound;
    

    [SerializeField] private float attackCooldown;
    [SerializeField] private float jumpCooldown;
    [SerializeField] private float walkCooldown;
    [SerializeField] private float dashCooldown;
    [SerializeField] private float ladderCooldown;
    


    private float walkCooldownTimer = Mathf.Infinity;
    private float jumpCooldownTimer = Mathf.Infinity;
    private float dashCooldownTimer = Mathf.Infinity;
    private float attackCooldownTimer = Mathf.Infinity;
    private float ladderCooldownTimer = Mathf.Infinity;
    
    
    //public RigidbodyConstraints2D constraints;
    public float dashTimer;
    public float currentDashTimer = 0f;
    private Rigidbody2D body;
    Vector2 moveInput;
    Animator myAnimator;
    private bool facingRight;
    private Health playerHealth;
    
    private SpriteRenderer mySpriteRenderer;
    private spooky_enemy spooky;

    private DialogueTrigger dialogueTrigger;
    private DialogueTrigger2 dialogueTrigger2;
    private DialogueTrigger3 dialogueTrigger3;

    CapsuleCollider2D myBoxCollider;

    // public int maxHealth = 10;
    // public int currentHealth;
    float gravityStart;

    
    public float attackRange = 0.5f;

    public LayerMask enemyLayer;
    private enemy_health monsterHealth;


     private void Awake()
    {
        dialogueTrigger = GetComponent<DialogueTrigger>();
        dialogueTrigger2 = GetComponent<DialogueTrigger2>();
        dialogueTrigger3 = GetComponent<DialogueTrigger3>();



        spooky = GetComponent<spooky_enemy>();
        myBoxCollider = GetComponent<CapsuleCollider2D>();
        body = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        monsterHealth = GetComponent<enemy_health>();
        playerHealth = GetComponent<Health>();
        gravityStart = body.gravityScale;
        //currentHealth = maxHealth;
        
    }

    private void OnDrawGizmos(){
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance, 
        new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z));
    }


    
    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

   private bool EnemyInsight(){
        RaycastHit2D hit = Physics2D.BoxCast(boxCollider.bounds.center + transform.right * range * transform.localScale.x * colliderDistance, 
        new Vector3(boxCollider.bounds.size.x * range, boxCollider.bounds.size.y, boxCollider.bounds.size.z)
        , 0, Vector2.left, 0, enemyLayer);

        if(hit.collider != null){
            monsterHealth = hit.transform.GetComponent<enemy_health>();
        }

        return hit.collider != null;
   }

    private void DamageEnemy(){
        if(EnemyInsight()){
            monsterHealth.TakeDamage(damage);
        }
    }


    private void OnTriggerEnter2D(Collider2D collision){
      if(collision.transform.tag == "Void"){
         playerHealth.TakeDamage(999);
      } else if(collision.transform.tag == "Orb"){
         SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
      }
      else if(collision.transform.tag == "Ship"){
         SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
      }
      else if(collision.transform.tag == "Fire"){
         playerHealth.TakeDamage(1);
      }
      else if(collision.transform.tag == "Ship2"){
         SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
      }
      else if(collision.transform.tag == "dialogue"){

         dialogueTrigger.TriggerDialogue();
      }
      else if(collision.transform.tag == "dialogue2"){
         dialogueTrigger2.TriggerDialogue();
      }
      else if(collision.transform.tag == "dialogue3"){

         dialogueTrigger3.TriggerDialogue();

      }
   }

    void ClimbLadder()
    {
        if (!myBoxCollider.IsTouchingLayers(LayerMask.GetMask("ladder"))) {
            myAnimator.SetBool("isClimb", false);
            body.gravityScale = gravityStart;
            return; }
        Vector2 climbVelocity = new Vector2(body.velocity.x, moveInput.y * climbSpeed);
        body.velocity = climbVelocity;
        body.gravityScale = 0f;

        bool playerVerticalSpeed = Mathf.Abs(body.velocity.y) > Mathf.Epsilon;
        if(playerVerticalSpeed)
        {
            myAnimator.SetBool("isClimb", true);
            if(ladderCooldownTimer >= ladderCooldown){
                ladderCooldownTimer = 0;
                SoundManager.instance.PlatSound(ladderSound);

            }
        } else
        {
            myAnimator.SetBool("isClimb", false);
        }
    }

    
    void Run()
    {
        Vector2 playerVelocity = new Vector2(moveInput.x * speed, body.velocity.y);
        body.velocity = playerVelocity;
         if (body.velocity.x < 0 && !facingRight)
        {
            FlipSprite();
        }if (body.velocity.x > 0 && facingRight){

    
            FlipSprite();
        }

        bool playerHasHorizontalSpeed = Mathf.Abs(body.velocity.x) > Mathf.Epsilon;
        if(playerHasHorizontalSpeed)
        {
            myAnimator.SetBool("isMoving", true);
            if(walkCooldownTimer >= walkCooldown && Mathf.Abs(body.velocity.y) > Mathf.Epsilon){
                walkCooldownTimer = 0;
                SoundManager.instance.PlatSound(walkSound);
            }
        } else
        {
            myAnimator.SetBool("isMoving", false);
        }

    }

    

    void Update()
    {
        walkCooldownTimer += Time.deltaTime;
        jumpCooldownTimer += Time.deltaTime;
        dashCooldownTimer += Time.deltaTime;
        attackCooldownTimer += Time.deltaTime;
        ladderCooldownTimer += Time.deltaTime;
        

        Run();
        //FlipSprite();
        Jump();
        Fight();
        Dash();
        ClimbLadder();
        //body.constraints = RigidbodyConstraints2D.FreezeRotation;






    }
    void doDash(){
        if(dashCooldownTimer >= dashCooldown){
            dashCooldownTimer = 0;
            SoundManager.instance.PlatSound(dashSound);

        }
        myAnimator.SetBool("isDash", true);
        
        Vector2 playerVelocity = new Vector2(moveInput.x * dashSpeed, body.velocity.y);
        body.velocity = playerVelocity;
    }

    void Dash(){

        if (Time.time >= currentDashTimer)
        {
            if(Input.GetKey(KeyCode.E)){
                
                doDash();
                currentDashTimer = Time.time + dashTimer;
            
            }
            
            
        }
        else
        {
           
            myAnimator.SetBool("isDash", false);
        }


    }

    void FlipSprite()
    {
    
       
            Vector3 currentScale = gameObject.transform.localScale;
            currentScale.x *= -1;
            //transform.eulerAngles = new Vector3(0, 180, 0);
            gameObject.transform.localScale = currentScale;
            //mySpriteRenderer.flipX = true;
            facingRight = !facingRight;


      
    }

    void Jump()
    {

        if (!myBoxCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            return;
        }
        if (Input.GetKey(KeyCode.Space) && jumpCooldownTimer >= jumpCooldown)
        {
            jumpCooldownTimer = 0;
            body.velocity = new Vector2(body.velocity.x, jumpSpeed);
            myAnimator.SetBool("isJump", true);
            SoundManager.instance.PlatSound(jumpSound);
        } else
        {
            myAnimator.SetBool("isJump", false);
        }
    }


    void Fight()
    {
        if (!myBoxCollider.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            return;
        }
        if (Input.GetKey(KeyCode.Mouse0) && attackCooldownTimer >= attackCooldown)
        {
            attackCooldownTimer = 0;
            myAnimator.SetTrigger("isAttack");
            SoundManager.instance.PlatSound(attackSound);
            


        }
    

    }

    




}

