using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetHandPosition : MonoBehaviour
{
    public float HandDistance;
    public GameObject Lhand;
    public GameObject Rhand;
    public GameObject Fox;


    private float lx,ly,lz,rx,ry,rz;
    private float ax,ay,az;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        lx = Lhand.transform.localPosition.x;
        ly = Lhand.transform.localPosition.y;
        lz = Lhand.transform.localPosition.z;
        rx = Rhand.transform.localPosition.x;
        ry = Rhand.transform.localPosition.y;
        rz = Rhand.transform.localPosition.z;
        ax = Fox.transform.localPosition.x;
        ay = Fox.transform.localPosition.y;
        az = Fox.transform.localPosition.z;
        HandDistance = Mathf.Min((lx-ax)*(lx-ax) +(ly-ay)*(ly-ay) +(lz-az)*(lz-az),(ax-rx)*(ax-rx) +(ay-ry)*(ay-ry) +(az-rz)*(az-rz));
    }
}
