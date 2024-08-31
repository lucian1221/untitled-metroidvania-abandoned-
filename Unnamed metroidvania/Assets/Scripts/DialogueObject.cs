using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Dialogue/DialogueObject")]
public class DialogueObject : ScriptableObject
{
    [SerializeField] [TextArea] private string[] dialogue;
    [SerializeField] private ResponseController[] responses;
    public string[] Dialogue => dialogue;
    public bool HasResponses => Responses != null && Responses.Length > 0;
    public ResponseController[] Responses => responses;
}
