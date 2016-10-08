using UnityEngine;
using System.Collections;
using System.Linq;

public class CastleController : MonoBehaviour
{
    public UILabel UnitCountLabel;
    public UI2DSprite CastleSprite;

    int id = -1;
    public int Id { get { return id; } }

    public bool Init(int _id, Vector2 _position, int _unitCount, float _size)
    {
        id = _id;
        transform.localPosition = new Vector3(_position.x, _position.y, 0);
        UpdateCastle(_unitCount, _size);
        return true;
    }

    void UpdateCastle(int _unitCount, float _size)
    {
        int rectSize = (int)(_size * 2.0f);
        CastleSprite.width = rectSize;
        CastleSprite.height = rectSize;

        UnitCountLabel.text = _unitCount.ToString();
        float labelYPos = (CastleSprite.height + UnitCountLabel.height) * 0.5f;
        UnitCountLabel.transform.localPosition = new Vector3(0, labelYPos, 0);
    }
}
