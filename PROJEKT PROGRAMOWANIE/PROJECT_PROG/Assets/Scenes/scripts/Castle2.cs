using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Castle2 : MonoBehaviour
{

    void OnTriggerEnter2D(Collider2D other) // funkcja kolidowania z drzwami zamku // tylko gracz wchodzi w interakcje z zamkiem
    {
        if (other.tag == "Player")
        {
            SceneManager.LoadScene("Win2"); // je�li gracz wejdzie w interakcje z drzwami zamku, to za�aduj Scene Win
        }
    }


}
