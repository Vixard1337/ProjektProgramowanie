using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetBGColor : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<SpriteRenderer>().color = GameObject.Find("RememberLevel").GetComponent<RememberLevel>().BGcolor;
    }

 
}
