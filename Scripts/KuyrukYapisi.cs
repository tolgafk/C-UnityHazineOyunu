using System;
using System.Collections;
using System.Collections.Generic;

// Kuyruk yapısını temsil eden sınıf
public class Kuyruk<T>
{
    public readonly List<T> _list = new List<T>();

    // Kuyruğa eleman ekler
    public void Ekle(T eleman)
    {
        _list.Add(eleman);
    }

    public void BasaEkle(T eleman)
    {
        _list.Insert(0, eleman);
    }

    // Kuyruktan eleman çıkarır
     public T Dequeue()
    {
        if (_list.Count == 0)
        {
            throw new InvalidOperationException("Kuyruk boş!");
        }

        T item = _list[0]; // Store the first element
        _list.RemoveAt(0); // Remove the first element
        return item;
    }
    // Kuyruğun başındaki elemanı gösterir

    
    public T Onizleme()
    {
        if (_list.Count == 0)
        {
            throw new InvalidOperationException("Kuyruk boş!");
        }

        return _list[0];
    }

    // Kuyrukta eleman olup olmadığını kontrol eder
    public bool Bosmu()
    {
        return _list.Count == 0;
    }

    // Kuyruktaki eleman sayısını gösterir
    public int ElemanSayisi()
    {
        return _list.Count;
    }

    // Kuyruğu temizler
    public void Temizle()
    {
        _list.Clear();
    }
}
