using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitManager : MonoBehaviour
{
    public List<Unit> units = new List<Unit>();

    private void Update()
    {
        if (Input.GetKey(KeyCode.K))
        {
            KillAll();
        }
        else if (Input.GetKey(KeyCode.R))
        {
            ReviveAll();
        }
    }

    public void KillAll()
    {
        foreach (var unit in units)
        {
            unit.Die();
        }
    }

    public void ReviveAll()
    {
        foreach (var unit in units)
        {
            if (!unit.isAlive)
            {
                unit.Revive();
            }
        }
    }
}
