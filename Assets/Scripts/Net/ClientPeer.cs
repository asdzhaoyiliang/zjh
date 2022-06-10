﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

public class ClientPeer
{
    public Socket clientSocket;
    public NetMsg msg;

    public ClientPeer()
    {
        try
        {
            msg = new NetMsg();
            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }

    /**
     * 连接服务器
     */
    public void Connect(string ip, int port)
    {
        try
        {
            clientSocket.Connect(new IPEndPoint(IPAddress.Parse(ip), port));
            Debug.Log("连接服务器成功");
            StartReceive();
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }

    #region 接受数据

    /**
     * 数据缓存区
     */
    private byte[] receiveBuffer = new byte[1024];

    /**
     * 数据缓存
     */
    private List<byte> receiveCache = new List<byte>();

    /**
     * 是否正在处理接受到的数据
     */
    private bool isProcessingReceive = false;

    /**
     * 存放消息队列
     */
    public Queue<NetMsg> netMsgQueue = new Queue<NetMsg>();

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
        byte[] data = new byte[length];
        Buffer.BlockCopy(receiveBuffer, 0, data, 0, length);
        receiveCache.AddRange(data);
        if (!isProcessingReceive)
        {
            ProcessReceive();
        }

        StartReceive();
    }


    /**
     * 处理接收到的数据
     */
    private void ProcessReceive()
    {
        isProcessingReceive = true;
        byte[] packet = EncodeTool.DecodePacket(ref receiveCache);
        if (packet == null)
        {
            isProcessingReceive = false;
            return;
        }

        NetMsg msg = EncodeTool.DecodeMsg(packet);
        netMsgQueue.Enqueue(msg);
        ProcessReceive();
    }

    #endregion

    #region 发送消息

    /**
     * 发送消息
     */
    public void SendMsg(int opCode, int subCode, object value)
    {
        msg.Change(opCode, subCode, value);
        SendMsg(msg);
    }

    public void SendMsg(NetMsg msg)
    {
        try
        {
            byte[] data = EncodeTool.EncodeMsg(msg);
            byte[] packet = EncodeTool.EncodePacket(data);
            clientSocket.Send(packet);
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }

    #endregion

}
