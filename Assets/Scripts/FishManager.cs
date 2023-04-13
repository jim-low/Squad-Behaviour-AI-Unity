using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishManager : MonoBehaviour
{
    public Camera mainCamera;
    public Camera fishCamera;
    public GameObject fish;

    private AudioSource funkyTownAudio;

    void Start()
    {
        mainCamera.enabled = false;
        fishCamera.enabled = true;
        funkyTownAudio = fish.transform.gameObject.transform.gameObject.GetComponent<AudioSource>(); // safest way to get component in Unity
    }

    void OnGUI()
    {
        if (GUI.Button(new Rect(10, 10, 150, 50), "Fish?"))
        {
            mainCamera.enabled = !mainCamera.enabled;
            fishCamera.enabled = !fishCamera.enabled;

            if (fishCamera.enabled)
            {
                funkyTownAudio.Play();
            } else
            {
                funkyTownAudio.Pause();
            }
        }
    }
}
