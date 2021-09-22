using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputSCript : MonoBehaviour
{
    PlayerMovementScript m_PlayerMovement;
    float xMovement;
    bool jump;

    void Start()
    {
        m_PlayerMovement = GetComponent<PlayerMovementScript>();
    }

    void Update()
    {
        //On recupère les inputs
        xMovement = Input.GetAxisRaw("Horizontal");
        jump = Input.GetButtonDown("Jump");

        //On les envoit au joueur
    }
}
