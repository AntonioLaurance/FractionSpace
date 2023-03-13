using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BotonInicio: MonoBehaviour
{
    public void CambiaAEscena(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
