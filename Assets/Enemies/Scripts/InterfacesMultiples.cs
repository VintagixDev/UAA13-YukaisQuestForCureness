using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStunnable
{
    void Stun(float duration);
}

public interface IBurnable
{
    void Burn(float duration);
}
public interface IEnemyMovement
{
    void Movements();
}