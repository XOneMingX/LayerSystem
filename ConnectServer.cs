
using UnityEngine;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using TMPro;

public class ConnectServer : MonoBehaviour
{
    string editString; // edit the box text

    Socket serverSocket; // the server of Socket
    IPAddress ip; // PC ip
    IPEndPoint ipEnd;

    internal string recvStr; //received message
    string sendStr; //sent message

    internal byte[] recvData = new byte[1024]; // the received message must be byte
    byte[] sendData = new byte[1024]; // the sent message must be byte

    int recvLen; // the length of received message
    Thread connectThread; // connection thread


    public TMP_InputField GetIP;
    public TMP_InputField GetPort;
    string IPaddress;
    int PortNum;

    void InitSocket()
    {
        // define the IP and port of the server. The port corresponds to the server
        ip = IPAddress.Parse(IPaddress); // can be a LAN or Internet IP, in this case local
        ipEnd = new IPEndPoint(ip, PortNum);

        // start a thread connection, necessary, otherwise the main thread is stuck
        connectThread = new Thread(new ThreadStart(SocketReceive));
        connectThread.Start();
    }

    void SocketConnect()
    {
        if (serverSocket != null)
            serverSocket.Close();
        // the socket type must be defined in the child thread
        serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        print("ready to connect");
        serverSocket.Connect(ipEnd); // connection

        // output the string received for the first connection
        recvLen = serverSocket.Receive(recvData);
        recvStr = Encoding.ASCII.GetString(recvData, 0, recvLen);
        print(recvStr);
    }

    public void SocketSend(string sendStr)
    {
        sendData = new byte[1024]; // clear the send cache
        sendData = Encoding.ASCII.GetBytes(sendStr); // data type conversion
        serverSocket.Send(sendData, sendData.Length, SocketFlags.None); //sent
    }

    public void SocketReceive()
    {
        SocketConnect();
        while (true) // continue to receive data from the server
        {
            recvData = new byte[1024];
            recvLen = serverSocket.Receive(recvData);
            if (recvLen == 0)
            {
                SocketConnect();
                continue;
            }
            recvStr = Encoding.ASCII.GetString(recvData, 0, recvLen);
            print(recvStr);
        }
    }

    void SocketQuit()
    {
        if (connectThread != null) // close the thread
        {
            connectThread.Interrupt();
            connectThread.Abort();
        }
        if (serverSocket != null) // close the server
            serverSocket.Close();
        print("diconnect");
    }

    public void StartConnect()
    {
        IPaddress = GetIP.text;
        PortNum = int.Parse(GetPort.text);
        InitSocket(); // activate the connection
    }

    public void TestConnect()
    {
        IPaddress = "127.0.0.1";
        PortNum = 21;
        InitSocket();
    }

    void OnApplicationQuit()
    {
        SocketQuit(); // when the program exits, the connection is closed
    }
}
