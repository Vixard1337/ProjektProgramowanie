using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGSoundScript : MonoBehaviour
{
    void Start () // skrypt pozwala odtwarza� melodie na wielu scenach
    {

    }

    //Odtwarzanie Globalne
    private static BGSoundScript instance = null;
    public static BGSoundScript Instance // zwraca statyczn� instancje z klasy BGSound
    {
        get { return instance; }
    }
    void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(this.gameObject);
    }
    //Zakoncz odtwarzanie globalne
    //aktualizacja tylko 1
    //BGSoundScript.Instance.gameObject.GetComponent<AudioSource>().Pause(); //dodam do zatrzymania dzwi�ku w innej scenie na koniec skryptu
}
