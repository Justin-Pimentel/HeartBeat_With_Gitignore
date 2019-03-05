﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadioControl : MonoBehaviour
{
    // Start is called before the first frame update
    public static int currentMood = 0;
    public static string musicListener = "";
    public static bool mcIsAffected = false;
    public static bool npcIsAffected = false;
    public static bool isMusic = false;

    private bool isBG;

    public LayerMask Carriers;
    public ParticleSystem ps;
   
    private SpriteRenderer sr;
    private enum Mood { happy, sad, idle};
    [SerializeField] private AudioClip sadSong;
    [SerializeField] private AudioClip happySong;
    private AudioSource audioSource;
    private AudioSource backgroundMusic;

    public Sprite happy;
    public Sprite sad;
    public Sprite idle;

    public static float actionDist;

    Sprite[] sprites;
    AudioClip[] audioClips;
    Color[] particleColors;


    private void Start()
    {
        sprites = new Sprite[] { happy, sad, idle};
        audioClips = new AudioClip[] { happySong, sadSong };
        particleColors = new Color[] { Color.white, Color.cyan };
        currentMood = (int) Mood.idle;

        sr = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
        backgroundMusic = GameObject.Find("/GameController").GetComponent<AudioSource>();

        isBG = true;
        ps.Stop();
        actionDist = 4f;

    }

    // Update is called once per frame
    private void Update()
    {
        UIControl();
        if (characterSwitcher.isMusicGuyInCharge)
        {
            DetectAction();
            DetectMusic();
        }
        else
        {
            ps.Stop();
            TurnBgOn();
            ResetThisGuy();
        }
    }

    private void DetectAction()
    {
        if (Input.GetKeyDown(Control.positiveAction) && currentMood != 0)
        {
            PlaySong(0);
            TurnBgOff();
        }
        else if (Input.GetKeyDown(Control.negativeAction) && currentMood != 1)
        {
            PlaySong(1);
            TurnBgOff();
        }
        else if ((Input.GetKeyDown(Control.negativeAction) && currentMood == 1) || (Input.GetKeyDown(Control.positiveAction) && currentMood == 0))
        {
            ResetThisGuy();
            TurnBgOn();
        }
    }

    private void DetectMusic()
    {
        if (isMusic)
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, actionDist, Carriers);

            //Check if array is empty or if there was anything collided with
            // Codes from Justin's Rabbit script 
            if (colliders.Length != 0)
            {
                Array.Reverse(colliders);
                foreach (Collider2D coll in colliders)
                {
                    if (coll.gameObject.tag == "MC" && !mcIsAffected)
                    {
                        mcIsAffected = true;
                        // 3 seconds later call this function and reset MC 
                        Invoke("McNotAffected", 3f);
                    }
                    else if (coll.gameObject.tag == "Person" && !mcIsAffected && !npcIsAffected)
                    {
                        npcIsAffected = true;
                        musicListener = coll.gameObject.name;
                        //Debug.Log(musicListener);
                        // 3 seconds later reset npc
                        Invoke("NpcNotAffected", 3f);
                        break;
                    }
                }
            }
        }
        else
        {
            // reset 
            mcIsAffected = false;
            npcIsAffected = false;
            musicListener = "";

        }

    }
    // play songs, change sprites and particles according to the mood 0=happy 1=sad 
    private void PlaySong(int index)
    {
        currentMood = index==0? (int)Mood.happy: (int)Mood.sad;

        audioSource.clip = audioClips[index];

        sr.sprite = sprites[index];

        audioSource.Play();

        EmitParticles(index);

        isMusic = true;

    }
   

    private void EmitParticles(int index)
    {

        ps.startColor = particleColors[index];
        ps.Play();
    }

    private void ResetThisGuy()
    {
        currentMood = (int)Mood.idle;
        sr.sprite = sprites[currentMood];
        audioSource.clip = null;
        audioSource.Pause();
        isMusic = false;
    }
    private void TurnBgOff()
    {
        if (isBG && currentMood != (int)Mood.idle)
        {

            backgroundMusic.Pause();
            isBG = false;
        }
    }

    private void TurnBgOn()
    {
        if (!isBG)
        {
            backgroundMusic.Play();
            isBG = true;
        }
    }

    private void UIControl()
    {
        if (PauseUI.IsPaused)
        {
            audioSource.Pause();
        }
        else if (!PauseUI.IsPaused)
        {
            audioSource.UnPause();
        }
    }

    private void McNotAffected()
    {
        // doesne't send message from emoControl anymore 
        var msg = currentMood == 0 ? "Happy Song" : "Sad Song";
        MentalState.sendMsg(msg);
        mcIsAffected = false;
    }

    private void NpcNotAffected()
    {
        npcIsAffected = false;
    }
}
