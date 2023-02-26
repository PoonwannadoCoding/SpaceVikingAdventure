using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spooky_patrol : MonoBehaviour
{
    [SerializeField] private Transform leftEdge;
    [SerializeField] private Transform rightEdge;
    [SerializeField] private Animator anim;
    [SerializeField] private Transform enemy;

    [SerializeField] private float speed;
    private Vector3 initScale;
    [SerializeField] private float idleDuration;
    private float idleTimer;

    private bool movingLeft;


    [SerializeField] private AudioClip walkSound;
    [SerializeField] private float walkCooldown;
    private float walkCooldownTimer = Mathf.Infinity;

    private void Awake(){
        initScale = enemy.localScale;
    }

    private void OnDisable()
    {
        anim.SetBool("spooky_walk", false);
    }

    private void MoveInDirection(int _direction){

        idleTimer = 0;

        anim.SetBool("spooky_walk", true);
        if(walkCooldownTimer >= walkCooldown){
            walkCooldownTimer = 0;
            SoundManager.instance.PlatSound(walkSound);
        }

        enemy.localScale = new Vector3(Mathf.Abs(initScale.x) * _direction,
            initScale.y, initScale.z);


        enemy.position = new Vector3(enemy.position.x + Time.deltaTime * _direction * speed,
        enemy.position.y, enemy.position.z);

    }

    private void Update(){

        walkCooldownTimer += Time.deltaTime;
        

        if (movingLeft)
        {
            if (enemy.position.x >= leftEdge.position.x)
                MoveInDirection(-1);
            else
                DirectionChange();
        }
        else
        {
            if (enemy.position.x <= rightEdge.position.x)
                MoveInDirection(1);
            else
                DirectionChange();
        }
    }

    private void DirectionChange()
        {
            anim.SetBool("spooky_walk", false);
            idleTimer += Time.deltaTime;

            if(idleTimer > idleDuration)
                movingLeft = !movingLeft;
        }


}
