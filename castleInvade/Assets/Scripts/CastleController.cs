using UnityEngine;
using System.Collections;
using System.Linq;

public class CastleController : MonoBehaviour
{
    uint unitCount = 0;
    public uint UnitCount
    {
        get
        {
            return unitCount;
        }
        set
        {
            unitCount = value;
            
            int castleSize = (int)unitCount * 2;
            CastleSprite.width = castleSize;
            CastleSprite.height = castleSize;
            UnitCountLabel.text = unitCount.ToString();
            float labelYPos = (CastleSprite.height + UnitCountLabel.height) * 0.5f;
            UnitCountLabel.transform.localPosition = new Vector3(0, labelYPos, 0);
        }
    }

    public UILabel UnitCountLabel;
    public UI2DSprite CastleSprite;

    bool init()
    {
        return true;
    }

	// Update is called once per frame
	void Update ()
    {
	
	}

    
}
