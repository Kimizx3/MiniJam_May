using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

public class Tower : MonoBehaviour
{
    public List<GameObject> towers = new List<GameObject>();
    public float radius = 5f;

    private Button _selfButton;
    private int _numOfObjects;
    private bool _setActive = false;

    private void Awake()
    {
        _numOfObjects = towers.Count;
        
        foreach (var tower in towers)
        {
            tower.SetActive(false);
        }

        _selfButton = GetComponent<Button>();
        if (_selfButton != null)
        {
            _selfButton.onClick.AddListener(DisplayUI);
        }
    }

    private void DisplayUI()
    {
        _setActive = true;
        float angleStep = 180f / _numOfObjects;
        float currentAngle = 0f;

        for (int i = 0; i < _numOfObjects; i++)
        {
            float rad = currentAngle * Mathf.Deg2Rad;

            float x = transform.position.x + Mathf.Cos(rad) * radius;
            float z = transform.position.z + Mathf.Sin(rad) * radius;

            Vector3 spawnPosition = new Vector3(x, transform.position.y, z);
            towers[i].SetActive(true);

            currentAngle -= angleStep;
        }
    }

    private void CloseUI()
    {
        _setActive = false;
        foreach (var tower in towers)
        {
            tower.SetActive(false);
        }
    }
}
