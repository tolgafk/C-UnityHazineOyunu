using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoyutGirisi : MonoBehaviour
{

    public static int boyut = 70;

    public void getInput(string value)
    {
        boyut=Convert.ToInt32(value);
    }
}

