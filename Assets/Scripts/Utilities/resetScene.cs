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
    }
}
