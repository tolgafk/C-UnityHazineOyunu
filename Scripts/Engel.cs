
using System.Collections;
using System.Collections.Generic;

public abstract class Engel : Object
{
    
    public int[] boyut = new int[2];
    
    
}

public class HareketsizEngel : Engel
{
    public HareketsizEngel(int id, int boyutId) 
    {
        this.id = id;
        
    }

    
}

public class HareketliEngel : Engel
{
    public string name;
    public HareketliEngel(int id) 
    {
        this.id=id;
        if(id == 13)
        {
            name = "Kuþ";
        }
        if (id == 14)
        {
            name = "Arý";
        }
        
        
    }
}
