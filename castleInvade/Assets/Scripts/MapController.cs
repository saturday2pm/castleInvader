using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Simulator;
using ObjectPool;

public class MapController : MonoBehaviour
{
    public UI2DSprite BgSprite;
    public GameObject UnitPrefab;
    public GameObject CastlePrefab;

    ObjectPool<GameObject> unitPool;
    ObjectPool<GameObject> castlePool;

    Dictionary<int, CastleController> castles = new Dictionary<int, CastleController>();
    Dictionary<int, UnitController> units = new Dictionary<int, UnitController>();

    void Awake()
    {
        unitPool = new ObjectPool<GameObject>(100, () => { return Instantiate(UnitPrefab); });
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

    public void CreateCastleObject(int _id, Vector2 _position, int _unitCount, float _size, int _ownerIdx)
    {
        var castleObject = castlePool.pop();
        var castleController = castleObject.GetComponent<CastleController>();
        if(castleController != null)
        {
            castleObject.transform.parent = transform;
            castleObject.transform.localScale = Vector3.one;
            castleObject.SetActive(true);
            NGUITools.SetActive(castleObject, true);
            castleController.Init(_id, _position, _unitCount, _size, PlayerColorSelector.GetColorByNumber(_ownerIdx));
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
            unitController.Init(_id, _position, _unitCount, PlayerColorSelector.GetColorByNumber(_ownerIdx));
            units[_id] = unitController;
        }
    }

    public void RemoveUnitObject(int _id)
    {
        if (units.ContainsKey(_id))
        {
            var targetObject = units[_id].gameObject;
            NGUITools.SetActive(targetObject, false);
            unitPool.push(targetObject);
            units.Remove(_id);
        }
    }

    public CastleController GetCastleView(Castle castle)
    {
        if(!castles.ContainsKey(castle.Id))
        {
            int castleOwnerId = 0;
            if (castle.Owner != null)
                castleOwnerId = castle.Owner.Id;

            CreateCastleObject(castle.Id, new Vector2(castle.Pos.X, castle.Pos.Y), castle.UnitNum, castle.Radius, castleOwnerId);
        }
        return castles[castle.Id];
    }

    public UnitController GetUnitView(Unit unit)
    {
        if (!units.ContainsKey(unit.Id))
        {
            CreateUnitObject(unit.Id, new Vector2(unit.Pos.X, unit.Pos.Y), unit.Num, unit.Owner.Id);
        }
        return units[unit.Id];
    }
}
