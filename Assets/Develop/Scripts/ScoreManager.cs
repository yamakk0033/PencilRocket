using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ScoreManager
{
    public static decimal Altitude { get; set; }
    public static decimal ViewAltitude { get { return Altitude * 10; } }
}
