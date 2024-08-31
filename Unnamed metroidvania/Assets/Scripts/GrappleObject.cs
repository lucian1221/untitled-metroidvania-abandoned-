using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GrappleObject : MonoBehaviour
{
    public bool canGrapple;
    public Color defaultColor;

    // Start is called before the first frame update
    void Start()
    {
        defaultColor = GetComponent<SpriteRenderer>().color;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseOver()
    {
        if(PlayerController.canGrapple == false)
        {
            return;
        }
        canGrapple = true;
        GetComponent<SpriteRenderer>().color = Color.yellow;
        FindObjectOfType<PlayerController>().grappleObject = this;
    }

    private void OnMouseExit()
    {
        canGrapple = false;
        GetComponent<SpriteRenderer>().color = defaultColor;
    }
}
