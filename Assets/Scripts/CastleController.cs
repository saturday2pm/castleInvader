using UnityEngine;
using System.Collections;
using System.Linq;

public class CastleController : MonoBehaviour
{
    public UILabel UnitCountLabel;
    public UI2DSprite CastleSprite;
    public UI2DSprite SelectorSprite;

    static InputController InputController;


    int id = -1;
    public int Id { get { return id; } }

    public bool Init(int _id, Vector2 _position, int _unitCount, float _size, Color _color)
    {
        id = _id;
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

    public void OnClick()
    {
        if (!InputController)
        {
            var obj = GameObject.FindGameObjectWithTag("InputController");
            if(obj)
            {
                InputController = obj.GetComponent<InputController>();
                if(!InputController)
                {
                    Debug.Log("InputController is Missing!");
                }
            }
        }
        InputController.OnCastleClick(this);
    }
}
