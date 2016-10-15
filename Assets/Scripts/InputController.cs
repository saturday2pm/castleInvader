using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Simulator;

public class InputController : MonoBehaviour {

    CastleController FirstSelectCastle;
    CastleController SecondSelectCastle;
    bool IsSecondClick = false;

    // Use this for initialization
    void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
        
	}

    public void OnCastleClick(CastleController castle) { 
    
        if(!castle)
            return;

        switch (IsSecondClick)
        {
            case true:
                IsSecondClick = false;
                SecondSelectCastle = castle;
                //ToDo - Server로 공격정보 보내기
                // NetworkController.Attack(FirstSelectCastle.Id, SecondSelectCastle.Id);
                FirstSelectCastle.ActiveSelector();
                SecondSelectCastle.ActiveSelector();

                break;
            case false:
                IsSecondClick = true;
                FirstSelectCastle = castle;
                FirstSelectCastle.HoverSelector();
                break;
        }
    }

    public void OnBackgroundClick()
    {   
        FirstSelectCastle = null;
        SecondSelectCastle = null;
    }
}
