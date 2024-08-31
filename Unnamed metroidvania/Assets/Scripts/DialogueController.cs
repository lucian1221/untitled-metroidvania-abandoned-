using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueController : MonoBehaviour
{
    [SerializeField] private TMP_Text textLabel;
    [SerializeField] private DialogueObject testDialogue;
    [SerializeField] private GameObject dialogueBox;
    //public string text = "y = mx + c\nHello World\ntiiiiii'); DROP TABLE Dialogue; --";
    private Typewriter typewriter;
    private ResponseHandler responseHandler;
    public bool isTalking;
    public PlayerController player;

    // Start is called before the first frame update
    void Start()
    {
        //CloseDialogue();
        typewriter = GetComponent<Typewriter>();
        CloseDialogue();
        //ShowDialogue(testDialogue);
        responseHandler = GetComponent<ResponseHandler>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowDialogue(DialogueObject dialogueObject)
    {
        dialogueBox.SetActive(true);
        isTalking = true;
        StartCoroutine(ContinueSpeaking(dialogueObject));
        player.rigidbody.velocity = new Vector2(0, 0);
    }

    private IEnumerator ContinueSpeaking(DialogueObject dialogueObject)
    {
        /*foreach(string dialogue in dialogueObject.Dialogue)
        {
            yield return typewriter.Run(dialogue, textLabel);
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.E));
        }*/
        for(int i = 0; i < dialogueObject.Dialogue.Length; i++)
        {
            string dialogue = dialogueObject.Dialogue[i];
            //yield return typewriter.Run(dialogue, textLabel);
            yield return Type(dialogue);
            textLabel.text = dialogue;
            if(i == dialogueObject.Dialogue.Length - 1 && dialogueObject.HasResponses)
            {
                break;
            }
            yield return null;
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.E));
        }
        if (dialogueObject.HasResponses)
        {
            responseHandler.ShowResponses(dialogueObject.Responses);
        }
        else
        {
            CloseDialogue();
        }
    }

    private IEnumerator Type(string dialogue)
    {
        typewriter.Run(dialogue, textLabel);
        while (typewriter.isSpeaking)
        {
            yield return null;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                typewriter.Stop();
            }
        }
    }

    private void CloseDialogue()
    {
        dialogueBox.SetActive(false);
        textLabel.text = "";
        isTalking = false;
    }
}
