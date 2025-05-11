using System;
using Unity.Mathematics;
using UnityEngine;

public class EmptySlot : MonoBehaviour
{
    public GameObject baseTower;
    public TowerManager towerManager;

    private void OnMouseDown()
    {
        if (baseTower == null)
        {
            
        }
        else
        {
            
        }
    }

    public void PlaceTower(GameObject towerPrefab)
    {
        baseTower = Instantiate(towerPrefab, transform.position, Quaternion.identity);
    }

    public void UpgradeTower(GameObject upgradePrefab)
    {
        if (baseTower != null)
        {
            Destroy(baseTower);
        }

        baseTower = Instantiate(upgradePrefab, transform.position, quaternion.identity);
    }
}
