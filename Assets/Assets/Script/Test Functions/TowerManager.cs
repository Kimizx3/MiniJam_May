using UnityEngine;

public class TowerManager : MonoBehaviour
{
    public GameObject[] baseTowers;
    public GameObject[] upgradeTowers;

    private EmptySlot _currentSlot;

    public void SelectBaseTower(EmptySlot slot)
    {
        _currentSlot = slot;
    }
}
