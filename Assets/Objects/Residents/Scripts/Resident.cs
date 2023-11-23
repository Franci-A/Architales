using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Residents/new Resident")]
public class Resident : ScriptableObject
{
    public Race race;
    public List<Race> likes;
    public List<Race> dislikes;
    public Color blockColor;
    public Material blockMaterial;

    public int CheckLikes(Race race)
    {
        if(likes.Contains(race))
        {
            return 1;
        }else if(dislikes.Contains(race)) 
        { 
            return -1;
        }
        else return 0;
    }
}

public enum Race
{
    Dragon, 
    Dwarf,
    Elf,
    Fairy,
    Mermaid,
    Empty,
    Orc
}