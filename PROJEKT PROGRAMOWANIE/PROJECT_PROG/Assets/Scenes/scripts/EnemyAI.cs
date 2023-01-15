using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyAI : MonoBehaviour // skrypt zachowania przeciwnika, uruchomi si� gdy przeciwnik pojawi si� w zasi�gu widoczno�ci gracza
{
    public float gravity; // grawitacja dla przeciwnika np. opadanie z platformy
    public Vector2 velocity; // przedko�� ruchu przeciwnika
    public bool isWalkingLeft = true; // czy przeciwnik chodzi w lew� stron� domy�lnie na tak
    
    public LayerMask floorMask; //Maska dla pod�o�a
    public LayerMask wallMask; //Maska dla �cian
    
    private bool grounded = false; //czy przecownik jest na ziemi

    private bool shouldDie = false; // czy przeciwnik powinien umrze� domy�lnie ustawione na nie / umrze po interakcji z graczem je�li ten na niego skoczy
    private float deathTimer = 0; // przeciwnik nie umrze natychmiast 

    public float timeBeforeDestroy = 1.0f;// czas znikni�cia z ekranu 1 sekunda


    private enum EnemyState //Stany przeciwnika
    {
        walking, //ch�d
        falling, //spadanie
        dead //�mier�
    }

    private EnemyState state = EnemyState.falling;

    // Start is called before the first frame update
    void Start()
    {
        enabled = false; // zatrzymuje dzia�anie kodu, �eby przeciwnik nie porusza� si� w stron� gracza, od razu po wystartowaniu poziomu, lecz gdy znajdzie si� w jego zasi�gu wzroku
        Fall (); // uruchomienie funkcji opadania przeciwnika
    }

    // Update is called once per frame
    void Update()
    {
        UpdateEnemyPosition();// uruchamianie funkcji aktualizacji pozycji przeciwnika

        CheckCrushed();// aktualizacja funkcji sprawdzania czy przeciwnik zosta� zgnieciony
    }
    // ta funkcja Crush() b�dzie dziedziczona do klasy Player
    public void Crush () // funckja zgniecienia przeciwnika
    {
        state = EnemyState.dead; //zmiana stanu przeciwnika na �mier�

        GetComponent<Animator>().SetBool("isCrushed", true); // zmiana animacji chodzenia na zgniecion� postur� przeciwnika z asset�w animacji o nazwi� isCrushed

        GetComponent<Collider2D>().enabled = false; // false, aby gracz nie m�g� ju� przeprowadza� interakcji z przeciwnikiem

        shouldDie = true;
    }
    void CheckCrushed () // funkcja sprawdzania czy przeciwnik zosta� zgnieciony / zabity
    {
        if (shouldDie) // je�eli powinien umrze� to
        {
            if (deathTimer <= timeBeforeDestroy) // czas umierania jest mniejszy lub r�wny czasowi zanim zostanie zniszczony deathTimer = 0  timeBeforeDestroy = 1 sekunda
            {
                deathTimer += Time.deltaTime; // do czasu umierania doda� i przypisa� czas tej operacji // Time.deltaTime czas od jednej klatki do nastepnej frame to the next frame
            }
            else
            {
                shouldDie = false;

                Destroy(this.gameObject); // Nast�puj� zniszczenie przeciwnika
            }
        }

    }

    void UpdateEnemyPosition() // funkcja aktualizacji pozycji przeciwnika
    {
        if (state != EnemyState.dead) // je�eli stan nie jest r�wny stanu �mierci to aktualizuje pozycje przeciwnika
        {
            Vector3 pos = transform.localPosition; // pozycja
            Vector3 scale = transform.localScale;// przeciwnik ma tak� sam� skal�/rozmiar

            if(state == EnemyState.falling)
            {
                pos.y += velocity.y * Time.deltaTime; // pozycja zwi�ksza si� z czasem
                velocity.y -= gravity * Time.deltaTime; // pr�dko�� ruchu zmniejsza si� z czasem
            }
            if (state == EnemyState.walking)
            {
                if (isWalkingLeft)
                {
                    pos.x -= velocity.x * Time.deltaTime;

                    scale.x = -1; // -1 poniewa� przeciwnik kieruje si� w lewo w tym przypadku

                }
                else
                {
                    pos.x += velocity.x * Time.deltaTime;

                    scale.x = 1; // 1 poniewa� przeciwnik kieruje si� w prawo w tym przypadku
                }
            }
            if (velocity.y <= 0)
                pos = CheckGround (pos);
            CheckWalls (pos, scale.x);

            transform.localPosition = pos; //przyr�wnanie lokalnej pozycji do zmiennej pos
            transform.localScale = scale; //przyr�wnanie lokalnej skali do zmiennej scale

        }
    }

    Vector3 CheckGround (Vector3 pos) // funkcja sprawdzenia czy jest powierzchnia
    {   //Za pomoc� originLeft, Middle, Right mo�na stworzy� obiekty, kt�re mo�e uderzy� przeciwnik
        Vector2 originLeft = new Vector2 (pos.x - 0.5f + 0.2f, pos.y - .5f); //Vector2 wsp�rz�dne x i y
        Vector2 originMiddle = new Vector2 (pos.x, pos.y - .5f);
        Vector2 originRight = new Vector2 (pos.x + 0.5f - 0.2f, pos.y - .5f); // .5f po�owa wielko�ci przeciwnika

        RaycastHit2D groundLeft = Physics2D.Raycast (originLeft, Vector2.down, velocity.y * Time.deltaTime, floorMask);
        RaycastHit2D groundMiddle = Physics2D.Raycast (originMiddle, Vector2.down, velocity.y * Time.deltaTime, floorMask);
        RaycastHit2D groundRight = Physics2D.Raycast (originRight, Vector2.down, velocity.y * Time.deltaTime, floorMask);

        if (groundLeft.collider != null || groundMiddle.collider != null || groundRight.collider != null) // sprawdzenie czy warto�ci s� przypisane // referencja do obiektu, kt�ry uderzy przeciwnik
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

            if (hitRay.collider.tag == "Player") // je�li przeciwnik dotknie gracza to koniec gry gracz to tag 'Player'
            {
                SceneManager.LoadScene("GameOver"); // koniec gry przechodzi do tej sceny o nazwie 'Game Over'
            }

            pos.y = hitRay.collider.bounds.center.y + hitRay.collider.bounds.size.y / 2 + .5f; //oddzia�ywanie przeciwnika na ziemi

            grounded = true; // przeciwnik znajduje si� na ziemi

            velocity.y = 0; //predko�� ruchu 0

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

    void CheckWalls (Vector3 pos, float direction) //funkcja sprawdza czy s� �ciany na drodze przeciwnika / 1 funkcja do sprawdzenia wszystkich �cian
    {
        Vector2 originTop = new Vector2 (pos.x + direction * 0.4f, pos.y + .5f - 0.2f); //vector2 o� x i y // direction czyli kierunek jest r�wny x = 1(w prawo) lub x = -1(w lewo)
        Vector2 originMiddle = new Vector2 (pos.x + direction * 0.4f, pos.y); //tylko pos.y bo jest wycentrowane
        Vector2 originBottom = new Vector2 (pos.x + direction * 0.4f, pos.y - .5f + 0.2f);

        RaycastHit2D wallTop = Physics2D.Raycast (originTop, new Vector2 (direction, 0), velocity.x * Time.deltaTime, wallMask);
        RaycastHit2D wallMiddle = Physics2D.Raycast (originMiddle, new Vector2 (direction, 0), velocity.x * Time.deltaTime, wallMask); //vector2 (1,0) lub vector2 (-1,0) zale�nie od kierunku
        RaycastHit2D wallBottom = Physics2D.Raycast (originBottom, new Vector2 (direction, 0), velocity.x * Time.deltaTime, wallMask);

        if (wallTop.collider != null || wallMiddle.collider != null || wallBottom.collider != null) // sprawdzenie czy warto�ci s� przypisane // referencja do obiektu, kt�ry uderzy przeciwnik
        {
            RaycastHit2D hitRay = wallTop;

            if (wallTop) //je�li g�wny obiekt
            {
                hitRay = wallTop; //to uderzenie w g�rny obiekt itd
            }
            else if (wallMiddle)
            {
                hitRay = wallMiddle;
            }
            else if (wallBottom)
            {
                hitRay = wallBottom;
            }

            if (hitRay.collider.tag == "Player") // je�li przeciwnik dotknie gracza to koniec gry gracz to tag 'Player'
            {
                SceneManager.LoadScene("GameOver"); // koniec gry przechodzi do tej sceny o nazwie 'Game Over'
            }

            isWalkingLeft = !isWalkingLeft; // zmiana kierunku poruszania si� przeciwnika (z lewej na praw�)


        }
    }



    void OnBecameVisible() // funkcja gdy tylko b�dzie widoczny przeciwnik, to kod zadzia�a i przeciwnik b�dzie sie porusza� (przeciwnik widoczny dla gracza)
    {
        enabled = true; // uruchomienie skryptu
    }

    void Fall ()//funkcja Opad przeciwnika np. przeciwnik spadnie z platformy
    {
        velocity.y = 0; // pr�dko�� poruszania si�

        state = EnemyState.falling; // zmiana stanu przeciwnika na opadanie

        grounded = false; // przeciwnik nie znajduje si� na ziemi // podczas spadania

    }
}
