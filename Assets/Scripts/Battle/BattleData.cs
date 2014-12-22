﻿using UnityEngine;
using System.Collections;

public class BattleData {
    public readonly NPCInstance[] npcs;
    public readonly TileCoord[] deployPoints;
    public readonly Character[] npcData;
}

public struct NPCInstance {
    public readonly int row;
    public readonly int col;
    /// <summary>
    /// indexes into array position of json character data
    /// </summary>
    public readonly int index;
}

public struct TileCoord {
    public int row;
    public int col;
}