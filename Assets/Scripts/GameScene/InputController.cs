using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Simulator;

public class InputController : MonoBehaviour {

    CastleController FromCastle;
    CastleController ToCastle;
    bool IsFirstClicked = false;

    static MapController MapController;
    // Use this for initialization
    void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
        if (IsFirstClicked)
        {
            if (!MapController)
            {
                var obj = GameObject.FindGameObjectWithTag("Map");
                if (obj)
                {
                    MapController = obj.GetComponent<MapController>();
                    if (!MapController)
                    {
                        Debug.Log("MapController is Missing!");
                    }
                }
            }

            Vector3 mousePos = Input.mousePosition;
            CastleController hoverCastle = MapController.GetCastleByWorldPos(mousePos);
            if (!hoverCastle)
            {
                if (ToCastle)
                {
                    ToCastle.DeactiveSelector();
                    ToCastle = null;
                }
            }
            else
            {
                if (ToCastle != hoverCastle)
                {
                    if(ToCastle)
                        ToCastle.DeactiveSelector();
                    if (hoverCastle == FromCastle)
                        return;
                    ToCastle = hoverCastle;
                    ToCastle.ActivateToSelector();
                }
            }
        }

	}

    public void OnCastleClick(CastleController castle) { 
    
        if(!castle)
            return;

        switch (IsFirstClicked)
        {
            case true:
                ToCastle = castle;
                //ToDo - Server로 공격정보 보내기
                // if(FromCastle != ToCastle)
                //      NetworkController.Attack(FirstSelectCastle.Id, SecondSelectCastle.Id);
                FromCastle.DeactiveSelector();
                ToCastle.DeactiveSelector();
                IsFirstClicked = false;
                FromCastle = null;
                ToCastle = null;
                break;
            case false:
                FromCastle = castle;
                FromCastle.ActivateFromSelector();
                IsFirstClicked = true;
                break;
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
