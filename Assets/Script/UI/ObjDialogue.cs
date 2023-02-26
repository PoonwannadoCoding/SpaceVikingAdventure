using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjDialogue : MonoBehaviour
{
    private DialogueTrigger dialogueTrigger;

    // Start is called before the first frame update
    void Awake(){
        dialogueTrigger = GetComponent<DialogueTrigger>();
    }

    private void OnTriggerEnter2D(Collider2D collision){
      if(collision.transform.tag == "Player"){
         dialogueTrigger.TriggerDialogue();
      }
   }
}
