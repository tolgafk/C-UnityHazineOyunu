using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Android;
using UnityEngine.Timeline;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;
using System.Diagnostics;
using Debug = UnityEngine.Debug;
using TMPro;
using UnityEngine.U2D.IK;

public class GameControl1 : MonoBehaviour
{
    public TMP_Text scoreText;
    public TMP_Text logText;
    public TMP_Text zümrütT;
    public TMP_Text altinT;
    public TMP_Text gumusT;
    public TMP_Text tahtaT;

    public GameObject[] prefabs;
    
    private GameObject player;
    public GameObject way;
    public GameObject zort;

    public Kuyruk<Vector2> ways = new Kuyruk<Vector2>();

    public int boyut = 70;

    public static int[,] game;
    public static int[,] fog;
    public static List<GameObject> fogs = new List<GameObject>();

    public List<Vector2> solAltNokta;
    public Object[] things = new Object[61];

    private Kuyruk<Vector2> hedefNokta = new Kuyruk<Vector2>();

    AStarSon astar;
    public static int sandikSayac = 0;
    private int gorulenSandikSayisi = 0;
    private int altin = 0;
    private int zümrüt = 0;
    private int gümüş = 0;
    private int tahta = 0;
    // Start is called before the first frame update
    void Awake()
    {
        this.boyut = BoyutGirisi.boyut;
        int i, j;
        astar = new AStarSon(boyut);
        game = new int[boyut, boyut];
        fog = new int[boyut, boyut];

        int sayac = 0;

        for (i = 0; i < boyut; i++)
        {
            for (j = 0; j < boyut; j++)
            {
                game[i, j] = -1;
            }
        }
        
        //player konumu belirleme
        randomKonum(0, sayac);
        sayac++;
        

        //chest belirleme
        for (j = 1; j < 5; j++)
        {
            for (i = 0; i < 5; i++)
            {
                randomKonum(j, sayac);
                sayac++;
            }
        }

        //dag belirleme
        for (i = 0; i < 3; i++)
        {
            randomKonum(12, sayac);
            sayac++;
        }
        //agac belirleme
        for(i = 0; i < 10; i++)
        {
            int id;
            randomKonum(id = Random.Range(5,9), sayac);
            sayac++;
        }
        //duvar belirleme
        for(i = 0; i < 6; i++)
        {
            randomKonum(11, sayac);
            sayac++;
        }
        //kaya belirleme
        for(i = 0; i < 10; i++)
        {
            int id;
            randomKonum(id = Random.Range(9, 11), sayac);
            sayac++;
        }

        //kuş ve arı
        randomKonum(13, sayac);
        sayac++;
        randomKonum(13, sayac);
        sayac++;
        randomKonum(14, sayac);
        sayac++;
        randomKonum(14, sayac);
        sayac++;

        drawField();

        int satir = 3;
        int satirSon = boyut - 1;
        int sutun = 3;
        int sutunSon = boyut - 1;
        //değiştirilecek
        Vector2 nokta = new Vector2();
        for(i = sutun; i <= sutunSon; i += 7)
        {
            if(j < boyut / 2)
            {
                for(j = satir; j <= satirSon; j+= 7)
                {
                    nokta = new Vector2(j, i);
                    hedefNokta.Ekle(nokta);
                }
                if(j - satirSon < 4 && j > satirSon)
                {
                    nokta = new Vector2(boyut - 3, i);
                    hedefNokta.Ekle(nokta);
                }
            }
            else if(j > boyut / 2)
            {
                for(j = satirSon; j >= satir;  j-= 7)
                {
                    nokta = new Vector2(j, i);
                    hedefNokta.Ekle(nokta);
                }
            }
        }
        
    }

