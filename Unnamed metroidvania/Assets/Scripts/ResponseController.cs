using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ResponseController
{
    [SerializeField] private string responseText;
    [SerializeField] private DialogueObject dialogueObject;

    public string ResponseText => responseText;
    public DialogueObject DialogueObject => dialogueObject;
}
