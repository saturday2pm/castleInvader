using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ObjectPool;

public class MapController : MonoBehaviour
{
    public UI2DSprite BgSprite;
    public GameObject UnitPrefab;
    public GameObject CastlePrefab;

    ObjectPool<GameObject> unitPool;
    ObjectPool<GameObject> castlePool;

    Dictionary<int, CastleController> castles = new Dictionary<int, CastleController>();
    
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

    public void MakeCastleObject(int _id, Vector2 _position, int _unitCount, float _size)
    {
        var castleObject = castlePool.pop();
        var castleController = castleObject.GetComponent<CastleController>();
        if(castleController != null)
        {
            castleObject.transform.parent = transform;
            castleObject.transform.localScale = Vector3.one;
            NGUITools.SetActive(castleObject, true);
            castleController.Init(_id, _position, _unitCount, _size);
            castles[_id] = castleController;
        }
    }
}
