using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controls : MonoBehaviour
{
    public void ChangeToAD()
    {
        if (GameObject.Find("RememberLevel").GetComponent<RememberLevel>().ChangeControls == true)
        {
            GameObject.Find("RememberLevel").GetComponent<RememberLevel>().ChangeControls = false;
        }
           
       
    }
    public void ChangeToArrows()
    {
        if (GameObject.Find("RememberLevel").GetComponent<RememberLevel>().ChangeControls == false)
        {
            GameObject.Find("RememberLevel").GetComponent<RememberLevel>().ChangeControls = true;

        }
    }
}
