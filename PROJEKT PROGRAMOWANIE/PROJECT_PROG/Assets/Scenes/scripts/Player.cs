using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public float jumpVelocity; // predkosc skoku

    public float bounceVelocity; // pr�dko�� odbicia

    public Vector2 velocity; // dla osi x i y dlatego Vector2 x i y

    public float gravity; // grawitacja skoku

    //Layer Mask stosowanie warstw do poruszania i zarz�dzania dla platformami i �cianami
    public LayerMask wallMask; //maska �ciany
    public LayerMask floorMask; //maska pod�ogi do kolizji


    private bool walk, walk_left, walk_right, jump;

    public enum PlayerState // stan gracza czy skacze czy stoi czy idzie
    {
        jumping,
        idle, //bezczynno�� AFK
        walking,
        bouncing //odbicie od przeciwnika
    }

    private PlayerState playerState = PlayerState.idle;

    private bool grounded = false; //czy gracz jest na ziemi

    private bool bounce = false; // po zmia�dzeniu przeciwnika gracz odbije si� w g�r�

    // Start is called before the first frame update
    void Start()
    {
        //Fall (); // wywo�anie funkcji spadania
    }

    // Update is called once per frame
    void Update()
    {
        CheckPlayerInput(); // sprawdza medote wprowadzania przez u�ytkownika

        UpdatePlayerPosition(); // aktualizuje pozycje gracza

        UpdateAnimationStates(); // aktualizuje animacje ruchu gracza
    }
    void UpdatePlayerPosition()
    {
        Vector3 pos = transform.localPosition; // transform to gracz a vector3 bo os x,y,z pos - pozycja x=1 w prawo sie kieruje, x=-1 kieruje sie w lewo
        Vector3 scale = transform.localScale; // scala gracza

        if (walk) // je�li idziemy w prawo pozycja bedzie dodatnie, jezli w lewo bedzie ujemna
        {
            if (walk_left)
            {
                pos.x -= velocity.x * Time.deltaTime;

                scale.x = -1; // -1 poniewa� wtedy posta� gracza jest skierowana w lewo
            }

            if (walk_right)
            {
                pos.x += velocity.x * Time.deltaTime;

                scale.x = 1; // 1 poniewa� wtedy posta� gracza jest skierowana w prawo
            }

            pos = CheckWallRays(pos, scale.x);

        }

        if (jump && playerState != PlayerState.jumping)
        {
            playerState = PlayerState.jumping;
            velocity = new Vector2(velocity.x, jumpVelocity);
        }

        if(playerState == PlayerState.jumping)
        {
            pos.y += velocity.y * Time.deltaTime; //pozycja y zwi�ksza sie po nacisnieciu spacji

            velocity.y -= gravity * Time.deltaTime; //predko�� gracza zmieni sie gdy skoczy na dan� wysoko�� i zacznie opadac z czasem
        }

        if (bounce && playerState != PlayerState.bouncing) // je�li nast�puj� odbicie i stan gracza to nie odbicie od przeciwnika
        {
            playerState = PlayerState.bouncing; //ustaw stan gracza na odbicie
            velocity = new Vector2(velocity.x, bounceVelocity); //pr�dko�� x oraz pr�dko�� y to pr�dko�� odbicia 
        }

        if (playerState == PlayerState.bouncing)
        {
            pos.y += velocity.y * Time.deltaTime;

            velocity.y -= gravity * Time.deltaTime;
        }



        if (velocity.y <= 0)
            pos = CheckFloorRays (pos);
        if (velocity.y >= 0)
            pos = CheckCeilingRays (pos);

        transform.localPosition = pos; // przyrownanie pozycji do funcji pozycji gracza
        transform.localScale = scale; // przyrownanie skali do funcji skali gracza
    }

    void UpdateAnimationStates () // funkcja aktualizowania animacji ruchu, skoku i bezczynno�ci gracza //pobiera z asset�w
    {

        if (grounded && !walk && !bounce)//stan bezczynnosci animacja tylko jak gracz jest na ziemii / nie skacze i nie chodzi i si� nie odbija od przeciwnika
        {
            GetComponent<Animator>().SetBool("isJumping", false); // folder Animations>Player>nario_aminations sk�d nazwa isJumping
            GetComponent<Animator>().SetBool("isRunning", false); // folder Animations> Player > nario_aminations sk�d nazwa isRunning
        }
        if (grounded && walk)// stan chodzenia / gracz musi by� na ziemi
        {
            GetComponent<Animator>().SetBool("isJumping", false);
            GetComponent<Animator>().SetBool("isRunning", true);
        }
        if (playerState == PlayerState.jumping)// stan skoku 
        {
            GetComponent<Animator>().SetBool("isJumping", true);
            GetComponent<Animator>().SetBool("isRunning", false);
        }

    }



    void CheckPlayerInput()// funkcja sprawdzania sterowania
    {
        bool input_left;
        bool input_right;
        bool input_space;
        if (GameObject.Find("RememberLevel").GetComponent<RememberLevel>().ChangeControls == false)
        {
            input_left = Input.GetKey(KeyCode.A); // poruszanie w lewo
            input_right = Input.GetKey(KeyCode.D); // poruszanie w prawo
            input_space = Input.GetKeyDown(KeyCode.Space); // skok
        }
        else
        {
            input_left = Input.GetKey(KeyCode.LeftArrow); // poruszanie w lewo
            input_right = Input.GetKey(KeyCode.RightArrow); // poruszanie w prawo
            input_space = Input.GetKeyDown(KeyCode.Space); // skok
        }

        walk = input_left || input_right; // ruch jest prawd� je�li trzymamy strza�ki� w lewo lub w prawo
        walk_left = input_left && !input_right; // ruch w lewo jest prawd� je�li strza�ka w lewo jest wci�ni�ta
        walk_right = !input_left && input_right; // ruch w prawo jest prawd� je�li strza�ka w prawo jest wci�ni�ta
        jump = input_space; // skok jest prawd� je�li spacja jest wci�nieta
          
    }

    Vector3 CheckWallRays(Vector3 pos, float direction)// funkcja wyszukiwania kolizji ze �cianami
    {// pos.x gracza jest na �rodku gracza, direction=1 || direction=-1, wysoko�� gracza y=2
        Vector2 originTop = new Vector2(pos.x + direction * .4f, pos.y + 1f - 0.2f);
        Vector2 originMiddle = new Vector2(pos.x + direction * .4f, pos.y);
        Vector2 originBottom = new Vector2(pos.x + direction * .4f, pos.y - 1f + 0.2f);

        RaycastHit2D wallTop = Physics2D.Raycast(originTop, new Vector2(direction, 0), velocity.x * Time.deltaTime, wallMask);
        //Raycast uderzenie w obiekt g�rny
        RaycastHit2D wallMiddle = Physics2D.Raycast(originMiddle, new Vector2(direction, 0), velocity.x * Time.deltaTime, wallMask);
        //Raycast uderzenie w obiekt �rodkowy
        RaycastHit2D wallBottom = Physics2D.Raycast(originBottom, new Vector2(direction, 0), velocity.x * Time.deltaTime, wallMask);
        //Raycast uderzenie w obiekt dolny


        //je�li gracz dotknie obiektu, to nie b�dzie m�g� si� porusza� dalej w danym kierunku
        if (wallTop.collider != null || wallMiddle.collider != null || wallBottom.collider != null)
        {

            pos.x -= velocity.x * Time.deltaTime * direction;

        }

        return pos;

    }

    Vector3 CheckFloorRays (Vector3 pos) // sprawdza czy jest pod�oga poziomu
    {
        Vector2 originLeft = new Vector2 (pos.x - 0.5f + 0.2f, pos.y - 1f); //0.5f po�owa pozycji gracza grasz x=1 y=2 dlatego -0,5 i -1
        Vector2 originMiddle = new Vector2 (pos.x, pos.y - 1f);
        Vector2 originRight = new Vector2 (pos.x + 0.5f - 0.2f, pos.y - 1f);

        RaycastHit2D floorLeft = Physics2D.Raycast(originLeft, Vector2.down, velocity.y * Time.deltaTime, floorMask);
        RaycastHit2D floorMiddle = Physics2D.Raycast(originMiddle, Vector2.down, velocity.y * Time.deltaTime, floorMask);
        RaycastHit2D floorRight = Physics2D.Raycast(originRight, Vector2.down, velocity.y * Time.deltaTime, floorMask);

        if (floorLeft.collider != null || floorMiddle.collider != null || floorRight.collider != null) // sprawdzenie czy gracz napotka� przeszkod� z r�nych stron
        {
            RaycastHit2D hitRay = floorRight;

            if (floorLeft)
            {
                hitRay = floorLeft;
            }
            else if(floorMiddle)
            {
                hitRay = floorMiddle;
            }
            else if(floorRight)
            {
                hitRay = floorRight;
            }    

            if (hitRay.collider.tag == "Enemy") //Je�eli gracz dotknie przeciwnika, kt�ry jest pod nim to uruchomi si� funkcja Crush() z klasy EnemyAI (DZIEDZICZENIE)
            {
                bounce = true; //Je�li gracz zmia�dzy przeciwnika, to si� od niego odbije

                hitRay.collider.GetComponent<EnemyAI>().Crush();// DZIEDZICZENIE z Klasy ENEMYAI funkcji CRUSH()!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
            }

            playerState = PlayerState.idle;
            grounded = true;
            velocity.y = 0;
            pos.y = hitRay.collider.bounds.center.y + hitRay.collider.bounds.size.y / 2 + 1;
        }
        else
        {
            if(playerState != PlayerState.jumping)
            {
                Fall ();// funckja spadania po skoku
            }
        }

        return pos; //zwraca pozycje gracza
    }

    Vector3 CheckCeilingRays (Vector3 pos)// funkcja sprawdza, czy gracz dotyka obiekt�w np �cian, �eby nie m�g� przez nie przeskakiwa� bez kolizji
    {
        Vector2 originLeft = new Vector2(pos.x - 0.5f + 0.2f, pos.y + 1f); //0.5f po�owa pozycji gracza grasz x=1 y=2 dlatego -0,5 i -1
        Vector2 originMiddle = new Vector2(pos.x, pos.y + 1f);
        Vector2 originRight = new Vector2(pos.x + 0.5f - 0.2f, pos.y + 1f);

        RaycastHit2D ceilLeft = Physics2D.Raycast(originLeft, Vector2.up, velocity.y * Time.deltaTime, floorMask);// platforma, b�dzie pod�og�
        RaycastHit2D ceilMiddle = Physics2D.Raycast(originMiddle, Vector2.up, velocity.y * Time.deltaTime, floorMask);
        RaycastHit2D ceilRight = Physics2D.Raycast(originRight, Vector2.up, velocity.y * Time.deltaTime, floorMask);

        if (ceilLeft.collider != null || ceilMiddle.collider != null || ceilRight.collider != null) // sprawdzenie czy gracz napotka� przeszkod� z r�nych stron
        {
            RaycastHit2D hitRay = ceilLeft;

            if (ceilLeft)
            {
                hitRay = ceilLeft;
            }
            else if (ceilMiddle)
            {
                hitRay = ceilMiddle;
            }
            else if (ceilRight)
            {
                hitRay = ceilRight;
            }

            //FUNKCJA QuestionBlockBounce DZIEDZICZONA Z KLASY QUESTIONBLOCK
            if(hitRay.collider.tag == "QuestionBlock")
            {
                hitRay.collider.GetComponent<QuestionBlock>().QuestionBlockBounce();// U�ycie funkcji questionblockbounce() z klasy QuestionBlock

            }


            pos.y = hitRay.collider.bounds.center.y - hitRay.collider.bounds.size.y / 2 - 1;

            Fall ();// po uderzeniu w obiekt gracz spada na ziemie
        }
        return pos;// zwraca pozycje gracza
    }

    void Fall () //funckja spadania gracz, zawsze ma znale�� si� na ziemii
    {
        velocity.y = 0; //pr�dko�� po osi y

        playerState = PlayerState.jumping; //gracz musi by� w stanie skoku

        bounce = false; //je�li gracz spada to nie mo�e si� odbi�

        grounded = false; //nie mo�e by� na ziemii
    }

}
