using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyAI : MonoBehaviour // skrypt zachowania przeciwnika, uruchomi siê gdy przeciwnik pojawi siê w zasiêgu widocznoœci gracza
{
    public float gravity; // grawitacja dla przeciwnika np. opadanie z platformy
    public Vector2 velocity; // przedkoœæ ruchu przeciwnika
    public bool isWalkingLeft = true; // czy przeciwnik chodzi w lew¹ stronê domyœlnie na tak
    
    public LayerMask floorMask; //Maska dla pod³o¿a
    public LayerMask wallMask; //Maska dla œcian
    
    private bool grounded = false; //czy przecownik jest na ziemi

    private bool shouldDie = false; // czy przeciwnik powinien umrzeæ domyœlnie ustawione na nie / umrze po interakcji z graczem jeœli ten na niego skoczy
    private float deathTimer = 0; // przeciwnik nie umrze natychmiast 

    public float timeBeforeDestroy = 1.0f;// czas znikniêcia z ekranu 1 sekunda


    private enum EnemyState //Stany przeciwnika
    {
        walking, //chód
        falling, //spadanie
        dead //œmieræ
    }

    private EnemyState state = EnemyState.falling;

    // Start is called before the first frame update
    void Start()
    {
        enabled = false; // zatrzymuje dzia³anie kodu, ¿eby przeciwnik nie porusza³ siê w stronê gracza, od razu po wystartowaniu poziomu, lecz gdy znajdzie siê w jego zasiêgu wzroku
        Fall (); // uruchomienie funkcji opadania przeciwnika
    }

    // Update is called once per frame
    void Update()
    {
        UpdateEnemyPosition();// uruchamianie funkcji aktualizacji pozycji przeciwnika

        CheckCrushed();// aktualizacja funkcji sprawdzania czy przeciwnik zosta³ zgnieciony
    }
    // ta funkcja Crush() bêdzie dziedziczona do klasy Player
    public void Crush () // funckja zgniecienia przeciwnika
    {
        state = EnemyState.dead; //zmiana stanu przeciwnika na œmieræ

        GetComponent<Animator>().SetBool("isCrushed", true); // zmiana animacji chodzenia na zgniecion¹ posturê przeciwnika z assetów animacji o nazwiê isCrushed

        GetComponent<Collider2D>().enabled = false; // false, aby gracz nie móg³ ju¿ przeprowadzaæ interakcji z przeciwnikiem

        shouldDie = true;
    }
    void CheckCrushed () // funkcja sprawdzania czy przeciwnik zosta³ zgnieciony / zabity
    {
        if (shouldDie) // je¿eli powinien umrzeæ to
        {
            if (deathTimer <= timeBeforeDestroy) // czas umierania jest mniejszy lub równy czasowi zanim zostanie zniszczony deathTimer = 0  timeBeforeDestroy = 1 sekunda
            {
                deathTimer += Time.deltaTime; // do czasu umierania dodaæ i przypisaæ czas tej operacji // Time.deltaTime czas od jednej klatki do nastepnej frame to the next frame
            }
            else
            {
                shouldDie = false;

                Destroy(this.gameObject); // Nastêpujê zniszczenie przeciwnika
            }
        }

    }

    void UpdateEnemyPosition() // funkcja aktualizacji pozycji przeciwnika
    {
        if (state != EnemyState.dead) // je¿eli stan nie jest równy stanu œmierci to aktualizuje pozycje przeciwnika
        {
            Vector3 pos = transform.localPosition; // pozycja
            Vector3 scale = transform.localScale;// przeciwnik ma tak¹ sam¹ skalê/rozmiar

            if(state == EnemyState.falling)
            {
                pos.y += velocity.y * Time.deltaTime; // pozycja zwiêksza siê z czasem
                velocity.y -= gravity * Time.deltaTime; // prêdkoœæ ruchu zmniejsza siê z czasem
            }
            if (state == EnemyState.walking)
            {
                if (isWalkingLeft)
                {
                    pos.x -= velocity.x * Time.deltaTime;

                    scale.x = -1; // -1 poniewa¿ przeciwnik kieruje siê w lewo w tym przypadku

                }
                else
                {
                    pos.x += velocity.x * Time.deltaTime;

                    scale.x = 1; // 1 poniewa¿ przeciwnik kieruje siê w prawo w tym przypadku
                }
            }
            if (velocity.y <= 0)
                pos = CheckGround (pos);
            CheckWalls (pos, scale.x);

            transform.localPosition = pos; //przyrównanie lokalnej pozycji do zmiennej pos
            transform.localScale = scale; //przyrównanie lokalnej skali do zmiennej scale

        }
    }

    Vector3 CheckGround (Vector3 pos) // funkcja sprawdzenia czy jest powierzchnia
    {   //Za pomoc¹ originLeft, Middle, Right mo¿na stworzyæ obiekty, które mo¿e uderzyæ przeciwnik
        Vector2 originLeft = new Vector2 (pos.x - 0.5f + 0.2f, pos.y - .5f); //Vector2 wspó³rzêdne x i y
        Vector2 originMiddle = new Vector2 (pos.x, pos.y - .5f);
        Vector2 originRight = new Vector2 (pos.x + 0.5f - 0.2f, pos.y - .5f); // .5f po³owa wielkoœci przeciwnika

        RaycastHit2D groundLeft = Physics2D.Raycast (originLeft, Vector2.down, velocity.y * Time.deltaTime, floorMask);
        RaycastHit2D groundMiddle = Physics2D.Raycast (originMiddle, Vector2.down, velocity.y * Time.deltaTime, floorMask);
        RaycastHit2D groundRight = Physics2D.Raycast (originRight, Vector2.down, velocity.y * Time.deltaTime, floorMask);

        if (groundLeft.collider != null || groundMiddle.collider != null || groundRight.collider != null) // sprawdzenie czy wartoœci s¹ przypisane // referencja do obiektu, który uderzy przeciwnik
        {
            RaycastHit2D hitRay = groundLeft;
            if (groundLeft)
            {
                hitRay = groundLeft;
            }
            else if (groundMiddle)
            {
                hitRay = groundMiddle;
            }
            else if (groundRight)
            {
                hitRay = groundRight;
            }

            if (hitRay.collider.tag == "Player") // jeœli przeciwnik dotknie gracza to koniec gry gracz to tag 'Player'
            {
                SceneManager.LoadScene("GameOver"); // koniec gry przechodzi do tej sceny o nazwie 'Game Over'
            }

            pos.y = hitRay.collider.bounds.center.y + hitRay.collider.bounds.size.y / 2 + .5f; //oddzia³ywanie przeciwnika na ziemi

            grounded = true; // przeciwnik znajduje siê na ziemi

            velocity.y = 0; //predkoœæ ruchu 0

            state = EnemyState.walking; // zmiana stanu przeciwnika na chodzenie

        }
        else
        {
            if(state != EnemyState.falling)
            {
                Fall ();
            }
        }
        return pos;
    }

    void CheckWalls (Vector3 pos, float direction) //funkcja sprawdza czy s¹ œciany na drodze przeciwnika / 1 funkcja do sprawdzenia wszystkich œcian
    {
        Vector2 originTop = new Vector2 (pos.x + direction * 0.4f, pos.y + .5f - 0.2f); //vector2 oœ x i y // direction czyli kierunek jest równy x = 1(w prawo) lub x = -1(w lewo)
        Vector2 originMiddle = new Vector2 (pos.x + direction * 0.4f, pos.y); //tylko pos.y bo jest wycentrowane
        Vector2 originBottom = new Vector2 (pos.x + direction * 0.4f, pos.y - .5f + 0.2f);

        RaycastHit2D wallTop = Physics2D.Raycast (originTop, new Vector2 (direction, 0), velocity.x * Time.deltaTime, wallMask);
        RaycastHit2D wallMiddle = Physics2D.Raycast (originMiddle, new Vector2 (direction, 0), velocity.x * Time.deltaTime, wallMask); //vector2 (1,0) lub vector2 (-1,0) zale¿nie od kierunku
        RaycastHit2D wallBottom = Physics2D.Raycast (originBottom, new Vector2 (direction, 0), velocity.x * Time.deltaTime, wallMask);

        if (wallTop.collider != null || wallMiddle.collider != null || wallBottom.collider != null) // sprawdzenie czy wartoœci s¹ przypisane // referencja do obiektu, który uderzy przeciwnik
        {
            RaycastHit2D hitRay = wallTop;

            if (wallTop) //jeœli gówny obiekt
            {
                hitRay = wallTop; //to uderzenie w górny obiekt itd
            }
            else if (wallMiddle)
            {
                hitRay = wallMiddle;
            }
            else if (wallBottom)
            {
                hitRay = wallBottom;
            }

            if (hitRay.collider.tag == "Player") // jeœli przeciwnik dotknie gracza to koniec gry gracz to tag 'Player'
            {
                SceneManager.LoadScene("GameOver"); // koniec gry przechodzi do tej sceny o nazwie 'Game Over'
            }

            isWalkingLeft = !isWalkingLeft; // zmiana kierunku poruszania siê przeciwnika (z lewej na praw¹)


        }
    }



    void OnBecameVisible() // funkcja gdy tylko bêdzie widoczny przeciwnik, to kod zadzia³a i przeciwnik bêdzie sie porusza³ (przeciwnik widoczny dla gracza)
    {
        enabled = true; // uruchomienie skryptu
    }

    void Fall ()//funkcja Opad przeciwnika np. przeciwnik spadnie z platformy
    {
        velocity.y = 0; // prêdkoœæ poruszania siê

        state = EnemyState.falling; // zmiana stanu przeciwnika na opadanie

        grounded = false; // przeciwnik nie znajduje siê na ziemi // podczas spadania

    }
}
