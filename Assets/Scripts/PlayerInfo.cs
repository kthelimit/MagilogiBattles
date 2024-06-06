using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo
{
    public string name;
    public int mp;
    public int tempMp;
    public int atk;
    public int def;
    public int bas;
    public int add;
    public int block;
    public int boost;
    public bool debuffTajim;
    public bool debuffHeoyak;
    public bool debuffByungma;
    public bool debuffChadan;

    public PlayerInfo() { }
    public PlayerInfo(string name, int mp, int tempmp, int atk, int def, int bas)
    {
        this.name = name;
        this.mp = mp;
        this.tempMp = tempmp;
        this.atk = atk;
        this.def = def;
        this.bas = bas;
        this.block = 0;
        this.boost = 0;
        this.debuffTajim = false;
        this.debuffHeoyak = false;
        this.debuffByungma = false;
        this.debuffChadan = false;
    }


}
