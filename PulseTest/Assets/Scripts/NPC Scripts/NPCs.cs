﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCs : MonoBehaviour
{
    public GameObject master;
    public Animator anim;
    protected float speed;
    protected Vector3 scale;
    protected Vector3 scaleOpposite;
    protected GameObject Emo;
    protected int music;
    protected int check;
    protected bool schoolBell;

    public NPCs()
    {
        master = GameObject.Find("GameController");

    }

    protected virtual void directionCheck(float target, float pos)
    {
        if (target >= 0)
        {
            if (pos >= 0)
            {
                if (target >= pos) { transform.localScale = scale; }
                else if (target <= pos) { transform.localScale = scaleOpposite; }
            }
            else if (pos <= 0) { transform.localScale = scale; }
        }
        else if (target <= 0)
        {
            if (pos >= 0) { transform.localScale = scaleOpposite; }
            else if (pos <= 0)
            {
                if (target >= pos) { transform.localScale = scale; }
                else if (target < pos) { transform.localScale = scaleOpposite; }
            }
        }
    }
}
