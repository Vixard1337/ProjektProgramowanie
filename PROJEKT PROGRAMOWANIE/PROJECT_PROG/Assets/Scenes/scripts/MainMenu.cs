using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public void NewGane()
    {
        SceneManager.LoadScene("Level1");
        GameObject.Find("RememberLevel").GetComponent<RememberLevel>().CurrentLevel = 4;
        // przechodze do nastepnej sceny
        // scena 0 to menu
        // scena 1 to poziom
    }

    public void Options() //za�aduj menu opcji
    {
        SceneManager.LoadScene(2);
        // przechodze do nastepnej sceny
        // scena 0 to menu
        // scena 1 to poziom
    }

    public void LoadLevel() // menu wyboru poziom�w
    {
        SceneManager.LoadScene(3);
    }

    public void RestartLevel() // menu wyboru poziom�w
    {
        SceneManager.LoadScene(GameObject.Find("RememberLevel").GetComponent<RememberLevel>().CurrentLevel);
    }

    public void ChooseLevel1() // wyb�r levelu 1
    {
        SceneManager.LoadScene(4);
        GameObject.Find("RememberLevel").GetComponent<RememberLevel>().CurrentLevel = 4;
    }

    public void ChooseLevel2() // wyb�r 2 levelu
    {
        SceneManager.LoadScene(5);
        GameObject.Find("RememberLevel").GetComponent<RememberLevel>().CurrentLevel = 5;
    }

    public void QuitGame ()
    {
        Debug.Log("Wyjscie z gry"); //zapisuje w konsoli, �e gra zosta�a zamkni�ta
        Application.Quit(); // unity nie zamyka aplikacji w edytorze
    }
}
