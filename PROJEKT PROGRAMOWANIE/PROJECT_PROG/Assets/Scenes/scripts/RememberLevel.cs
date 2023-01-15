using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RememberLevel : MonoBehaviour
{
    public int CurrentLevel;
    public bool ChangeControls;
    public Color BGcolor;
    public static RememberLevel Instance;
    void Awake()
    {
        if (Instance)
        {
            DestroyImmediate(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
    }
}
