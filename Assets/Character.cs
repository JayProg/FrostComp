using UnityEngine;
using System.Collections;

[System.Serializable]
public class Character {
    public string Name, SoldierType, Notes;
    public int CurHealth, XP;
    public CharacterStat[] Stats;

    public Character()
    {
        Stats = new CharacterStat[6];
        Stats[0] = new CharacterStat() { Name = "Move" };
        Stats[1] = new CharacterStat() { Name = "Fight" };
        Stats[2] = new CharacterStat() { Name = "Shoot" };
        Stats[3] = new CharacterStat() { Name = "Armor" };
        Stats[4] = new CharacterStat() { Name = "Will" };
        Stats[5] = new CharacterStat() { Name = "Health" };
    }

    public CharacterStat GetStat(string name)
    {
        for (int i = 0; i < Stats.Length; i++)
        {
            if (Stats[i].Name == name)
                return Stats[i];
        }

        return null;
    }
}