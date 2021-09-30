﻿using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 6f;
    Vector3 movement;
    Animator anim;
    Rigidbody playerRigidbody;
    int floorMask;
    float camRayLength = 100f;

    private void Awake()
    {
        //get nilai mask dari layer yang bernama Floor
        floorMask = LayerMask.GetMask("Floor");
        //get komponen Animator
        anim = GetComponent<Animator>();
        //get komponen Rigidbody
        playerRigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        //get value input horizontal (-1,0,1)
        float h = Input.GetAxisRaw("Horizontal");

        //get value input vertical (-1,0,1)
        float v = Input.GetAxisRaw("Vertical");

        Move(h, v);
        Turning();
        Animating(h, v);

    }

    //Method player dapat berjalan
    public void Move(float h, float v)
    {
        //Set nilai x dan y
        movement.Set(h, 0f, v);
        //Menormalisasi nilai vector agar total panjang dari vector adalah 1
        movement = movement.normalized * speed * Time.deltaTime;
        //Move to position
        playerRigidbody.MovePosition(transform.position + movement);
    }

    void Turning()
    {
        //Ray dari posisi mouse di layar
        Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        //raycast untuk floorHit
        RaycastHit floorHit;

        //Lakukan raycast
        if (Physics.Raycast(camRay, out floorHit, camRayLength, floorMask))
        {
            //Mendapatkan vector dari posisi player dan posisi floorHit
            Vector3 playerToMouse = floorHit.point - transform.position;
            playerToMouse.y = 0f;

            //Mendapatkan look rotation baru ke hit position
            Quaternion newRotation = Quaternion.LookRotation(playerToMouse);

            //Rotasi player
            playerRigidbody.MoveRotation(newRotation);
        }
    }

    public void Animating(float h, float v)
    {
        bool walking = h != 0f || v != 0f;
        anim.SetBool("IsWalking", walking);
    }
}