﻿using UnityEngine;
using System.Collections;
using System.Linq;

public class CastleController : MonoBehaviour
{
    public UILabel UnitCountLabel;
    public UI2DSprite CastleSprite;
    public UI2DSprite SelectorSprite;
    public UILabel UpgradeButton;

    public Sprite FromSelectorSprite;
    public Sprite ToSelectorSprite;

    static InputController InputController;

    //int id = -1;
    public int Id; //{ get { return id; } }
    public bool IsUserCastle { get; set; }

    public bool Init(int _id, Vector2 _position, int _unitCount, float _size, Color _color, bool _isUserCastle)
    {
        if (InputController == null)
        {
            InputController = GameObject.FindObjectOfType<InputController>();
        }

        var button = SelectorSprite.GetComponent<UIButton>();

        Id = _id;
        transform.localPosition = new Vector3(_position.x, _position.y, 0);
        UpdateCastle(_unitCount, _size, _color, false, _isUserCastle);

        return true;
    }

    public void UpdateCastle(int _unitCount, float _size, Color _color, bool _isUpgradable, bool _isUserCastle)
    {
        CastleSprite.color = _color;
        IsUserCastle = _isUserCastle;

        int rectSize = (int)(_size * 2.0f);
        CastleSprite.width = rectSize;
        CastleSprite.height = rectSize;
        SelectorSprite.width = rectSize;
        SelectorSprite.height = rectSize;

        UnitCountLabel.text = _unitCount.ToString();
        float labelYPos = (CastleSprite.height + UnitCountLabel.height) * 0.5f;
        UnitCountLabel.transform.localPosition = new Vector3(0, labelYPos, 0);
        UpgradeButton.transform.localPosition = new Vector3(0, labelYPos - UnitCountLabel.height - CastleSprite.height, 0);

        if (_isUpgradable && _isUserCastle)
            ShowUpgradeButton();
        else
            HideUpgradeButton();
    }

    void OnPress(bool isDown)
    {
        if (isDown)
            InputController.OnCastleClick(this);
        else
        {
            var hoveredCastle = UICamera.currentTouch.current.GetComponent<CastleController>();
            InputController.OnDrop(hoveredCastle);
        }
    }

    void OnDrag(Vector2 delta)
    { 
        InputController.OnDrag(delta);
        var hoveredCastle = UICamera.currentTouch.current.GetComponent<CastleController>();
        InputController.OnCastleHover(hoveredCastle);
    }

    void OnDrop(GameObject go)
    {
        //InputController.OnDrop(this);
    }

    public void ActivateToSelector()
    {
        var selectorSprite = SelectorSprite.GetComponent<UI2DSprite>();
        selectorSprite.sprite2D = ToSelectorSprite;
    }

    public void ActivateFromSelector()
    {
        var selectorSprite = SelectorSprite.GetComponent<UI2DSprite>();
        selectorSprite.sprite2D = FromSelectorSprite;
    }

    public void DeactiveSelector()
    {
        var selector = SelectorSprite.GetComponent<UI2DSprite>();
        selector.sprite2D = null;
    }

    public void OnUpgrade()
    {
        InputController.OnCastleUpgrade(this);
    }

    public void ShowUpgradeButton()
    {
        UpgradeButton.enabled = true;
    }

    public void HideUpgradeButton()
    {
        UpgradeButton.enabled = false;
    }

    public override bool Equals(object o)
    {
        CastleController obj = o as CastleController;
        if (!obj)
            return false;

        return this.Id == obj.Id;
    }
}
