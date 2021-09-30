using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AThrowable : MonoBehaviour, IThrowable
{
    //Constante
    private const float PokeLength = 0.6f;

    //Privates
    protected bool thrown; //Indique si on est actuellement en mouvement ou pas
    protected int direction; //Dans quel sens on va
    protected float throwSpeed; //A quelle vitesse on va
    protected float speedMultiplier; //Demultiplicateur de vitesse, dépend du type de throwable
    protected int damages; //Dégâts infligés par un poke
    protected float impulseSpeed; //Longueur du déplacement lors de l'impulse
    protected Rigidbody2D objectRigidbody; //Le rigidbody de l'objet, pour le déplacer
    protected RaycastHit2D pokedObject; //Pour stocker les objets dans lesquels on rebondi

    //Public
    public Transform upPoint, downPoint; //Le point en haut (au milieu) du throwable, pareil pour le bas
    public LayerMask pokableLayers; //Les masques sur lesquels on peut interagir

    //Initialisation
    protected void Awake()
    {
        thrown = false;
        direction = 0;
        throwSpeed = 0f;

        objectRigidbody = gameObject.GetComponent<Rigidbody2D>();
        objectRigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
    }

    private void FixedUpdate()
    {
        if (thrown) ThrownMover();
    }

    private void ThrownMover()
    {
        objectRigidbody.velocity = new Vector2(direction * throwSpeed * Time.fixedDeltaTime, objectRigidbody.velocity.y);

        CollisionChecker();
    }

    private void CollisionChecker()
    {

        pokedObject = Physics2D.Raycast(upPoint.position, upPoint.position + Vector3.right * direction * PokeLength, pokableLayers);
        if(pokedObject.collider != null)
        {
            if (pokedObject.collider.GetComponent<IDamageable>() != null) pokedObject.collider.GetComponent<IDamageable>().TakeDamage(damages);
            OnPoke();
        }
        else
        {
            pokedObject = Physics2D.Raycast(downPoint.position, downPoint.position + Vector3.right * direction * PokeLength, pokableLayers);
            if (pokedObject.collider != null)
            {
                if (pokedObject.collider.GetComponent<IDamageable>() != null) pokedObject.collider.GetComponent<IDamageable>().TakeDamage(damages);
                OnPoke();
            }
        }
    }

    public void getThrown(float xThrower, float throwerSpeed)
    {
        if(!thrown)
        {
            thrown = true;
            direction = Math.Sign(objectRigidbody.position.x - xThrower);
            throwSpeed = throwerSpeed * speedMultiplier;

            objectRigidbody.constraints = RigidbodyConstraints2D.None;
            objectRigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;

            objectRigidbody.velocity = new Vector2(direction * impulseSpeed, objectRigidbody.velocity.y);
            ThrownMover();
        }
    }

    protected abstract void OnPoke();
}
