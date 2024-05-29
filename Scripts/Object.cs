
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;

public class Object
{
    public int id;
    public int[,] idBoyut = {
        {1, 1},//player  0
        {1, 1},//tahtaSandik  1
        {1, 1},//gumusSandik  2
        {1, 1},//altinSandik  3
        {1, 1},//zumrutSandik  4
        {2, 2},//agac1  5
        {3, 3},//agac2  6
        {4, 4},//agac3  7
        {5, 5},//agac4  8
        {2, 2},//kaya1  9 
        {3, 3},//kaya2  10
        {10, 1},//duvar  11
        {15, 15},//dag  12 
        {2, 2},//kus  13
        {2, 2},//ari 14
    };
    public static string GetClassName(int id)
    {
        if (id == 1) return "Tahta Sand�k";
        else if (id == 2) return "G�m�� Sand�k";
        else if (id == 3) return "Alt�n Sand�k";
        else if (id == 4) return "Z�mr�t Sand�k";
        if (id > 4 && id < 9) return "A�a�";
        if (id > 8 && id < 11) return "Kaya";
        if (id == 11) return "Duvar";
        if (id == 12) return "Da�";
        return null;
    }
    
    public int GetBoyut(int i, int j)
    {
        return this.idBoyut[i, j];
    }

    
}

public class Player : Object
{
    public Player(int id)
    {
        this.id = id;
        
    }
}

public class Sandik : Object
{
    public Sandik(int id)
    {
        this.id = id;

    }
}