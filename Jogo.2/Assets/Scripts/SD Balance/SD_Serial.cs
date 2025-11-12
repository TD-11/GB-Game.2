using UnityEngine;
using System.Collections;
using System.IO.Ports;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Threading;
using UnityEngine.UIElements;

public class SD_Serial : MonoBehaviour
{
    private static SerialPort mySerialPort;

    public char[] test_string;
    
    public string SerialCOM = "COM1";
    
    private string mensage = "";

    private string State;

    public bool _START = false;

    public float A = 0;
    public float B = 0;
    public float C = 0;
    public float D = 0;
    public float P = 0;
    
    void Start()
    {
        foreach (string str in SerialPort.GetPortNames())
        {
            Debug.Log(string.Format("COM port: {0}", str));
        }

        if (SerialPort.GetPortNames().Length > 0)
        {
            SerialCOM = SerialPort.GetPortNames()[0];
        }
        
        Thread thread = new Thread(Run);  
        thread.Start(); 
    }

    void FixedUpdate()
    {

    }

    void Awake()
    {

    }

 
    
    void Update()
    {

    }

    void Run()  
    {  
        while (true)  
        {  
           
        if (_START == true)
        {
            try
            {
                char _c = uart_getc();
                mensage += _c.ToString();
                
                if ( _c == '\n')
                {
                   // mensage = "*1000.00;1000.00;1000.00;1000.00;1000.00";
                   
                    if (mensage.ToCharArray()[0]=='*')
                    {
                        mensage = mensage.Remove(0, 1);
                        
                        Debug.Log( mensage );
                        
                        string[] partes = mensage.Split(';');
                        string a = partes[0];
                        string b = partes[1];
                        string c = partes[2];
                        string d = partes[3];
                        string p = partes[4];
                    
                        Debug.Log("A=> "+ a);
                        Debug.Log("B=> "+ b);
                        Debug.Log("C=> "+ c);
                        Debug.Log("D=> "+ d);
                        Debug.Log("P=> "+ p);
                        
                        
                        A = float.Parse(a.Replace('.',','));
                        B = float.Parse(b.Replace('.',','));
                        C = float.Parse(c.Replace('.',','));
                        D = float.Parse(d.Replace('.',','));
                        P = float.Parse(p.Replace('.',','));

                        mySerialPort.DiscardInBuffer();
                        mySerialPort.DiscardOutBuffer();
                        
                        State = "Data Received: "+ mensage;
                    }
                    mensage = ""; 
                } 

            }
            catch (Exception e)
            {
                Exception rootCause = e.GetBaseException();
                if (rootCause.GetType() == typeof(TimeoutException) )
                {
                    State = "Data Loading: Timeout";
                    Debug.Log(State);
                }
                else
                {
                    State = "Data Received Erro: " + e.Message;
                    Debug.Log(State);
                }
            }

        }
        }  
    }
    
    private static void DataReceviedHandler(object sender, SerialDataReceivedEventArgs e)
    {
        mySerialPort = (SerialPort)sender; // It never gets here!
        string indata = mySerialPort.ReadExisting();
        Debug.Log("Data Received: 1");
        Debug.Log(indata);
    }

    void OnGUI() // simple GUI
    {
        int textPosition_y = 60;
        int textPosition_x = 10;

        
        if (SerialPort.GetPortNames().Length > 0)
        {
            foreach (string str in SerialPort.GetPortNames())
            {
                GUI.Label(new Rect(textPosition_x, textPosition_y, 1000, 20), string.Format("COM port: {0}", str));
                textPosition_y += 20;
            }
        }

        SerialCOM = GUI.TextField(new Rect(120, 10, 80, 20), SerialCOM);

        if (_START == false)
            if (GUI.Button(new Rect(10, 10, 100, 50), "ON"))
            {
                _START = true;

                try
                {
                    mySerialPort = new SerialPort(SerialCOM, 57600);
                    mySerialPort.Parity = Parity.None;
                    mySerialPort.StopBits = StopBits.One;
                    mySerialPort.DataBits = 8;
                    mySerialPort.Handshake = Handshake.None;
                    mySerialPort.ReadBufferSize = 512;
                    mySerialPort.WriteBufferSize = 512;
                    mySerialPort.ReadTimeout = 100;
                    mySerialPort.WriteTimeout = 100;

                    mySerialPort.Open();

                    State = "Door open successfully.";
                    Debug.Log(State);
                }
                catch (Exception e)
                {
                    State = "Error opening port: "+ e.Message;
                    Debug.Log(State);
                }

            }
        if (_START == true)
            if (GUI.Button(new Rect(10, 10, 100, 50), "OFF"))
            {
                _START = false;

                if (mySerialPort.IsOpen)
                {
                    mySerialPort.Close();
                }
            }
        
        GUI.Label(new Rect(240, 10, 1000, 1000), State);
    }

    private void uart_TX_CW_puts(char[] TX_command_word)
    {
        for (int i = 0; i < Convert.ToString(TX_command_word).Length; i++)
        {
            uart_putc(TX_command_word[i]);		//Advance though string till end
        }
    }

    //uart_gets
    private char[] uart_gets(int n)
    {
        int i = 0;

        char[] Array = new char[n];

        while (i < n)					//Grab data till the array fills
        {
            Array[i] = (char)mySerialPort.ReadByte();//uart_getc();
            Thread.Sleep(5);
            i++;
        }
        return Array;
    }

    //uart_getc
    private char uart_getc()
    {
        char rx_char = (char)mySerialPort.ReadByte();
        return rx_char;
    }

    //uart_putc
    private void uart_putc(char tx_char)
    {
        mySerialPort.Write(Convert.ToString(tx_char));
    }

    //uart_puts
    private void uart_puts(string str)				//Sends a String to the UART.
    {
        char[] c = str.ToCharArray();

        for (int i = 0; i < str.Length; i++)
        {
            uart_putc(c[i]);		//Advance though string till end
        }
    }


    void OnApplicationQuit()
    {
        if (mySerialPort.IsOpen)
        {
            mySerialPort.Close();
        }
    }
}