    void Start()
    {
        scoreText.text = Move.adimSayisi.ToString();
        if (game[(int)hedefNokta.Onizleme().y, (int)hedefNokta.Onizleme().x] > 4)
        {
            Vector2 pos = hedefNokta.Onizleme();
            int k = 1;
            while (ways.Bosmu())
            {
                foreach (var newpos in new List<Vector2> { Vector2.down, Vector2.left, Vector2.up, Vector2.right, Vector2.down + Vector2.left, Vector2.down + Vector2.right, Vector2.up + Vector2.left, Vector2.up + Vector2.right })
                {
                    pos = hedefNokta.Onizleme() + (newpos * k);
                    if (pos.x >= 0 && pos.x < boyut && pos.y >= 0 && pos.y < boyut && GameControl1.game[(int)pos.y, (int)pos.x] < 5)
                    {
                        ways = astar.FindPath(player.transform.position, pos);
                        if (!ways.Bosmu())
                        {
                            hedefNokta.Dequeue();
                            hedefNokta.BasaEkle(pos);
                            break;
                        }
                    }
                }
                k++;
            }
        }
        else ways = astar.FindPath(player.transform.position, hedefNokta.Onizleme());
        
    }

    bool sandik = false;
    bool notRun = true;
    void Update()
    {
        zümrütT.text = zümrüt.ToString() + "/5";
        altinT.text = altin.ToString() + "/5";
        gumusT.text = gümüş.ToString() + "/5";
        tahtaT.text = tahta.ToString() + "/5";
        scoreText.text = Move.adimSayisi.ToString();
        if (sandikSayac == 20)
        {
            SceneManager.LoadScene("GameOver");
        }
        if (ways.Bosmu() && sandik)
        {
            ways = astar.FindPath(player.transform.position, hedefNokta.Onizleme());
            sandik = false;
        }
        if (!hedefNokta.Bosmu() && (Vector2)player.transform.position == hedefNokta.Onizleme())
        {
            hedefNokta.Dequeue();
            if (!hedefNokta.Bosmu())
            {
                if (game[(int)hedefNokta.Onizleme().y, (int)hedefNokta.Onizleme().x] > 4)
                {
                    Vector2 pos;
                    int k = 1;
                    while (ways.Bosmu())
                    {
                        foreach (var newpos in new List<Vector2> { Vector2.down, Vector2.left, Vector2.up, Vector2.right, Vector2.down + Vector2.left, Vector2.down + Vector2.right, Vector2.up + Vector2.left, Vector2.up + Vector2.right })
                        {
                            pos = hedefNokta.Onizleme() + (newpos * k);
                            if (pos.x >= 0 && pos.x < boyut && pos.y >= 0 && pos.y < boyut && game[(int)pos.y, (int)pos.x] < 5)
                            {
                                ways = astar.FindPath(player.transform.position, pos);
                                if (!ways.Bosmu())
                                {
                                    hedefNokta.Dequeue();
                                    hedefNokta.BasaEkle(pos);
                                    break;
                                }

                            }
                        }
                        k++;
                    }
                }
                else ways = astar.FindPath(player.transform.position, hedefNokta.Onizleme());
            }
        }

        while (!ways.Bosmu())
        {
            Instantiate(way, ways.Dequeue(), Quaternion.identity);
        }
        
        // ilerledikçe görülen şeyleri işlemek için
        for (int i = (int)solAltNokta[0].y - 3; i <= (int)solAltNokta[0].y + 3; i++)
        {
            for (int j = (int)solAltNokta[0].x - 3; j <= (int)solAltNokta[0].x + 3; j++)
            {
                try
                {
                    fog[i, j] = 0;
                    Vector3 kontrol = new Vector2(j, i);
                    foreach(GameObject fog in fogs)
                    {
                        if ( fog == null ) continue;
                        if(kontrol == fog.transform.position)
                        {
                            Destroy(fog);
                        }
                    }
                }
                catch (IndexOutOfRangeException e)
                {
                    continue;
                }
                
                if (game[i, j] > 0 && game[i, j] < 5)
                {
                    if (game[i, j] == 1) tahta++;
                    if (game[i, j] == 2) gümüş++;
                    if (game[i, j] == 3) altin++;
                    if (game[i, j] == 4) zümrüt++;
                    float distance = 10000;
                    sandik = true;
                    game[i, j] = -1;
                    Vector2 konum = new Vector2(j, i);
                    Vector2 closest = player.transform.position;
                    ways.Temizle();
                    gorulenSandikSayisi++;
                    GameObject[] objs = GameObject.FindGameObjectsWithTag("Way");
                    foreach(GameObject obj in objs)
                    {
                        if(Vector2.Distance(obj.transform.position, konum) < distance)
                        {
                            distance = Vector2.Distance(obj.transform.position, konum);
                            closest = obj.transform.position;
                        }
                        Destroy(obj);
                    }
                    //hedefNokta.BasaEkle(closest);
                    hedefNokta.BasaEkle(konum);
                }
                if (game[i, j] > 4 && game[i, j] < 13 && notRun)
                {
                    Debug.Log(Object.GetClassName(game[i, j]) + "Bulundu!");
                    notRun = false;
                }
            }
        }
        notRun = true;
        if (player != null)
        {
            solAltNokta[0] = player.transform.position;
        }
    }

