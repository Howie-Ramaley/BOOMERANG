using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    private Transform back1;
    private Transform back2;
    private Transform front1;
    private Transform cam;

    // Start is called before the first frame update
    void Start()
    {
        back1 = transform.Find("Back1");
        back2 = transform.Find("Back2");
        front1 = transform.Find("Front1");
        cam = GameObject.FindGameObjectWithTag("MainCamera").transform;
    }

    void LateUpdate()
    {
        if(back1 != null)
        {
            back1.position = new Vector3 (cam.position.x * 0.15F, cam.position.y * 0.05F, back1.position.z);
        }
        if(back2 != null)
        {
            back2.position = new Vector3 (cam.position.x * 0.3F, cam.position.y * 0.1F, back2.position.z);
        }
        if(front1 != null)
        {
            front1.position = new Vector3 (cam.position.x / -0.25F, front1.position.y, front1.position.z);
        }
    }
}
