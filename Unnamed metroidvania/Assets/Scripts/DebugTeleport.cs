using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugTeleport : MonoBehaviour
{
    public GameObject player;
    public Transform location;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<PlayerController>().gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Minus))
        {
            player.transform.position = location.position;
        }
    }
}
