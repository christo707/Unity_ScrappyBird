﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class TapController : MonoBehaviour
{

    public delegate void playerDelegate();
    public static event  playerDelegate onPlayerDied;
    public static event  playerDelegate onPlayerScored;

   public float tapForce = 300;
   public float tiltForce = 2;

   public Vector3 startpos;

   Rigidbody2D rigidbody;
   Quaternion downRotation;
   Quaternion forwardRotation;

   GameManager game;

   void Start(){
       rigidbody = GetComponent<Rigidbody2D>();
       downRotation = Quaternion.Euler(0,0,-50);
       forwardRotation = Quaternion.Euler(0,0,40);
       game = GameManager.Instance;
   }

   void Update(){
       if(game.GameOver){
           return;
       }
       if(Input.GetMouseButtonDown(0)){                 //Left Click
          transform.rotation = forwardRotation;
          rigidbody.velocity = Vector3.zero;
          rigidbody.AddForce(Vector2.up * tapForce, ForceMode2D.Force); 
       }
       transform.rotation = Quaternion.Lerp(transform.rotation, downRotation, tiltForce * Time.deltaTime);
   }

   void OnTriggerEnter2D(Collider2D col){
       if(col.gameObject.tag == "scoreZone"){
           onPlayerScored();  //EventSent to GameManager
       }
       if(col.gameObject.tag == "deadZone"){
           rigidbody.simulated = false;
           onPlayerDied();
       }
   }

    void OnEnable(){
        GameManager.onGameStarted += onGameStarted;
        GameManager.onGameOver += onGameOver; 
    }

    void OnDisable(){
        GameManager.onGameStarted -= onGameStarted;
        GameManager.onGameOver -= onGameOver; 
    }

    void onGameStarted(){
        rigidbody.velocity = Vector3.zero;
        rigidbody.simulated = true;
    }

    void onGameOver(){
        transform.localPosition = startpos;
        transform.rotation = Quaternion.identity;
    }

}
