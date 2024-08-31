using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ResponseHandler : MonoBehaviour
{
    [SerializeField] private RectTransform responseBox;
    [SerializeField] private RectTransform responseButtonTemp;
    [SerializeField] private RectTransform responseContainer;
    private DialogueController dialogue;
    private List<GameObject> responseButtons = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        dialogue = GetComponent<DialogueController>();
        responseBox.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowResponses(ResponseController[] responses)
    {
        responseBox.gameObject.SetActive(true);
        float responseBoxWidth = 0;
        foreach(ResponseController response in responses)
        {
            GameObject responseButton = Instantiate(responseButtonTemp.gameObject, responseContainer);
            responseButton.gameObject.SetActive(true);
            responseButton.GetComponent<TMP_Text>().text = response.ResponseText;
            responseButton.GetComponent<Button>().onClick.AddListener(() => OnAnswer(response));
            responseBoxWidth += responseButtonTemp.sizeDelta.x;
            responseButtons.Add(responseButton);
        }
        responseBox.sizeDelta = new Vector2(responseBoxWidth, responseBox.sizeDelta.y);
        responseBox.gameObject.SetActive(true);
    }

    private void OnAnswer(ResponseController response)
    {
        responseBox.gameObject.SetActive(false);
        foreach(GameObject button in responseButtons)
        {
            Destroy(button);
        }
        responseButtons.Clear();
        dialogue.ShowDialogue(response.DialogueObject);
    }
}
