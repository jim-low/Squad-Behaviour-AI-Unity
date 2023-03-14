using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private bool isDetected = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool isSpotted()
    {
        return isDetected;
    }

    public void SetDetection(bool detected)
    {
        isDetected = detected;
    }

    void OnDrawGizmos()
    {
        if (isDetected)
        {
            Gizmos.DrawIcon(transform.position + (transform.up * 2.25f), "detected.png", true);
        }
    }
}
