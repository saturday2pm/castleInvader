using UnityEngine;
using System.Collections.Generic;
using Simulator;
using ObjectPool;
using System;
using System.Linq;

public class MapController : MonoBehaviour
{
    public UI2DSprite BgSprite;
    public GameObject UnitPrefab;
    public GameObject CastlePrefab;
    public InputController InputController;

    ObjectPool<GameObject> unitPool;
    ObjectPool<GameObject> castlePool;

    Dictionary<int, CastleController> castles = new Dictionary<int, CastleController>();
    Dictionary<int, UnitController> units = new Dictionary<int, UnitController>();

    void Awake()
    {
        unitPool = new ObjectPool<GameObject>(10, () => { return Instantiate(UnitPrefab); });
        castlePool = new ObjectPool<GameObject>(10, () => { return Instantiate(CastlePrefab); });
    }

    public void SetMapSize(Vector2 size)
    {
        float bgMargin = 100.0f;
        BgSprite.width = (int)(size.x + bgMargin);
        BgSprite.height = (int)(size.y + bgMargin);

        GetComponent<UIWidget>().width = (int)size.x;
        GetComponent<UIWidget>().height = (int)size.y;

        transform.localPosition = new Vector3(size.x * -0.5f, size.y * -0.5f, 0.0f);
    }

    public void CreateCastleObject(int _id, Vector2 _position, int _unitCount, float _size, int _ownerIdx, bool _isUserCastle)
    {
        var castleObject = castlePool.pop();
        var castleController = castleObject.GetComponent<CastleController>();
        if(castleController != null)
        {
            castleObject.transform.parent = transform;
            castleObject.transform.localScale = Vector3.one;
            castleObject.SetActive(true);
            NGUITools.SetActive(castleObject, true);
            castleController.Init(_id, _position, _unitCount, _size, PlayerColorSelector.GetColorById(_ownerIdx), _isUserCastle);
            castles[_id] = castleController;
        }
    }

    public void CreateUnitObject(int _id, Vector2 _position, int _unitCount, int _ownerIdx)
    {
        var unitObject = unitPool.pop();
        var unitController = unitObject.GetComponent<UnitController>();
        if(unitController != null)
        {
            unitController.transform.parent = transform;
            unitController.transform.localScale = Vector3.one;
            NGUITools.SetActive(unitObject, true);
            unitController.Init(_id, _position, _unitCount, PlayerColorSelector.GetColorById(_ownerIdx));
            units[_id] = unitController;
        }
    }

    public CastleController GetCastleObject(Castle _castle)
    {
        if(!castles.ContainsKey(_castle.Id))
        {
            int castleOwnerId = 0;
            bool isUserCastle = _castle.Owner.IsUser;
            if (_castle.Owner != null)
                castleOwnerId = _castle.Owner.Id;

            CreateCastleObject(_castle.Id, new Vector2(_castle.Pos.X, _castle.Pos.Y), _castle.UnitNum, _castle.Radius, castleOwnerId, isUserCastle);
        }
        return castles[_castle.Id];
    }

    public UnitController GetUnitObject(Unit _unit)
    {
        if (!units.ContainsKey(_unit.Id))
        {
            CreateUnitObject(_unit.Id, new Vector2(_unit.Pos.X, _unit.Pos.Y), _unit.Num, _unit.Owner.Id);
        }
        return units[_unit.Id];
    }

    public void RemoveUnitObject(int _targetId)
    {
        if (units.ContainsKey(_targetId))
        {
            var targetObject = units[_targetId].gameObject;
            NGUITools.SetActive(targetObject, false);
            unitPool.push(targetObject);
            units.Remove(_targetId);
        }
    }

    void OnPress(bool isDown)
    {
        if (isDown)
            InputController.OnBackgroundClick();
    }

    void OnDrag(Vector2 delta)
    {
        if(UICamera.currentTouch.current == gameObject)
        {
            InputController.OnCastleHover(null);
        }
    }

    void OnDrop(GameObject go)
    {
        InputController.OnDrop(null);
    }
}
