using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class StartScreen : MonoBehaviour
{
    public string SampleScene;
    public void OnPlayButtonClick()
    {
        SceneManager.LoadScene("SampleScene");
    }

}
