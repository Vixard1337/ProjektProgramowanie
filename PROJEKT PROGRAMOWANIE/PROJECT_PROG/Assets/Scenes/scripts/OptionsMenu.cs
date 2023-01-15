using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OptionsMenu : MonoBehaviour
{

    public void BackButton1 ()
    {
        SceneManager.LoadScene(0);
        // przechodze do nastepnej sceny
        // scena 0 to menu
        // scena 1 to poziom
    }

}
