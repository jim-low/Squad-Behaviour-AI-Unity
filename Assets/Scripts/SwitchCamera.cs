using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchCamera : MonoBehaviour
{
    public Camera mainCamera;
    public Camera fishCamera;

    void Start()
    {
        mainCamera.enabled = true;
        fishCamera.enabled = false;
    }

    void OnGUI()
    {
        if (GUI.Button(new Rect(10, 10, 150, 50), "Switch Camera"))
        {
            mainCamera.enabled = !mainCamera.enabled;
            fishCamera.enabled = !fishCamera.enabled;
        }
    }
}
