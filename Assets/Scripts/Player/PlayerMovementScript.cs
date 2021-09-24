using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementScript : MonoBehaviour
{
    //Constantes
    private const float speed = 400f; //Vitesse des mouvements du personnage
    private const float jumpForce = 590f; //"Puissance" du saut du personnage
    private const float groundCheckingRadius = .12f; //Rayon du cercle dans lequel on cherche des collisions avec le sol
    private const float gravityOn = 6.6f, gravityOff = 0f; //La force de la gravite, en dehors d'un saut et durant un saut respectivement
    private const float shortInput = 0.09f, longInput = 0.27f; //Durée minimale d'un appui bouton, durée maximale d'un appui bouton


    //Privates
    private float xInput; //Input A/D
    private bool jumpInput_i, jumpInput_c; //Inputs de saut : instantane (la frame ou le bouton est appuye) et continu (tant que le bouton est enfonce)
    private bool jumpToken; //Indique si le joueur est en capacite de sauter (a toucher le sol depuis son dernier saut)
    private bool isGrounded; //Indique si le joueur est au sol ou non
    private bool isBonked; //Indique la tete du joueur est en collision avec le sol ou non
    private bool isJumping; //Indique si le joueur est actuellement en train de sauter ou non
    private bool jumpForgiveness; //Indique si le joueur est dans l'algo d'acceptance
    private float jumpTimer, forgivenessTimer; //Pour decompter le temps passe en saut, et le temps d'acceptation
    private Collider2D[] groundColliders, bonkColliders; //Les colliders que le joueur touche (ou non) avec ses pieds, et touche (ou non) avec sa tete
    private Rigidbody2D playerRigidbody; //Le rigidbody de notre personnage, pour le faire bouger

    //Publics
    public Transform playerFeet; //Bas du sprite, pour check contact avec le sol
    public Transform playerHead; //Comme au dessus mais dans l'autre sens
    public LayerMask groundLayer; //Le layer utilise par le sol, pour check les collisions avec les plateformes

    //On initialise juste le rigidbody
    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
        playerRigidbody.gravityScale = gravityOn;
    }

    //On awake avec nos initialisations pour eviter les erreurs
    void Awake()
    {
        xInput = 0f;
        jumpInput_i = false; jumpInput_c = false;
        jumpToken = false;
        isGrounded = false;
        isBonked = false;
        isJumping = false;
        jumpTimer = 0f; forgivenessTimer = 0f;
    }

    //On recupere les infos physiques, puis on s'en sert pour faire des actions
    void FixedUpdate()
    {
        //On check si on est au sol, on check si on le token de saut a la bonne valeur
        GroundChecker();
        BonkChecker();
        JumpTokenizer();

        //On applique les inputs qui ont ete recus
        GroundMover();
        JumpMover();
    }

    void GroundChecker()
    {
        groundColliders = Physics2D.OverlapCircleAll(playerFeet.position, groundCheckingRadius, groundLayer);
        if (groundColliders.Length == 0) isGrounded = false;
        else isGrounded = true;
    }

    void BonkChecker()
    {
        bonkColliders = Physics2D.OverlapCircleAll(playerHead.position, groundCheckingRadius, groundLayer);
        if (bonkColliders.Length == 0) isBonked = false;
        else isBonked = true;
    }

    void JumpTokenizer()
    {
        if (!jumpToken && isGrounded) jumpToken = true;
    }

    void GroundMover()
    {
        playerRigidbody.velocity = new Vector2(xInput * Time.fixedDeltaTime * speed, playerRigidbody.velocity.y);
    }

    void JumpMover()
    {
        if(jumpForgiveness)
        {
            forgivenessTimer += Time.fixedDeltaTime;
            if (jumpToken)
            {
                JumpModeOn();
                jumpForgiveness = false;
            }
            else if (forgivenessTimer > shortInput) jumpForgiveness = false;
        }

        if(jumpInput_i)
        {
            jumpInput_i = false;
            if (jumpToken) JumpModeOn();
            else
            {
                jumpForgiveness = true;
                forgivenessTimer = 0f;
            }
        }

        if(isJumping)
        {
            if (isBonked) JumpModeOff();
            else if (jumpTimer > longInput) JumpModeOff();
            else if (jumpTimer > shortInput && !jumpInput_c) JumpModeOff();
            else
            {
                playerRigidbody.velocity = new Vector2(playerRigidbody.velocity.x, jumpForce * Time.deltaTime);
                jumpTimer += Time.deltaTime;
            }
        }
    }

    void JumpModeOn()
    {
        playerRigidbody.gravityScale = gravityOff;
        playerRigidbody.velocity = Vector2.right * playerRigidbody.velocity.x;
        jumpTimer = 0f;
        isJumping = true;
    }

    void JumpModeOff()
    {
        playerRigidbody.gravityScale = gravityOn;
        isJumping = false;
    }

    /// <summary>
    /// Recoit les inputs du script d'input
    /// </summary>
    /// <param name="xMovement">Le mouvement horizontal du joueur (Q ou D)</param>
    /// <param name="Jump">Instruction de saut</param>
    public void ReceiveInputs(float xMovement, bool jump_i, bool jump_c)
    {
        xInput = xMovement;
        if (jump_i) jumpInput_i = true;
        jumpInput_c = jump_c;
    }
}
