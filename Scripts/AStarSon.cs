using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AStarNode
{
    public Vector2 konum;
    public int gCost;
    public int hCost;
    public int fCost;
    public AStarNode parent;

    public AStarNode(Vector2 konum)
    {
        this.konum = konum;
        parent = null;
    }
}

class AStarSon
{
    public int boyut;
    public AStarSon(int boyut)
    {
        this.boyut = boyut;
    }

    
    public Kuyruk<Vector2> FindPath(Vector2 start1, Vector2 end1)
    {
        AStarNode start = new AStarNode(start1);
        AStarNode end = new AStarNode(end1);
        List<AStarNode> openList = new List<AStarNode>();
        List<AStarNode> closedList = new List<AStarNode>();

        openList.Add(start);
        while(openList.Count > 0)
        {
            AStarNode currentN = openList[0];
            int currentIndex = 0;

            //gidilmeyen komþu noktalarý þu an ki noktanýn maaliyetinden daha iyi mi diye kontrol ediyor
            for(int i = 0; i < openList.Count; i++)
            {
                if (openList[i].fCost < currentN.fCost || (openList[i].fCost == currentN.fCost && openList[i].hCost < currentN.hCost))
                {
                    currentN = openList[i];
                    currentIndex = i;
                }
            }
            openList.RemoveAt(currentIndex);
            closedList.Add(currentN);

            //sona varýldýysa kuyruða ekleyip fonksiyonu bitiriyor.
            if(currentN.konum == end.konum)
            {
                Kuyruk<Vector2> path = new Kuyruk<Vector2>();
                AStarNode current = currentN;
                while (current != null)
                {
                    path.BasaEkle(current.konum);
                    current = current.parent;
                }
                return path;
            }

            List<AStarNode> children = new List<AStarNode>();

            // komþu noktalar oyunun alanýnda mý veya engel var mý diye bakýp o noktaya gidiyor
            foreach(var newPos in new List<Vector2> { Vector2.down, Vector2.left, Vector2.up, Vector2.right })
            {
                Vector2 konum = currentN.konum + newPos;
                if (konum.x >= 0 && konum.x < boyut && konum.y >= 0 && konum.y < boyut && GameControl1.game[(int)konum.y, (int)konum.x] < 5)
                {
                    AStarNode newN = new AStarNode(konum);
                    newN.gCost = currentN.gCost + 1;
                    newN.hCost = Mathf.Abs((int)end.konum.x - (int)konum.x) + Mathf.Abs((int)end.konum.y - (int)konum.y);
                    newN.parent = currentN;
                    children.Add(newN);
                }
            }

            //bir noktaya daha kýsa bir þekilde ulaþýlýyorsa onun parenti ve deðeri güncelleniyor
            foreach(AStarNode child in children)
            {
                if (closedList.Exists(n => n.konum == child.konum)) continue;
                if(openList.Exists(n => n.konum == child.konum))
                {
                    AStarNode existingN = openList.Find(n => n.konum == child.konum);
                    if ((child.gCost < existingN.gCost))
                    {
                        existingN.gCost = child.gCost;
                        existingN.parent = child.parent;
                    }
                }
                else
                {
                    openList.Add(child);
                }
            }
        }
        Debug.Log("fail");
        return new Kuyruk<Vector2>();
        
    }

}