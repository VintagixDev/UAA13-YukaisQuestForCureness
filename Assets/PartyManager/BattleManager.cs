using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour
{
    [Header("Other class")]
    [SerializeField] bool _battleComplete;
    [SerializeField] int _EnemyAlive;
    public bool Battle()
    {
        if (_EnemyAlive == 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public void WhenEnemyIsKilled()
    {
        _EnemyAlive = _EnemyAlive - 1;
    }
    public void StartBattle()
    {
        // Fait spawn les ennemis
    }
    public void ResetBattleSystem()
    {
        _battleComplete = false; 
        _EnemyAlive = 0;
    }
    // Update is called once per frame
    void Update()
    {
        if (!_battleComplete)
        {
            if (Battle())
            {
                _battleComplete = true;
            }
        }
    }
}