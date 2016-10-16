using UnityEngine;
using System.Collections;

public class UnitController : MonoBehaviour
{
    public UI2DSprite UnitSprite;
    int id = -1;
    public int Id { get { return id; } }

    public void Init(int _id, Vector2 _position, int _unitCount, Color _color)
    {
        id = _id;
        UnitSprite.color = _color;
        transform.localPosition = new Vector3(_position.x, _position.y, 0.0f);

        int size = (int)(Mathf.Sqrt(_unitCount) * 6.0f);
        UnitSprite.width = size;
        UnitSprite.height = size;
        //iTween.FadeFrom(gameObject, 0.0f, 0.5f);
    }

    public void UpdateUnit(Vector2 position, int unitCount)
    { 
        transform.localPosition = new Vector3(position.x, position.y, 0.0f);
        int size = (int)(Mathf.Sqrt(unitCount) * 6.0f);
        UnitSprite.width = size;
        UnitSprite.height = size;
    }
}
