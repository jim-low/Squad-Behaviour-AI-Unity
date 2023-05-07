using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class resetScene : MonoBehaviour
{
    // Start is called before the first frame update
    public void reset()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);  

        GameObject[] redBois = GameObject.FindGameObjectsWithTag("Ally");
        GameObject[] blackBois = GameObject.FindGameObjectsWithTag("Enemy");

        foreach (GameObject red in redBois) {
            red.GetComponent<Soldier>().Reset();
        }

        foreach (GameObject black in blackBois) {
            black.GetComponent<Soldier>().Reset();
        }
    }
}
