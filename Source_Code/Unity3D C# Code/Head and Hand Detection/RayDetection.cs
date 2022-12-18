using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayDetection : MonoBehaviour
{
    public string Hit = "0";
    Ray ray;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ray = new Ray(transform.position,transform.forward);
        bool isCollider = Physics.Raycast(ray, out RaycastHit hit);
        if(isCollider==true){
            Hit = hit.collider.gameObject.name;
        }
        
    }
}
