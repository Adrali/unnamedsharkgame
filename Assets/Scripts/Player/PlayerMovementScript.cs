using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementScript : MonoBehaviour
{
    //Constantes
    private const float Speed = 535f; //Vitesse des mouvements du personnage
    private const float DashForce = 1765f; //"Puissance" du dash du personnage
    private const float JumpForce = 575f; //"Puissance" du saut du personnage
    private const float GroundCheckingRadius = .12f; //Rayon du cercle dans lequel on cherche des collisions avec le sol
    private const float GravityOn = 6.6f, GravityOff = 0f; //La force de la gravite, en dehors d'un saut et durant un saut respectivement
    private const float InputLength = 0.1f; //Durée courte pour une action, standardisee
    private const float DashTotalCooldown = 0.66f; //Il faut attendre au moins ce temps la entre 2 dash


    //Privates
    private float xInput; //Input A/D
    private bool jumpInput; //Input de saut instantane (la frame ou le bouton est appuye)
    private bool dashInput; //Input de dash, lui aussi instantanne
    private bool jumpToken; //Indique si le joueur est en capacite de sauter (est au sol ou a toucher le sol depuis son dernier saut)
    private bool dashToken; //Indique si le joueur est en etat de dash (cooldown ecoule)
    private bool isGrounded; //Indique si le joueur est au sol ou non
    private bool isBonked; //Indique la tete du joueur est en collision avec le sol ou non
    private bool isDashing; //Indique si le joueur est en train de dasher ou non
    private bool dashForgiveness; //Indique si le joueur est dans l'algo d'acceptance ou non
    private int lastXInput; //Memorise le dernier input horizontal, si celui-ci est different de 0
    private float dashTimer, dashCooldown, dashForgivenessTimer; //Utilise pour chronometrer le dash en cours, faire revenir le dash et faire la tolerance du dash, respectivement
    private bool isJumping; //Indique si le joueur est actuellement en train de sauter ou non
    private bool jumpForgiveness; //Indique si le joueur est dans l'algo d'acceptance
    private float jumpTimer, jumpForgivenessTimer; //Pour decompter le temps passe en saut, et le temps d'acceptation
    private Collider2D[] groundColliders, bonkColliders; //Les colliders que le joueur touche (ou non) avec ses pieds, touche (ou non) avec sa tete, touche (ou non) avec son corps
    private Rigidbody2D playerRigidbody; //Le rigidbody de notre personnage, pour le faire bouger

    //Publics
    public Transform playerBackFeet, playerFrontFeet; //Bas du sprite, pour check contact avec le sol. On en a 2, un pour l'avant et l'autre pour l'arriere
    public Transform playerBackHead, playerFrontHead; //Comme au dessus mais dans l'autre sens. 2 pour les meme raisons qu'au dessus
    public LayerMask groundLayer; //Le layer utilise par le sol, pour check les collisions avec les plateformes

    //On awake avec nos initialisations pour eviter les erreurs
    void Awake()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
        playerRigidbody.gravityScale = GravityOn;

        xInput = 0f;
        jumpInput = false;
        jumpToken = false;
        dashToken = false;
        isGrounded = false;
        isBonked = false;
        isDashing = false;
        dashForgiveness = false;
        lastXInput = 1;
        dashTimer = 0f; dashCooldown = DashTotalCooldown; dashForgivenessTimer = 0f;
        isJumping = false;
        jumpForgiveness = false;
        jumpTimer = 0f; jumpForgivenessTimer = 0f;
    }

    //On recupere les infos physiques, puis on s'en sert pour faire des actions
    void FixedUpdate()
    {
        //On check les differentes variables utiles
        GroundChecker();
        BonkChecker();
        LastXGetter();
        DashTokenizer();
        JumpTokenizer();

        //On applique les inputs qui ont ete recus
        DashMover();
        GroundMover();
        JumpMover();
    }

    void GroundChecker()
    {
        groundColliders = Physics2D.OverlapCircleAll(playerBackFeet.position, GroundCheckingRadius, groundLayer);
        if (groundColliders.Length == 0)
        {
            groundColliders = Physics2D.OverlapCircleAll(playerFrontFeet.position, GroundCheckingRadius, groundLayer);
            if (groundColliders.Length == 0) isGrounded = false;
            else isGrounded = true;
        } 
        else isGrounded = true;
    }

    void BonkChecker()
    {
        bonkColliders = Physics2D.OverlapCircleAll(playerBackHead.position, GroundCheckingRadius, groundLayer);
        if (bonkColliders.Length == 0)
        {
            bonkColliders = Physics2D.OverlapCircleAll(playerFrontHead.position, GroundCheckingRadius, groundLayer);
            if (bonkColliders.Length == 0) isBonked = false;
            else isBonked = true;
        }
        else isBonked = true;
    }

    void LastXGetter()
    {
        if (!isDashing) if ((int)xInput != 0) lastXInput = (int)xInput;
    }

    void DashTokenizer()
    {
        if (!dashToken && !isDashing)
        {
            if (dashCooldown > DashTotalCooldown) dashToken = true;
            else dashCooldown += Time.fixedDeltaTime;
        }
    }

    void JumpTokenizer()
    {
        if (!jumpToken && !isJumping && isGrounded) jumpToken = true;
    }

    void DashMover()
    {
        if (dashForgiveness)
        {
            dashForgivenessTimer += Time.fixedDeltaTime;
            if (dashToken && !isDashing)
            {
                DashModeOn();
                dashForgiveness = false;
            }
            else if (dashForgivenessTimer > InputLength) dashForgiveness = false;
        }

        if (dashInput)
        {
            dashInput = false;
            if (dashToken && !isDashing) DashModeOn();
            else
            {
                dashForgiveness = true;
                dashForgivenessTimer = 0f;
            }
        }

        if (isDashing)
        {
            if (dashTimer > InputLength) DashModeOff();
            else
            {
                playerRigidbody.velocity = new Vector2(lastXInput * Time.fixedDeltaTime * DashForce, playerRigidbody.velocity.y);
                dashTimer += Time.fixedDeltaTime;
            }
        }
    }

    void DashModeOn()
    {
        if (isJumping) JumpModeOff();
        isDashing = true;
        playerRigidbody.gravityScale = GravityOff;
        playerRigidbody.velocity = Vector2.zero;
        dashTimer = 0f;
        dashToken = false;
    }

    void DashModeOff()
    {
        isDashing = false;
        playerRigidbody.gravityScale = GravityOn;
        dashCooldown = 0f;
    }

    void GroundMover()
    {
        if(!isDashing) playerRigidbody.velocity = new Vector2(xInput * Time.fixedDeltaTime * Speed, playerRigidbody.velocity.y);
    }

    void JumpMover()
    {
        if(jumpForgiveness)
        {
            jumpForgivenessTimer += Time.fixedDeltaTime;
            if (jumpToken && !isJumping && !isDashing)
            {
                JumpModeOn();
                jumpForgiveness = false;
            }
            else if (jumpForgivenessTimer > InputLength) jumpForgiveness = false;
        }

        if(jumpInput)
        {
            jumpInput = false;
            if(jumpToken && !isJumping && !isDashing) JumpModeOn();
            else
            {
                jumpForgiveness = true;
                jumpForgivenessTimer = 0f;
            }
        }

        if(isJumping)
        {
            if (isBonked) JumpModeOff();
            else if (jumpTimer > InputLength) JumpModeOff();
            else
            {
                playerRigidbody.velocity = new Vector2(playerRigidbody.velocity.x, JumpForce * Time.fixedDeltaTime);
                jumpTimer += Time.fixedDeltaTime;
            }
        }
    }

    void JumpModeOn()
    {
        playerRigidbody.gravityScale = GravityOff;
        playerRigidbody.velocity = Vector2.right * playerRigidbody.velocity.x;
        jumpTimer = 0f;
        isJumping = true;
        if(!isGrounded) jumpToken = false;
    }

    void JumpModeOff()
    {
        playerRigidbody.gravityScale = GravityOn;
        isJumping = false;
    }

    /// <summary>
    /// Recoit les inputs du script d'input
    /// </summary>
    /// <param name="xMovement">Le mouvement horizontal du joueur (Q ou D)</param>
    /// <param name="jump_i">Instruction de saut</param>
    /// <param name="dash_i">Instruction de dash</param>
    public void ReceiveInputs(float xMovement, bool jump_i, bool dash_i)
    {
        xInput = xMovement;
        if (jump_i) jumpInput = true;
        if (dash_i) dashInput = true;
    }
}
