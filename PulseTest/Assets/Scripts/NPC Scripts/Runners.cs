﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.AI; may need this in the future??

    //Will rework the runner to be the bully instead
public class Runners : NPCs
{
    public static bool bullying;
    private bool stopBullying;
    private float bullyTimer;
    private float bullyTime;

    protected override void Awake()
    {
        base.Awake();
        int ranX = Random.Range((int)Playground.LeftX, (int)Playground.RightX);
        int ranY = Random.Range((int)Playground.LowerY, (int)Playground.UpperY);
        target = new Vector3(ranX, ranY, -1);
        bullyTime = 5f;
        bullying = false;
        stopBullying = false;
    }

    protected override void Update()
    {
        if (schoolBell == false)
        {
            time = Time.fixedUnscaledTime;
            directionCheck(target.x, transform.position.x);
            avatarChecks();
            if (stopBullying == false)
            {
                checkMC();
            }
            else
            {
                walkAround();
            }
            /*
            transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
            if (transform.position == target)
            {
                int ranX = Random.Range((int)Playground.LeftX, (int)Playground.RightX);
                int ranY = Random.Range((int)Playground.LowerY, (int)Playground.UpperY);
                target = new Vector3(ranX, ranY, -1);
            }
            */
            DetectMovement();
        }
        else
        {
            toClass();
        }
    }

    private void checkMC()
    {
        bool mcDist = checkDist(transform.position, NpcInstantiator.mcPos);
        if (mcDist)
        {
            bullying = true;
            //Insert bully effects
            if(bullyTimer >= bullyTime)
            {
                //Decrement mood
                Debug.Log("Affecting mood");
                MentalState.currentState -= 1;
                bullyTimer = 0f;
            }
            else
            {
                bullyTimer += Time.deltaTime;
            }

            float dist = Vector3.Distance(NpcInstantiator.mcPos, transform.position);
            target = NpcInstantiator.mcPos;
            if (dist > 15.0f)
            {
                //target = NpcInstantiator.mcPos;
                transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
            }
            else
            {
                //PUT THE ANIMATION FOR LAUGHING HEREEEE
                //
                //
                //
                //
                anim.SetTrigger("taunt");
                   //
                   //
                   //
                //RIGHT HERE YOU POOPMATE
            }
        }
        else
        {
            /*
            bullying = false;
            transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
            if (transform.position == target)
            {
                int ranX = Random.Range((int)Playground.LeftX, (int)Playground.RightX);
                int ranY = Random.Range((int)Playground.LowerY, (int)Playground.UpperY);
                target = new Vector3(ranX, ranY, -1);
            }
            */
            walkAround();
        }
    }

    private void walkAround()
    {
        bullying = false;
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
        if (transform.position == target)
        {
            int ranX = Random.Range((int)Playground.LeftX, (int)Playground.RightX);
            int ranY = Random.Range((int)Playground.LowerY, (int)Playground.UpperY);
            target = new Vector3(ranX, ranY, -1);
        }
    }

    protected override void playBall()
    {
        if (nameChange)
        {
            int count = transform.childCount;
            for (int i = 0; i < count; i++)
            {
                if (transform.GetChild(i).gameObject.tag != "Avatars" && holdBunny == false)
                {
                    GameObject.Destroy(transform.GetChild(i).gameObject);
                }
            }
            if (BallProjectile.meanBallThrown)
            {
                stopBullying = true;
                anim.SetTrigger("isHit");
                timer = time + 2.0f;
                Emo = master.GetComponent<NpcInstantiator>().madFace;
                addQueue(2);
                addEmo();
                BallProjectile.meanBallThrown = false;
            }
            else
            {
                anim.SetTrigger("playCatch");
                addQueue(1);
            }
        }
    }

    protected override void checkRabbitBit()
    {
        base.checkRabbitBit();
        stopBullying = true;
    }

}
