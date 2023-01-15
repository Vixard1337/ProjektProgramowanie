using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Hole : MonoBehaviour
{

    void OnTriggerEnter2D (Collider2D other) // funkcja czy gracz dotkn¹³ tego obiektu, spad³ i przegra³
    {
        if (other.tag == "Player")
        {
            SceneManager.LoadScene("GameOver"); // jeœli gracz wpadnie w dziurê to przegra
        }
    }

}
