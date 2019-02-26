﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RabbitJump : MonoBehaviour
{
    public static bool beingCarried = false;
    public float actionDist;
    private Rigidbody2D rb;
    private double currentPosX;
    private double lastPosX;

    public Animator anim;
   
    // Start is called before the first frame update
    void Start()
    {
        lastPosX = transform.position.x;
        actionDist = 4f;
    }

    // Update is called once per frame
    void Update()
    {
        //DetectMovement();
        JumpIntoArms();
    }

    public void JumpIntoArms()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (beingCarried)
            {
                CancelInvoke("RabbitHappiness");
                transform.parent = null;
                GetComponent<Movement>().enabled = true;
                beingCarried = false;
                GetComponent<SortRender>().offset = 10;
                EmoControl.rabbitHug = false;
                anim.SetBool("isCarried", false);
            }
            else
            {
                //Raycast hit register for mouse position
                RaycastHit2D hit = Physics2D.CircleCast(transform.position, actionDist, Vector2.zero);

                //Check if an avatar was clicked on
                if (hit.collider != null && (hit.collider.gameObject.tag == "Person" || hit.collider.gameObject.tag == "MC"))
                {
                    //Check distance from object
                    //Debug.Log("I want to jump into " + hit.collider.gameObject.name + "'s arms");
                    float distance = Vector2.Distance(transform.position, hit.collider.gameObject.transform.position);
                    
                    beingCarried = true;
                    anim.SetBool("isCarried", true);
                    transform.position = new Vector3(hit.collider.gameObject.transform.position.x + 0.1f, hit.collider.gameObject.transform.position.y, -1);
                    transform.parent = hit.collider.gameObject.transform;
                    if (hit.collider.gameObject.name == "MC")
                    {
                        EmoControl.rabbitHug = true;
                        InvokeRepeating("RabbitHappiness", 0f, 3f);
                    }
                        
                    GetComponent<Movement>().enabled = false;
                    GetComponent<SortRender>().offset = 0;
                    //Debug.Log("I'm being carried");
                }
            } 
        }else if (Input.GetKeyDown(KeyCode.E))
        { 
            //Rabbit bite code!
            //Send out circle cast to see who's around to munch on
            RaycastHit2D biteCheck = Physics2D.CircleCast(transform.position, actionDist, Vector2.zero);

            if(biteCheck.collider != null && (biteCheck.collider.gameObject.tag == "Person" || biteCheck.collider.gameObject.tag == "MC"))
            {
                Debug.Log("I bit " + biteCheck.collider.gameObject.name + "!");
                if (biteCheck.collider.gameObject.tag == "MC")
                {
                    MentalState.sendMsg("Bit by rabbit");
                }
            }
        }
    }

    private void RabbitHappiness()
    {
        MentalState.sendMsg("Held Rabbit");
    }
}
