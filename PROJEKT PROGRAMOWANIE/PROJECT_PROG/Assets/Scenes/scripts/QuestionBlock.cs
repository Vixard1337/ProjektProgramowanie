using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class QuestionBlock : MonoBehaviour
{
    //kontrolowanie jak wysoko odbije siê blok i jak szybko to siê stanie
    //public do modyfikowania w Inspector view na Unity
    //private tylko w obszarze skryptu
    public float bounceHeight = 0.5f; // jak wysoko siê odbije
    public float bounceSpeed = 4f; // jak szybko siê odbije
    //0f to np 0 jednostek
    public float coinMoveSpeed = 8f; //jak szybko bêdzie siê poruszaæ moneta
    public float coinMoveHeight = 3f; //jak wysoko bêdzie siê poruszaæ moneta
    public float coinFallDistance = 2f; // jak d³ugo ma opadaæ, zanim zostanie zniszczona

    private Vector2 originalPosition; // oryginalna pozycja bloku

    public Sprite emptyBlockSprite; //obraz bloku po interakcji gracza

    private bool canBounce = true; // czy siê odbije? domyœlnie na tak


    // Start is called before the first frame update
    void Start()
    {
        originalPosition = transform.localPosition; // transform poniewa¿ skrypt jest dodany jako component do assetu questionblock
    }
    //TA FUNKCJA BÊDZIE DZIEDZICZONA DO KLASY PLAYER, ¯EBY GRACZ MÓG£ MIEÆ INTERAKCJE Z BLOKIEM
    public void QuestionBlockBounce () // odbicie bloku poprzez interakcje gracza na blok
    {
        if (canBounce) // jeœli mo¿e blok siê odbiæ
        {
            canBounce = false; // to nie mo¿e siê odbiæ 2 raz

            StartCoroutine(Bounce());
               
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ChangeSprite() // funckja zmiany obrazu bloku po interakcji gracza z "?" na pusty
    {
        GetComponent<Animator>().enabled = false; // wy³¹czenie animacji po interakcji gracza
        GetComponent<SpriteRenderer>().sprite = emptyBlockSprite;
    }

    void PresentCoin () // funkcja przedstawienia monety
    {
        GameObject spininingCoin = (GameObject)Instantiate (Resources.Load("Prefabs/Spinning_Coin", typeof(GameObject))); // Wczytywanie modelu monety z folderu Prefabs typ GameObject

        spininingCoin.transform.SetParent (this.transform.parent);//nadanie rodzica

        spininingCoin.transform.localPosition = new Vector2 (originalPosition.x, originalPosition.y + 1); //nadaje pozycji monety po uderzeniu przez gracza +1, ¿eby moneta nie by³a na bloku tylko nad nim

        StartCoroutine (MoveCoin(spininingCoin));//uruchamianie programu, króry bêdzie porusza³ monetê w dó³ i w górê

    }




    IEnumerator Bounce() 
        {

        ChangeSprite(); // po uderzeniu bloku przez gracza zmienia siê obraz bloku

        PresentCoin(); // po uderzeniu wyœwietli siê moneta nad blokiem
        
        while (true)//while 1 zmiana pozycji bloku / zwiekszanie pozycji bloku += inkrementacja / nieskoñczona pêtla
        {
            transform.localPosition = new Vector2 (transform.localPosition.x, transform.localPosition.y + bounceSpeed * Time.deltaTime); // ustawienie pozycji bloku

            if (transform.localPosition.y >= originalPosition.y + bounceHeight)
                break; // zakoñczenie pêtli

            yield return null;
        }

        while (true)//while 2 zmiana pozycji bloku na jego oryginaln¹ pozycjê -= dekrementacja / nieskoñczona pêtla
        {
            transform.localPosition = new Vector2 (transform.localPosition.x, transform.localPosition.y - bounceSpeed * Time.deltaTime); // ustawienie pozycji bloku na jego otryginalnej pozycji

            if (transform.localPosition.y <= originalPosition.y)
            {
                transform.localPosition = originalPosition;
                break; // zakoñczenie pêtli
            }
            
            yield return null;
        }

    }

    IEnumerator MoveCoin(GameObject coin)
    {
        while(true)
        {
            coin.transform.localPosition = new Vector2 (coin.transform.localPosition.x, coin.transform.localPosition.y + coinMoveSpeed * Time.deltaTime); //zmienia pozycje monety po osi y
            //przez parametr szybkoœci poruszania siê monety klatka po klatce
            if (coin.transform.localPosition.y >= originalPosition.y + coinMoveHeight + 1)// sprawdzamy czy pozycja monety siê zmieni³a (+1, bo o 1 w góre ni¿ oryginalna pozycja)
           
            break; // zakoñczenie pêtli

            yield return null;
        }

        while (true)//while 2 zmiana pozycji monety na jego oryginaln¹ pozycjê -= dekrementacja / nieskoñczona pêtla
        {
            coin.transform.localPosition = new Vector2(coin.transform.localPosition.x, coin.transform.localPosition.y - coinMoveSpeed * Time.deltaTime); // ustawienie pozycji bloku na jego otryginalnej pozycji
            //zmienia pozycje monety po osi y o 1
            // powrót do pozycji oryginalnej (pocz¹tkowej)
            if (coin.transform.localPosition.y <= originalPosition.y + coinFallDistance + 1)
            {
                Destroy(coin.gameObject);//zniszczenie obiektu
                break; // zakoñczenie pêtli
            }
            yield return null;
        }

    }


}
