using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; //odniesienie do gracza nazwa target
    public Transform leftBounds; //granica kamery lewa
    public Transform rightBounds; //granica kamery prawa

    public float smoothDampTime = 0.15f; // czas od pocz¹tkowej pozycji do docelowej pozycji
    private Vector3 smoothDampVelocity = Vector3.zero;

    private float camWidth, camHeight, levelMinX, levelMaxX;


    // Start is called before the first frame update
    void Start()
    {
        camHeight = Camera.main.orthographicSize * 2; // okreœlenie wysokoœci kamery
        camWidth = camHeight * Camera.main.aspect; // okreœlenie szerokoœci kamery

        float leftBoundsWidth = leftBounds.GetComponentInChildren<SpriteRenderer>().bounds.size.x / 2; // pivot point wyœrodkowany
        float rightBoundsWidth = rightBounds.GetComponentInChildren<SpriteRenderer>().bounds.size.x / 2; //2 bo potrzebujemy tylko po³owy

        levelMinX = leftBounds.position.x + leftBoundsWidth + (camWidth / 2);  //minumalna pozycja x kamery
        levelMaxX = rightBounds.position.x - rightBoundsWidth - (camWidth / 2); // maksymalna pozycja x kamery
    }



// Update is called once per frame
void Update()
{

    if (target)
    {

        float targetX = Mathf.Max(levelMinX, Mathf.Min(levelMaxX, target.position.x));
        float x = Mathf.SmoothDamp(transform.position.x, targetX, ref smoothDampVelocity.x, smoothDampTime);
        transform.position = new Vector3(x, transform.position.y, transform.position.z);
    }

}
}
