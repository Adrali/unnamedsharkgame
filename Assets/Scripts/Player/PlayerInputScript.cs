using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputScript : MonoBehaviour
{
    //Privates
    private PlayerMovementScript playerMover;
    private float xMovement;
    private bool jump_i;
    private bool dash_i;

    //Initialisation : on a juste besoin de recuperer un script
    void Start()
    {
       playerMover = GetComponent<PlayerMovementScript>();
    }

    //On awake sans valeur dans les inputs, pour eviter toute erreur
    private void Awake()
    {
        xMovement = 0f;
        jump_i = false;
    }

    //On passe les updates a recuperer les inputs, d'ou l'interet de scripts separes :
    //Les updates ne seront que ici, et les FixedUpdates dans les autres scripts joueurs
    void Update()
    {
        //On recupère les inputs
        xMovement = Input.GetAxisRaw("Horizontal");
        jump_i = Input.GetButtonDown("Jump");
        dash_i = Input.GetButtonDown("Dash");

        //On les envoit au joueur
        playerMover.ReceiveInputs(xMovement, jump_i, dash_i);
    }
}
