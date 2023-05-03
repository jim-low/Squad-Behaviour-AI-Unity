using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FishPanelColorChange : MonoBehaviour
{
    float timeLeft;
    Color targetColor;

    private Image panelImg;

    void Start()
    {
        panelImg = GetComponent<Image>();
    }

    void Update()
    {
        if (timeLeft <= Time.deltaTime)
        {
            // transition complete
            // assign the target color
            panelImg.color = targetColor;

            // start a new transition
            targetColor = new Color(Random.value, Random.value, Random.value);
            timeLeft = 1.0f;
        }
        else
        {
            // transition in progress
            // calculate interpolated color
            panelImg.color = Color.Lerp(panelImg.color, targetColor, Time.deltaTime / timeLeft);

            // update the timer
            timeLeft -= Time.deltaTime;
        }
    }
}
