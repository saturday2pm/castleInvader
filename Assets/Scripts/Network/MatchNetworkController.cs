﻿using ProtocolCS;
using ProtocolCS.Constants;
using System;

class MatchNetworkController : NetworkController
{
    public static int CurrentPlayerId { get; set; }
    public static MatchSuccess LastSuccessMatch { get; set; }
    public void Init()
    {
        CurrentPlayerId = new Random().Next(1000);
        LastSuccessMatch = null;
        Init("ws://localhost/mmaker?version=" + ProtocolVersion.version);
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

    private void OnMatchSuccess(MatchSuccess matchData)
    {
        LastSuccessMatch = matchData;
        //Do Something For GameStart
        Close();
    }
}
