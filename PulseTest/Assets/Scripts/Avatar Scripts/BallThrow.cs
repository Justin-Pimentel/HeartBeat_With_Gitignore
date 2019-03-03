﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallThrow : MonoBehaviour
{
    public GameObject ball;
    public GameObject newBall;
    public float offset;
    public float pickupDist;
    public Animator anim;
    public bool thrownBall = false;
    public bool isMeanBall = false;
    

    private bool towardRight;

    // Start is called before the first frame update
    void Start()
    {
        anim.SetBool("hasBall", true);
        anim.SetBool("isThrowing", false);

        towardRight = true;
    }

    // Update is called once per frame
    void Update()
    {
        //Space bar for nice action and E for mean action
        if (Input.GetKeyDown(KeyCode.Space) && !thrownBall)
        {
            ThrowBall();
        }else if (Input.GetKeyDown(KeyCode.E) && !thrownBall)
        {
            isMeanBall = true;
            ThrowBall();
        }
       
        //Check to see that a ball was thrown and that it is resting stationary on the ground
        if (Input.GetKey(KeyCode.Space) && thrownBall && GameObject.Find("newBall") != null)
        {
            PickupBall();
        }
    }

    //Helper function to throw ball, reset animation, and stop motion while throwing
    void ThrowBall()
    {
        thrownBall = true;
        anim.SetBool("isThrowing", true);
        anim.SetBool("isThrowing", true);

        // postpone 0.6 seconds to finish the animation
        //Stop movement while throwing
        GameObject.Find("2").GetComponent<Movement>().enabled = false;
        StartCoroutine(PutOutBall());
        StartCoroutine(ResetAnimation());
    }

    //Function to instantiate ball when thrown
    IEnumerator PutOutBall()
    {
        yield return new WaitForSeconds(0.6f);
        //A temporary gameobject to store ball instantiation info
        GameObject tempBall;

        //Check if ball kid is facing left
        if (transform.localScale.x < 0)
        {
            //If so, decompose quaternion into vector3 to modify angles
            Vector3 tempRot = transform.rotation.eulerAngles;
            //Create new vector with modified rotation
            tempRot = new Vector3(tempRot.x, tempRot.y + 180, tempRot.z);
            //Turn it back into a quaternion for instantiate() to use
            Quaternion q = Quaternion.Euler(tempRot);
            //Instantiate ball facing other direction
            tempBall = Instantiate(ball, transform.position, q);
        }
        else
        {
            tempBall = Instantiate(ball, transform.position, transform.rotation);
        }

        //Update ballProjectile script with mean ball info
        BallProjectile.meanBallThrown = isMeanBall;

        anim.SetBool("hasBall", false);
        //Re-enable movement once animation has finished
        GameObject.Find("2").GetComponent<Movement>().enabled = true;
    }

    void PickupBall()
    {
        newBall = GameObject.Find("newBall");
        float distance = Vector3.Distance(transform.position, newBall.transform.position);

        if(distance < pickupDist)
        {
            thrownBall = false;
            isMeanBall = false;
            BallProjectile.meanBallThrown = false;
            Destroy(newBall);
            anim.SetBool("hasBall", true);
        }
    }

    IEnumerator ResetAnimation()
    {
        yield return new WaitForSeconds(0.25f);
        anim.SetBool("isThrowing", false);
    }

}
