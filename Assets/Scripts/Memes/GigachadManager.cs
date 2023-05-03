using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GigachadManager : MonoBehaviour
{
    private AudioSource audioSource;
    private Image gigachadPortrait;
    public Camera mainCamera;
    public Camera gigachadCamera;
    public Sprite[] gigachadImages;

    private int prevIndex = 0;

    void Start()
    {
        mainCamera.enabled = true;
        gigachadCamera.enabled = false;
        audioSource = GetComponent<AudioSource>();
        audioSource.Pause();
        gigachadPortrait = GetComponent<Image>();
    }

    private IEnumerator changeGigachad()
    {
        int currIndex = Random.Range(0, gigachadImages.Length);
        if (prevIndex == currIndex)
        {
            currIndex = Random.Range(0, gigachadImages.Length);
        }
        yield return new WaitForSeconds(2f);
        prevIndex = currIndex;
        gigachadPortrait.sprite = gigachadImages[currIndex];
        StartCoroutine(changeGigachad());
    }

    void OnGUI()
    {
        if (GUI.Button(new Rect(10, 70, 150, 50), "Gigachad"))
        {
            if (mainCamera.enabled)
            {
                mainCamera.enabled = false;
                gigachadCamera.enabled = true;
                audioSource.Play();
            } else {
                mainCamera.enabled = true;
                gigachadCamera.enabled = false;
                audioSource.Pause();
            }
            StartCoroutine(changeGigachad());
        }
    }
}
