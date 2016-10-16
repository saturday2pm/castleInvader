﻿using ProtocolCS;
using ProtocolCS.Constants;
using System;
using System.Threading;
using UnityEngine.SceneManagement;

public class MatchModule : NetworkModule
{
    public static int CurrentPlayerId { get; set; }
    public static MatchSuccess LastSuccessMatch { get; set; }
    public delegate void ConnectionChanged();
    public ConnectionChanged OnConnected { get; set; }

    int connectCount = 0;

    void Start()
    {
        CurrentPlayerId = new Random().Next(1000);
        LastSuccessMatch = null;
        Connect("ws://localhost:9916/mmaker?version=" + ProtocolVersion.version);
        PacketHelper.AddHandler<MatchSuccess>(OnMatchSuccess);
    }

    public bool RequestMatch()
    {
        if (!IsAlive)
            return false;

        JoinQueue matchRequest = new JoinQueue() { senderId = CurrentPlayerId };
        Send(matchRequest);

        return true;
    }

    public bool CancelMatchRequest()
    {
        if (!IsAlive)
            return false;

        LeaveQueue cancelRequest = new LeaveQueue() { senderId = CurrentPlayerId };
        Send(cancelRequest);
        return true;
    }

    public bool RequestBotMatch()
    {
        if (!IsAlive)
            return false;

        JoinBotQueue matchRequest = new JoinBotQueue() { senderId = CurrentPlayerId };
        Send(matchRequest);
        return true;
    }

    protected override void OnOpen(object sender, EventArgs e)
    {
        base.OnOpen(sender, e);
        OnConnected();
    }

    private void OnMatchSuccess(MatchSuccess matchData)
    {
        LastSuccessMatch = matchData;
        Close();
        SceneManager.LoadScene("GameScene");
    }
}
