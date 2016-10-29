using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Simulator;


public class InputController : MonoBehaviour {

    CastleController FromCastle;
    CastleController ToCastle;
    bool IsFirstClicked = false;

    public delegate void MoveDelegate(int src, int dst);
    public delegate void UpgradeDelegate(int target);

    public MoveDelegate OnMove { get; set; }
    public UpgradeDelegate OnUpgrade { get; set; }


    static MapController MapController;
    // Use this for initialization
    void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
	}

    public void OnCastleClick(CastleController castle) { 
    
        if(!castle)
            return;

        switch (IsFirstClicked)
        {
            case true:
                ToCastle = castle;
                if (OnMove != null)
                    OnMove(FromCastle.Id, ToCastle.Id);
                FromCastle.DeactiveSelector();
                ToCastle.DeactiveSelector();
                IsFirstClicked = false;
                FromCastle = null;
                ToCastle = null;
                break;
            case false:
                //분기 : 첫번째 클릭한 캐슬이 사용자의 성인지
                FromCastle = castle;
                FromCastle.ActivateFromSelector();
                IsFirstClicked = true;
                break;
        }
    }

    public void OnCastleUpgrade(CastleController castle)
    {
        OnUpgrade(castle.Id);
    }

    public void OnCastleHover(bool isHover, CastleController castle)
    {
        if (!IsFirstClicked || FromCastle == castle)
            return;

        if(isHover)
        {
            castle.ActivateToSelector();
        }
        else
        {
            castle.DeactiveSelector();
        }
    }

public void OnBackgroundClick()
    {
        IsFirstClicked = false;
        if(FromCastle)
            FromCastle.DeactiveSelector();
        if (ToCastle)
            ToCastle.DeactiveSelector();

        FromCastle = null;
        ToCastle = null;
    }
}
