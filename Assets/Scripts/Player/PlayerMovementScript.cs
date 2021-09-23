using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementScript : MonoBehaviour
{
    //Constantes
    private const float speed = 400f; //Vitesse des mouvements du personnage
    private const float jumpForce = 15f; //"Puissance" du saut du personnage
    private const float groundCheckingRadius = .12f; //Rayon du cercle dans lequel on cherche des collisions avec le sol

    //Privates
    private float xInput; //Input A/D
    private bool jumpInput; //Input de saut
    private bool jumpToken; //Indique si le joueur est en capacite de sauter (a toucher le sol depuis son dernier saut)
    private bool isGrounded; //Indique si le joueur est au sol ou non
    private Collider2D[] groundColliders; //Les colliders que le joueur touche (ou non) avec ses pieds
    private Rigidbody2D playerRigidbody; //Le rigidbody de notre personnage, pour le faire bouger

    //Publics
    public Transform playerFeet; //Bas du sprite, pour check contact avec le sol
    public LayerMask groundLayer; //Le layer utilise par le sol, pour check les collisions avec les plateformes

    void Start()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
    }

    void Awake()
    {
        xInput = 0f;
        jumpInput = false;
        jumpToken = false;
        isGrounded = false;
    }

    void FixedUpdate()
    {
        //On check si on est au sol, on check si on le token de saut a la bonne valeur
        GroundChecker();
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
        if (jumpInput)
        {
            jumpInput = false;
            playerRigidbody.velocity = new Vector2(playerRigidbody.velocity.x, jumpForce);
        }
    }

    /// <summary>
    /// Recoit les inputs du script d'input
    /// </summary>
    /// <param name="xMovement">Le mouvement horizontal du joueur (Q ou D)</param>
    /// <param name="Jump">Instruction de saut</param>
    public void ReceiveInputs(float xMovement, bool Jump)
    {
        xInput = xMovement;
        if (Jump) jumpInput = true;
    }
}
