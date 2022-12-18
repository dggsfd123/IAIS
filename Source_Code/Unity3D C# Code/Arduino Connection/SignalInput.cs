using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using System;
using System.IO;
using System.Threading;
public class SignalInput : MonoBehaviour
{
    private SerialPort sp;
    private Thread receiveThread;  
    public string portName = "COM3";
    public int baudRate = 9600;
    public Parity parity = Parity.None;
    public int dataBits = 8;
    public StopBits stopBits = StopBits.One;
    public string strRec;

    void Start()
    {
        OpenPort();
        receiveThread = new Thread(ReceiveThreadfunction);
        receiveThread.IsBackground = true;
        receiveThread.Start();
    }
    // Update is called once per frame
    void Update()
    {
    }

    #region 
    public void OpenPort()
    {
        
        sp = new SerialPort(portName, baudRate, parity, dataBits, stopBits);
        sp.ReadTimeout = 400;
        try
        {
            sp.Open();
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
        }
    }
    #endregion
    #region 
    void OnApplicationQuit()
    {
        ClosePort();
    }

    public void ClosePort()
    {
        try
        {
            sp.Close();
            receiveThread.Abort();
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
        }
    }
    #endregion
    private void ReceiveThreadfunction()
    {
        while (true)
        {
            if (this.sp != null && this.sp.IsOpen)
            {
                try
                {
                    strRec = sp.ReadLine();            //SerialPort读取数据有多种方法，我这里根据需要使用了ReadLine()
                    //Debug.Log("Receive From Serial: " + strRec);
                }
                catch
                {
                    //continue;
                }
            }
        }
    }
}
