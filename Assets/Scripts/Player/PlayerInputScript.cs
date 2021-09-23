using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputScript : MonoBehaviour
{
    PlayerMovementScript playerMover;
    float xMovement;
    bool jump;

    void Start()
    {
       playerMover = GetComponent<PlayerMovementScript>();
    }

    void Update()
    {
        //On recupère les inputs
        xMovement = Input.GetAxisRaw("Horizontal");
        jump = Input.GetButtonDown("Jump");

        //On les envoit au joueur
        playerMover.ReceiveInputs(xMovement, jump);
    }
}
