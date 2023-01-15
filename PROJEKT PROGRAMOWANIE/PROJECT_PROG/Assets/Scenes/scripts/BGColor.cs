using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGColor : MonoBehaviour
{
    public void ChangeToLight()
    {
        GameObject.Find("RememberLevel").GetComponent<RememberLevel>().BGcolor = Color.cyan;
   

    }
    public void ChangeToDark()
    {
        GameObject.Find("RememberLevel").GetComponent<RememberLevel>().BGcolor = Color.blue;

    }
}
