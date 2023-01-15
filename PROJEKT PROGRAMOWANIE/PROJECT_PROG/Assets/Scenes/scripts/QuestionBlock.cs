using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class QuestionBlock : MonoBehaviour
{
    //kontrolowanie jak wysoko odbije si� blok i jak szybko to si� stanie
    //public do modyfikowania w Inspector view na Unity
    //private tylko w obszarze skryptu
    public float bounceHeight = 0.5f; // jak wysoko si� odbije
    public float bounceSpeed = 4f; // jak szybko si� odbije
    //0f to np 0 jednostek
    public float coinMoveSpeed = 8f; //jak szybko b�dzie si� porusza� moneta
    public float coinMoveHeight = 3f; //jak wysoko b�dzie si� porusza� moneta
    public float coinFallDistance = 2f; // jak d�ugo ma opada�, zanim zostanie zniszczona

    private Vector2 originalPosition; // oryginalna pozycja bloku

    public Sprite emptyBlockSprite; //obraz bloku po interakcji gracza

    private bool canBounce = true; // czy si� odbije? domy�lnie na tak


    // Start is called before the first frame update
    void Start()
    {
        originalPosition = transform.localPosition; // transform poniewa� skrypt jest dodany jako component do assetu questionblock
    }
    //TA FUNKCJA B�DZIE DZIEDZICZONA DO KLASY PLAYER, �EBY GRACZ M�G� MIE� INTERAKCJE Z BLOKIEM
    public void QuestionBlockBounce () // odbicie bloku poprzez interakcje gracza na blok
    {
        if (canBounce) // je�li mo�e blok si� odbi�
        {
            canBounce = false; // to nie mo�e si� odbi� 2 raz

            StartCoroutine(Bounce());
               
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ChangeSprite() // funckja zmiany obrazu bloku po interakcji gracza z "?" na pusty
    {
        GetComponent<Animator>().enabled = false; // wy��czenie animacji po interakcji gracza
        GetComponent<SpriteRenderer>().sprite = emptyBlockSprite;
    }

    void PresentCoin () // funkcja przedstawienia monety
    {
        GameObject spininingCoin = (GameObject)Instantiate (Resources.Load("Prefabs/Spinning_Coin", typeof(GameObject))); // Wczytywanie modelu monety z folderu Prefabs typ GameObject

        spininingCoin.transform.SetParent (this.transform.parent);//nadanie rodzica

        spininingCoin.transform.localPosition = new Vector2 (originalPosition.x, originalPosition.y + 1); //nadaje pozycji monety po uderzeniu przez gracza +1, �eby moneta nie by�a na bloku tylko nad nim

        StartCoroutine (MoveCoin(spininingCoin));//uruchamianie programu, kr�ry b�dzie porusza� monet� w d� i w g�r�

    }




    IEnumerator Bounce() 
        {

        ChangeSprite(); // po uderzeniu bloku przez gracza zmienia si� obraz bloku

        PresentCoin(); // po uderzeniu wy�wietli si� moneta nad blokiem
        
        while (true)//while 1 zmiana pozycji bloku / zwiekszanie pozycji bloku += inkrementacja / niesko�czona p�tla
        {
            transform.localPosition = new Vector2 (transform.localPosition.x, transform.localPosition.y + bounceSpeed * Time.deltaTime); // ustawienie pozycji bloku

            if (transform.localPosition.y >= originalPosition.y + bounceHeight)
                break; // zako�czenie p�tli

            yield return null;
        }

        while (true)//while 2 zmiana pozycji bloku na jego oryginaln� pozycj� -= dekrementacja / niesko�czona p�tla
        {
            transform.localPosition = new Vector2 (transform.localPosition.x, transform.localPosition.y - bounceSpeed * Time.deltaTime); // ustawienie pozycji bloku na jego otryginalnej pozycji

            if (transform.localPosition.y <= originalPosition.y)
            {
                transform.localPosition = originalPosition;
                break; // zako�czenie p�tli
            }
            
            yield return null;
        }

    }

    IEnumerator MoveCoin(GameObject coin)
    {
        while(true)
        {
            coin.transform.localPosition = new Vector2 (coin.transform.localPosition.x, coin.transform.localPosition.y + coinMoveSpeed * Time.deltaTime); //zmienia pozycje monety po osi y
            //przez parametr szybko�ci poruszania si� monety klatka po klatce
            if (coin.transform.localPosition.y >= originalPosition.y + coinMoveHeight + 1)// sprawdzamy czy pozycja monety si� zmieni�a (+1, bo o 1 w g�re ni� oryginalna pozycja)
           
            break; // zako�czenie p�tli

            yield return null;
        }

        while (true)//while 2 zmiana pozycji monety na jego oryginaln� pozycj� -= dekrementacja / niesko�czona p�tla
        {
            coin.transform.localPosition = new Vector2(coin.transform.localPosition.x, coin.transform.localPosition.y - coinMoveSpeed * Time.deltaTime); // ustawienie pozycji bloku na jego otryginalnej pozycji
            //zmienia pozycje monety po osi y o 1
            // powr�t do pozycji oryginalnej (pocz�tkowej)
            if (coin.transform.localPosition.y <= originalPosition.y + coinFallDistance + 1)
            {
                Destroy(coin.gameObject);//zniszczenie obiektu
                break; // zako�czenie p�tli
            }
            yield return null;
        }

    }


}