    //idsine göre rastgele konum belirliyor ve sol alt noktalarını sol alta ekliyor
    private void randomKonum(int id, int sayac)
    {
        bool devam = true;
        while (devam)
        {
            Object temp = null;
            int konumX = Random.Range(0, boyut);
            int konumY = Random.Range(0, boyut);
            if(id == 0)
            {
                temp = new Player(id);
                things[sayac] = temp;
            }
            if(id >=1 && id <= 4)
            {
                temp = new Sandik(id);
                things[sayac] = temp;
            }
            if(id >= 5 && id <= 12)
            {
                temp = new HareketsizEngel(id, 0);
                things[sayac] = temp;
            }
            if(id == 13 || id == 14)
            {
                temp = new HareketliEngel(id);
                things[sayac] = temp;
            }
            if (kontrol(konumX, konumY, temp))
            {
                Vector2 solAlt = new Vector2(konumX, konumY);

                if (temp.id == 13)
                {
                    for (int j = konumY - 5; j < konumY + temp.GetBoyut(id, 1) + 5; j++)
                    {
                        for (int k = konumX; k < konumX + temp.GetBoyut(id, 0); k++)
                        {
                            try
                            {
                                game[j, k] = id;
                            }
                            catch (IndexOutOfRangeException e)
                            {
                                continue;
                            }
                        }
                    }
                }
                if (temp.id == 14)
                {
                    for (int j = konumY; j < konumY + temp.GetBoyut(id, 1); j++)
                    {
                        for (int k = konumX - 3; k < konumX + temp.GetBoyut(id, 0) + 3; k++)
                        {
                            try
                            {
                                game[j, k] = id;
                            }
                            catch (IndexOutOfRangeException e)
                            {
                                continue;
                            }
                        }
                    }
                }
                else
                {
                    for (int j = konumY; j < konumY + temp.GetBoyut(id, 1); j++)
                    {
                        for (int k = konumX; k < konumX + temp.GetBoyut(id, 0); k++)
                        {
                            try
                            {
                                game[j, k] = id;
                            }
                            catch (IndexOutOfRangeException e)
                            {
                                continue;
                            }
                        }
                    }
                }
                solAltNokta.Add(solAlt);
                return;
            }
            else continue;
        }
    }

    //engeller üst üste gelmesin diye
    private bool kontrol(int konumX, int konumY, Object temp)
    {
        if(temp.GetType().Name == "HareketliEngel")
        {
            if (temp.id == 13)
            {
                for (int j = konumY - 5; j < konumY + temp.GetBoyut(temp.id, 1) + 5; j++)
                {
                    for (int k = konumX; k < konumX + temp.GetBoyut(temp.id, 0); k++)
                    {
                        try
                        {
                            if (game[j, k] != -1)
                            {
                                return false;
                            }
                        }
                        catch (IndexOutOfRangeException e)
                        {
                            return false;
                        }
                    }
                }
            }
            else
            {
                for (int j = konumY; j < konumY + temp.GetBoyut(temp.id, 1); j++)
                {
                    for (int k = konumX - 3; k < konumX + temp.GetBoyut(temp.id, 0) + 3; k++)
                    {
                        try
                        {
                            if (game[j, k] != -1)
                            {
                                return false;
                            }
                        }
                        catch (IndexOutOfRangeException e)
                        {
                            return false;
                        }
                    }
                }
            }
        }
        else
        {
            for (int j = konumY; j < konumY + temp.GetBoyut(temp.id, 1); j++)
            {
                for (int k = konumX; k < konumX + temp.GetBoyut(temp.id, 0); k++)
                {
                    try
                    {
                        if (game[j, k] != -1)
                        {
                            return false;
                        }
                    }
                    catch (IndexOutOfRangeException e)
                    {
                        return false;
                    }
                }
            }
            if (temp.id == 0)
            {
                if (konumX < 4 || konumX > 100 - 3) return false;
                if (konumY < 4 || konumY > 100 - 3) return false;
            }
        }
        
        return true;
    }

