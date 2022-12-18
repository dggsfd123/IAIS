using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

public class AnimationControlScript : MonoBehaviour
{
    private Animator animator;
    public GameObject Foxie;
    public GameObject Cam;
    public String H;

    public string sig;
    private int k = 0;
    public float freezetime = 5;
    public float distance;
    private bool interact = false;
    
    Socket serverSocket; 
    Socket clientSocket; 
    IPEndPoint ipEnd; 
    string recvStr; 
    string sendStr; 
    byte[] recvData=new byte[1024]; 
    byte[] sendData=new byte[1024]; 
    int recvLen; 
    Thread connectThread; 


    void InitSocket()
    {

        ipEnd=new IPEndPoint(IPAddress.Any,5877);

        serverSocket=new Socket(AddressFamily.InterNetwork,SocketType.Stream,ProtocolType.Tcp);
        serverSocket.Bind(ipEnd);

        serverSocket.Listen(10);

        connectThread=new Thread(new ThreadStart(SocketReceive));
        connectThread.Start();
    }
 
    void SocketConnet()
    {
        if(clientSocket!=null)
            clientSocket.Close();

        print("Waiting for a client");

        clientSocket=serverSocket.Accept();
 
        IPEndPoint ipEndClient=(IPEndPoint)clientSocket.RemoteEndPoint;
  
        print("Connect with "+ipEndClient.Address.ToString()+":"+ipEndClient.Port.ToString());
    }
    void SocketSend(string sendStr)
    {

        sendData=new byte[1024];

        sendData=Encoding.ASCII.GetBytes(sendStr);

        clientSocket.Send(sendData,sendData.Length,SocketFlags.None);
    }

    void SocketReceive()
    {
  
        SocketConnet();      
   
        while(true)
        {
 
            recvData=new byte[1024];

            recvLen=clientSocket.Receive(recvData);

            if(recvLen==0)
            {
                SocketConnet();
                continue;
            }

            recvStr=Encoding.ASCII.GetString(recvData,0,recvLen);
            print(recvStr);
        }
    }

    void SocketQuit()
    {

        if(clientSocket!=null)
            clientSocket.Close();

        if(connectThread!=null)
        {
            connectThread.Interrupt();
            connectThread.Abort();
        }
 
        serverSocket.Close();
        print("diconnect");
    }








    // Start is called before the first frame update
    void Start()
    {
        InitSocket(); 
        animator = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        freezetime -= Time.deltaTime;
        sig = Foxie.GetComponent<SignalInput>().strRec;
        distance = Foxie.GetComponent<GetHandPosition>().HandDistance;
        H = Cam.GetComponent<RayDetection>().Hit;


        ///    Begin Interaction   /////
        if(interact==false&&(H=="1"||H=="2"))
        {
            interact = true;
            animator.SetBool("Interactive", false);
            Debug.Log("In Interactive Mode");
        }

        /// Move Modual to match the Line of Sight ///
        if(interact==true&&distance>=10)
        {
            if(H=="1"){
                SocketSend("11\n");
            }
            if(H=="2"){
                SocketSend("22\n");
            }
        }

        // For Debug
        // ///*************No Signal Input************///
        // if (Input.GetKeyDown(KeyCode.W) & freezetime<=0)
        // {
        //    animator.SetBool("Yes", false);
        //    Debug.Log("Yes");
        //    freezetime = 3;
        //    //RobotMove01
        //    SocketSend("yy\n");
        // }

        // if (Input.GetKeyDown(KeyCode.E) & freezetime<=0)
        // {
        //    animator.SetBool("No", false);
        //    Debug.Log("No");
        //    freezetime = 3;
        //    //RobotMove02
        //    SocketSend("nn\n");
        // }
        // ///*************No Signal Input************///



        ///  Passive Interaction  and   Active Interaction  ///////
        if(interact==true)
        {
            ///   Passive Interaction  ///
            if (sig == "a")  ////Touch Head
            {
                animator.SetBool("Yes", false);
                Debug.Log("Yes");
                freezetime = 3;
                //RobotMove01
                SocketSend("yy\n");
            }
            if (sig == "b")     ////Touch Body
            {
                animator.SetBool("No", false);
                Debug.Log("No");
                freezetime = 3;
                //RobotMove02
                SocketSend("nn\n");
            }


            ///   Active Interaction  ///
            if (distance < 10 & freezetime<=0)  ///Hand Position
            {
                animator.SetBool("Active", false);
                Debug.Log("Active");
                freezetime = 5;
                //RobotMove03
                SocketSend("jj\n");
            }
            if((H == "1"||H == "2") && freezetime <0)  ///Line of Sight Position
            {
                animator.SetBool("Yes", true);
                animator.SetBool("No", true);
                animator.SetBool("Active", true);
            }


            if (Input.GetKeyDown(KeyCode.H) & freezetime<=0)  ///Quit Interaction
            {
                //Exit
                SocketSend("ee\n");
            }
        }

    }
}
