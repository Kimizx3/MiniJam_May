using System;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;
using UnityEngine.UIElements;


public class Unit : MonoBehaviour
{
    public string Name;
    public bool isAlive { get; private set; } = true;
    

    private Vector3 initPosition;
    private Quaternion initRotation;

    private void Awake()
    {
        initPosition = transform.position;
        initRotation = transform.rotation;
    }

    public void Die()
    {
        if (isAlive)
        {
            isAlive = false;
            gameObject.SetActive(false);
        }
    }

    public void Revive()
    {
        if (!isAlive)
        {
            isAlive = true;
            gameObject.SetActive(true);
        }
    }
}