    //hareketli engeller hariç doğru çalışıyor bulaşma
    private void drawField()
    {
        Vector2 merkez;

        foreach (Vector2 solAlt in solAltNokta)
        {
            switch (game[(int)solAlt.y, (int)solAlt.x])
            {
                case 0:
                    player = Instantiate(prefabs[0], solAlt, Quaternion.identity);
                    break;
                case 1:
                    Instantiate(prefabs[1], solAlt, Quaternion.identity);
                    break;
                case 2:
                    Instantiate(prefabs[2], solAlt, Quaternion.identity);
                    break;
                case 3:
                    Instantiate(prefabs[3], solAlt, Quaternion.identity);
                    break;
                case 4:
                    Instantiate(prefabs[4], solAlt, Quaternion.identity);
                    break;

                case 5:
                    merkez.x = solAlt.x + 0.5f;
                    merkez.y = solAlt.y + 0.5f;
                    _ = (merkez.x < boyut / 2) ? Instantiate(prefabs[5], merkez, Quaternion.identity) : Instantiate(prefabs[15], merkez, Quaternion.identity);
                    break;
                case 6:
                    merkez.x = solAlt.x + 1f;
                    merkez.y = solAlt.y + 1f;
                    _ = (merkez.x < boyut / 2) ? Instantiate(prefabs[6], merkez, Quaternion.identity) : Instantiate(prefabs[16], merkez, Quaternion.identity);
                    break;
                case 7:
                    merkez.x = solAlt.x + 1.5f;
                    merkez.y = solAlt.y + 1.5f;
                    _ = (merkez.x < boyut / 2) ? Instantiate(prefabs[7], merkez, Quaternion.identity) : Instantiate(prefabs[17], merkez, Quaternion.identity);
                    break;
                case 8:
                    merkez.x = solAlt.x + 2f;
                    merkez.y = solAlt.y + 2f;
                    _ = (merkez.x < boyut / 2) ? Instantiate(prefabs[8], merkez, Quaternion.identity) : Instantiate(prefabs[18], merkez, Quaternion.identity);
                    break;
                case 9:
                    merkez.x = solAlt.x + 0.5f;
                    merkez.y = solAlt.y + 0.5f;
                    _ = (merkez.x < boyut / 2) ? Instantiate(prefabs[9], merkez, Quaternion.identity) : Instantiate(prefabs[19], merkez, Quaternion.identity);
                    break;
                case 10:
                    merkez.x = solAlt.x + 1f;
                    merkez.y = solAlt.y + 1f;
                    _ = (merkez.x < boyut / 2) ? Instantiate(prefabs[10], merkez, Quaternion.identity) : Instantiate(prefabs[20], merkez, Quaternion.identity);
                    break;
                case 11:
                    merkez.x = solAlt.x + 4.5f;
                    merkez.y = solAlt.y;
                    _ = (merkez.x < boyut / 2) ? Instantiate(prefabs[11], merkez, Quaternion.identity) : Instantiate(prefabs[21], merkez, Quaternion.identity);
                    break;
                case 12:
                    merkez.x = solAlt.x + 7F;
                    merkez.y = solAlt.y + 7F;
                    _ = (merkez.x < boyut / 2) ? Instantiate(prefabs[12], merkez, Quaternion.identity) : Instantiate(prefabs[22], merkez, Quaternion.identity);
                    break;
                case 13:
                    merkez.x = solAlt.x + 0.5f;
                    merkez.y = solAlt.y + 0.5f;
                    Instantiate(prefabs[13], merkez, Quaternion.identity);
                    break;
                case 14:
                    merkez.x = solAlt.x + 0.5f;
                    merkez.y = solAlt.y + 0.5f;
                    Instantiate(prefabs[14], merkez, Quaternion.identity);
                    break;

            }
        }
    }
}
