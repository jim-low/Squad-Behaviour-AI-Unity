using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private bool isDetected = false;

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
