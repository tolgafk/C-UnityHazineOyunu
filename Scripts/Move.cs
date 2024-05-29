using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Move : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject player;
    public int boyut;
    private Transform tr;
    public static int adimSayisi = 0;
    void Awake()
    {
        tr = player.transform;
        
        InvokeRepeating("OtoHareket", 3f, 0.05f);
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 yeniPos = tr.localPosition;
        if (Input.GetKeyDown(KeyCode.W))
        {
            yeniPos.y += 1f;
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            yeniPos.y -= 1f;
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            yeniPos.x += 1f;
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            yeniPos.x -= 1f;
        }
        tr.localPosition = yeniPos;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        
        if (other.CompareTag("Chests"))
        {
            Destroy(other.gameObject);
            GameControl1.sandikSayac++;
        }

        if (other.CompareTag("Way"))
        {
            //Way.DrawW();
            Destroy(other.gameObject);
        }

        /*if (other.CompareTag("Fog"))
        {
            Destroy(other.gameObject);
        }*/
    }

    private void OtoHareket()
    {
        GameObject obj = GameObject.FindGameObjectWithTag("Way");
        if (obj != null)
        {
            tr.position = obj.transform.position;
            adimSayisi++;
        }
    }
}
