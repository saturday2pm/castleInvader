using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Simulator;


public class InputController : MonoBehaviour {

    CastleController FromCastle;
    CastleController ToCastle;
    CastleController HoverCastle;
    bool IsFirstClicked = false;

    public delegate void MoveDelegate(int src, int dst);
    public delegate void UpgradeDelegate(int target);

    public MoveDelegate OnMove { get; set; }
    public UpgradeDelegate OnUpgrade { get; set; }

    public LineRenderer Indicator;
    static MapController MapController;

    Vector3 DragSrc;
    Vector3 DragDst;
    bool IsDraging = false;

    // Use this for initialization

    void Start () {
    }

    // Update is called once per frame
    void Update () {

	}

    public void OnCastleClick(CastleController castle)
    {
        if (!castle || IsDraging)
            return;

        Indicator.SetWidth(0.1f, 0.1f);
        DragSrc = castle.transform.localPosition;
        DragDst = castle.transform.localPosition;
        FromCastle = castle;
        FromCastle.ActivateFromSelector();
        IsDraging = true;
    }

    public void OnDrag(Vector2 delta)
    {
        if (!IsDraging)
            return;

        DragDst += new Vector3(delta.x, delta.y, -1);
        Indicator.SetPosition(0, DragSrc);
        Indicator.SetPosition(1, DragDst);
    }

    public void OnDrop(CastleController castle)
    {
        if (!IsDraging)
            return;

        if (castle)
        {
            ToCastle = castle;
            OnMove(FromCastle.Id, ToCastle.Id);

            FromCastle.DeactiveSelector();
            ToCastle.DeactiveSelector();

            IsFirstClicked = false;
            FromCastle = null;
            ToCastle = null;
        }

        DragSrc = Vector3.zero;
        DragDst = Vector3.zero;

        Indicator.SetPosition(0, DragSrc);
        Indicator.SetPosition(1, DragDst);
        IsDraging = false;
    }

    public void OnCastleUpgrade(CastleController castle)
    {
        OnUpgrade(castle.Id);
    }

    public void OnCastleHover(CastleController castle)
    {
        if (!IsDraging || FromCastle == castle)
            return;

        if(castle)
        {
            HoverCastle = castle;
            HoverCastle.ActivateToSelector();
        }
        else if(HoverCastle)
        {
            HoverCastle.DeactiveSelector();
            HoverCastle = null;
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

        if (IsDraging)
        {
            DragSrc = Vector3.zero;
            DragDst = Vector3.zero;

            Indicator.SetPosition(0, DragSrc);
            Indicator.SetPosition(1, DragDst);
            IsDraging = false;
        }
    }
}

