﻿using UnityEngine;
using System.Collections;
using System.Linq;

public class CastleController : MonoBehaviour
{
    public UILabel UnitCountLabel;
    public UI2DSprite CastleSprite;
    public UI2DSprite SelectorSprite;

    public Sprite FromSelectorSprite;
    public Sprite ToSelectorSprite;

    static InputController InputController;

    //int id = -1;
    public int Id; //{ get { return id; } }

    public bool Init(int _id, Vector2 _position, int _unitCount, float _size, Color _color)
    {
        if (InputController == null)
        {
            InputController = GameObject.FindObjectOfType<InputController>();
        }

        var button = SelectorSprite.GetComponent<UIButton>();

        Id = _id;
        transform.localPosition = new Vector3(_position.x, _position.y, 0);
        UpdateCastle(_unitCount, _size, _color);

        return true;
    }

    public void UpdateCastle(int _unitCount, float _size, Color _color)
    {
        CastleSprite.color = _color;

        int rectSize = (int)(_size * 2.0f);
        CastleSprite.width = rectSize;
        CastleSprite.height = rectSize;
        SelectorSprite.width = rectSize;
        SelectorSprite.height = rectSize;

        UnitCountLabel.text = _unitCount.ToString();
        float labelYPos = (CastleSprite.height + UnitCountLabel.height) * 0.5f;
        UnitCountLabel.transform.localPosition = new Vector3(0, labelYPos, 0);
    }

    void OnPress(bool isDown)
    {
        InputController.OnCastleClick(this);
    }

    void OnDrag(Vector2 delta)
    { 
        InputController.OnDrag(delta);
        var hoveredCastle = UICamera.currentTouch.current.GetComponent<CastleController>();
        InputController.OnCastleHover(hoveredCastle);
    }

    void OnDrop(GameObject go)
    {
        InputController.OnDrop(this);
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

    public override bool Equals(object o)
    {
        CastleController obj = o as CastleController;
        if (!obj)
            return false;

        return this.Id == obj.Id;
    }
}
