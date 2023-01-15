using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Hole : MonoBehaviour
{

    void OnTriggerEnter2D (Collider2D other) // funkcja czy gracz dotkn�� tego obiektu, spad� i przegra�
    {
        if (other.tag == "Player")
        {
            SceneManager.LoadScene("GameOver"); // je�li gracz wpadnie w dziur� to przegra
        }
    }

}
