using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    private float startPos;
    [SerializeField] private GameObject cam;
    [SerializeField] private float parallax;
    [SerializeField] private Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        startPos = transform.position.x;
        cam = FindObjectOfType<Camera>().gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        float distance = cam.transform.position.x * parallax;
        transform.position = new Vector3(startPos + distance, transform.position.y, transform.position.z) + offset;
    }
}
