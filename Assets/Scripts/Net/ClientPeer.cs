using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

public class ClientPeer
{
    public Socket clientSocket;

    
    public ClientPeer()
    {
        try
        {
            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream,ProtocolType.Tcp);

        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }

    /**
     * 连接服务器
     */
    public void Connect(string ip, int port)
    {
        try
        {
            clientSocket.Connect(new IPEndPoint(IPAddress.Parse(ip),port));
            Debug.Log("连接服务器成功");
            StartReceive();
        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }

    /**
     * 数据缓存区
     */
    private byte[] receiveBuffer = new byte[1024];
    /**
     * 开始接收数据
     */
    public void StartReceive()
    {
        if (clientSocket == null && clientSocket.Connected == false)
        {
            Debug.Log("连接服务器成功");
            return;
        }

        clientSocket.BeginReceive(receiveBuffer, 0, 1024, SocketFlags.None, ReceiveCallBack, clientSocket);
    }

    /**
     * 开始接收完成后的回调
     */
    private void ReceiveCallBack(IAsyncResult ar)
    {
        int length = clientSocket.EndReceive(ar);
    }
}
