using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FermerPorteBoutton : MonoBehaviour
{
    [SerializeField] private GameSupervisor MASTER;
    private void OnTriggerEnter2D(Collider2D collision)
    {

        MASTER.SetBattle();
    }
}
