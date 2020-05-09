using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadGameScene : MonoBehaviour
{
    void Start()
    {

        ///Just a small Scene change on Restart to clear everything out.
        UnityEngine.SceneManagement.SceneManager.LoadSceneAsync ( "MineSweeper", UnityEngine.SceneManagement.LoadSceneMode.Single );
    }
}
