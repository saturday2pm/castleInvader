using UnityEngine;
using ProtocolCS;
using WebSocketSharp;
using System;

public class NetworkModule : MonoBehaviour
{
    public bool IsAlive
    {
        get
        {
            return webSocketClient.IsAlive;
        }
    }

    PacketHelper packetHelper = new PacketHelper();
    WebSocket webSocketClient;

    void Start()
    {
        MainThreadDispatcher.Init();
        OnStart();
    }

    // Use this for initialization
    public void Connect(string url)
    {
        Debug.Log("WSConnect - " + url);

        webSocketClient = new WebSocket(url);
        webSocketClient.ConnectAsync();

        webSocketClient.OnOpen += Ws_OnOpen;
        webSocketClient.OnMessage += Ws_OnMessage; ;
        webSocketClient.OnClose += Ws_OnClose;
        webSocketClient.OnError += WebSocketClient_OnError;
    }

    public void Close()
    {
        webSocketClient.CloseAsync();
    }

    void Update()
    {
        packetHelper.Flush();
    }

    public void Send<T>(T packet) where T : PacketBase
    {
        webSocketClient.SendAsync(Serializer.ToJson(packet),
            x =>
            {
                    MainThreadDispatcher.Queue(() =>
                    {
                        if (x)
                            OnSendSuccess(packet);
                        else
                            OnSendFail(packet);
                    });
            });
    }

    public void AddHandler<T>(Action<T> func) where T : PacketBase
    {
        packetHelper.AddHandler(func);
    }

    protected virtual void OnStart()
    {

    }

    protected virtual void OnSendSuccess<T>(T successPacket) where T : PacketBase
    {
        Debug.Log("Send Success : " + Serializer.ToJson(successPacket));
    }

    protected virtual void OnSendFail<T>(T failedPacket) where T : PacketBase
    {
        //Debug.Log("Send Failed : " + Serializer.ToJson(failedPacket));
    }

    protected virtual void OnClose()
    {
        Debug.Log("On Close");

    }

    protected virtual void OnOpen()
    {
        Debug.Log("On Open");
    }

    private void OnMessage(string message)
    {
        //Debug.Log("Recieve Message : " + message);
        packetHelper.PushPacket(message);
    }

    private void Ws_OnOpen(object sender, EventArgs e)
    {
        Debug.Log("On Open--");
        MainThreadDispatcher.Queue(OnOpen);
    }

    private void Ws_OnClose(object sender, EventArgs e)
    {
        Debug.Log("On Close--" + e.ToString());
        MainThreadDispatcher.Queue(OnClose);
    }

    private void Ws_OnMessage(object sender, MessageEventArgs e)
    {
        MainThreadDispatcher.Queue(()=> 
        {
            OnMessage(e.Data.ToString());
        });
    }

    private void WebSocketClient_OnError(object sender, ErrorEventArgs e)
    {
        Debug.LogError("Error : " + e.Message);
    }
}

