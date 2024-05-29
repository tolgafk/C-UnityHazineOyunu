using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EngelMove : MonoBehaviour
{
    public GameObject kendi;
    public GameObject yol;
    private Transform tr;
    private int i;
    private int hedef;
    void Start()
    {
        tr = transform;
        InvokeRepeating("OtoHareket", 0f, 1f);
        if (kendi.tag == "arý") hedef = -3;
        if (kendi.tag == "kus") hedef = -5;
        for(int j = hedef; j <= hedef* (-1); j++)
        {
            if (kendi.tag == "arý")  Instantiate(yol, new Vector2(tr.position.x + j, tr.position.y), Quaternion.identity);
            if (kendi.tag == "kus") Instantiate(yol, new Vector2(tr.position.x, tr.position.y + j), Quaternion.identity);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (kendi.tag == "arý")
        {
            if (i == -3) hedef = 3;
            else if (i == 3) hedef = -3;
        }
        if (kendi.tag == "kus")
        {
            if( i == -5) hedef = 5;
            else if(i == 5) hedef = -5;
        }
    }

    private void OtoHareket()
    {
        if(i < hedef)
        {
            i++;
            if (kendi.tag == "arý") tr.position += (Vector3)Vector2.right;
            if (kendi.tag == "kus") tr.position += (Vector3)Vector2.up;
        }
        if (i > hedef)
        {
            i--;
            if (kendi.tag == "arý") tr.position += (Vector3)Vector2.left;
            if (kendi.tag == "kus") tr.position += (Vector3)Vector2.down;
        }
    }
}
