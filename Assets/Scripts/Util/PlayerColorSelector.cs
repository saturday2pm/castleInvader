using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class PlayerColorSelector
{
    static Color[] playerColors = { Color.black, Color.blue, Color.cyan, Color.gray, Color.green, Color.magenta, Color.red, Color.yellow };
    static Dictionary<int, Color> colorDict = new Dictionary<int, Color>();
    static int availableColorIdx = 0;
    static public Color GetColorById(int _id)
    {
        if (!colorDict.ContainsKey(_id))
        {
            Color color = playerColors[availableColorIdx++];
            colorDict[_id] = color;
        }

        return colorDict[_id];
    }
}

