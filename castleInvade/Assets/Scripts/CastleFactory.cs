using UnityEngine;
using System.Collections;
using ObjectPool;
public class CastleFactory : MonoBehaviour
{
    public GameObject CastlePrefab;
    ObjectPool<GameObject> castlePool;


    void Start()
    {
        castlePool = new ObjectPool<GameObject>(10, () =>
        {
            return Instantiate(CastlePrefab);
        });
    }

    public GameObject Get()
    {
        return castlePool.pop();
    }

    public void Release(GameObject obj)
    {
        castlePool.push(obj);
    }
}
