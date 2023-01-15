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

    public void Options() //za³aduj menu opcji
    {
        SceneManager.LoadScene(2);
        // przechodze do nastepnej sceny
        // scena 0 to menu
        // scena 1 to poziom
    }

    public void LoadLevel() // menu wyboru poziomów
    {
        SceneManager.LoadScene(3);
    }

    public void RestartLevel() // menu wyboru poziomów
    {
        SceneManager.LoadScene(GameObject.Find("RememberLevel").GetComponent<RememberLevel>().CurrentLevel);
    }

    public void ChooseLevel1() // wybór levelu 1
    {
        SceneManager.LoadScene(4);
        GameObject.Find("RememberLevel").GetComponent<RememberLevel>().CurrentLevel = 4;
    }

    public void ChooseLevel2() // wybór 2 levelu
    {
        SceneManager.LoadScene(5);
        GameObject.Find("RememberLevel").GetComponent<RememberLevel>().CurrentLevel = 5;
    }

    public void QuitGame ()
    {
        Debug.Log("Wyjscie z gry"); //zapisuje w konsoli, ¿e gra zosta³a zamkniêta
        Application.Quit(); // unity nie zamyka aplikacji w edytorze
    }
}
