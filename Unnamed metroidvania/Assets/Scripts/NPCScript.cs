using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCScript : MonoBehaviour
{
    [SerializeField] private DialogueObject dialogue;
    [SerializeField] private DialogueController dialogueController;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            //Debug.Log("yes i see the player");

            if(Input.GetKeyDown(KeyCode.E) && collision.GetComponent<PlayerController>().isGrounded && !dialogueController.isTalking)
            {
                //Debug.Log("yes the player can talk to me");
                dialogueController.ShowDialogue(dialogue);
            }
        }
    }
}
