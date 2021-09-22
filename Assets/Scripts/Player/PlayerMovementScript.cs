using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementScript : MonoBehaviour
{
    float xInput;
    bool jumpInput;
    float speed = 5f;
    public Transform playerFeet;
    public LayerMask groundLayer;
    float groundCheckerRadius = 0.12f;
    int groundCollisions;
    bool jumpToken = false;
    Rigidbody2D rigidbody;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        
    }

    void GroundChecker()
    {
        Physics2D.OverlapCircle(playerFeet.position, groundCheckerRadius, groundLayer);
    }

    void PlayerMover()
    {
        rigidbody.MovePosition(rigidbody.position + Vector2.right * xInput * Time.fixedDeltaTime * speed);
        if (jumpInput)
        {
            jumpInput = false;
            if(jumpToken)
            {
                rigidbody.velocity = rigidbody.velocity + Vector2.up * speed;
            }
        }
    }

    /// <summary>
    /// Reçoit les inputs du script d'input
    /// </summary>
    /// <param name="xMovement">Le mouvement horizontal du joueur (Q ou D)</param>
    /// <param name="Jump">Instruction de saut</param>
    void ReceiveInputs(float xMovement, bool Jump)
    {
        xInput = xMovement;
        if (Jump) jumpInput = true;
    }
}
