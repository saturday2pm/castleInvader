﻿using UnityEngine;
using ProtocolCS;
using WebSocketSharp;
using UnityEditor;

public class NetworkModule : MonoBehaviour
{
    public bool IsAlive
    {
        get
        {
            return webSocketClient.IsAlive;
        }
    }
    WebSocket webSocketClient;

    // Use this for initialization
    public void Connect(string url)
    {
        webSocketClient = new WebSocket(url);
        webSocketClient.ConnectAsync();

        webSocketClient.OnOpen += WsMatchMaking_OnOpen;
        webSocketClient.OnMessage += WsMatchMaking_OnMessage; ;
        webSocketClient.OnClose += WsMatchMaking_OnClose;    
    }

    public void Close()
    {
        webSocketClient.CloseAsync();
    }

    void Update()
    {
        PacketHelper.Flush();
    }

    public void Send<T>(T packet) where T : PacketBase
    {
        webSocketClient.SendAsync(Serializer.ToJson(packet),
            x => {
                if(x)
                    WsSend_OnCompleteSuccess(packet);
                else
                    WsSend_OnCompleteFail(packet);
            });
    }

    protected virtual void WsSend_OnCompleteSuccess<T>(T successPacket) where T : PacketBase
    {
        Debug.Log("Send Success : " + Serializer.ToJson(successPacket));
    }

    protected virtual void WsSend_OnCompleteFail<T>(T failedPacket) where T : PacketBase
    {
        Debug.Log("Send Failed : " + Serializer.ToJson(failedPacket));
    }

    protected virtual void WsMatchMaking_OnClose(object sender, CloseEventArgs e)
    {
    }

    protected virtual void WsMatchMaking_OnOpen(object sender, System.EventArgs e)
    {
    }

    private void WsMatchMaking_OnMessage(object sender, MessageEventArgs e)
    {
        Debug.Log("Recieve Message : " + e.Data.ToString());
        PacketHelper.PushPacket(e.Data);
    }
}

