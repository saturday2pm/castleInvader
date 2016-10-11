using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class PlayerColorSelector
{
    static Color[] playerColors = { Color.black, Color.blue, Color.cyan, Color.gray, Color.green, Color.magenta, Color.red, Color.white, Color.yellow };
    static public Color GetColorByNumber(int number)
    {
        if (number >= playerColors.Length)
        {
            throw new Exception("too many players.");
        }

        return playerColors[number];
    }
}

