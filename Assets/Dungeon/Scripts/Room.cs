using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room
{
    /// <summary>
    /// 
    /// </summary>
    [Header("Grid")]
    public Vector2 gridPos;

    /// <summary>
    /// 
    /// </summary>
    [Header("Type of room")]
    public int type;

    /// <summary>
    /// doorTop,doorBot,doorLeft,doorRight : Indique si une porte existe à chaque endroit de la piece
    /// </summary>
    [Header("Door position")]
    public bool doorTop;
    public bool doorBot;
    public bool doorLeft;
    public bool doorRight;

    public Room(Vector2 _gridPos, int _type)
    {
        gridPos = _gridPos;
        type = _type;
    }
}
