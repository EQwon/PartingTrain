using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScene : MonoBehaviour
{
    public void MoveScene(int i)
    {
        SceneManager.LoadScene(i);
        Time.timeScale = 1;
    }
}