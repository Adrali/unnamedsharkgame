using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatScript : APlayer, IDamageable
{
    //Constantes
    private const int BootDamage = 5; //Dégâts infligés lorsqu'on aterrit sur un ennemi
    private const float BootReach = 0.1f; //La "portée" de nos bottes pour toucher les ennemis directement en dessous de nous
    private const float ThrowableReach = 0.6f; //La portée pour jeter un élément de l'environnement

    //Privates
    private Rigidbody2D playerRigidbody; //Le rigidbody de notre personnage, pour le faire bouger
    private PlayerMovementScript playerMovement; //Le script de mouvement pour forcer le personnage à bouger
    private int healthPoints = 1; //Le nombre de points de vie dispo pour notre perso
    private Collider2D baddieHit; //Pour stocker les ennemis touches par les bottes
    private Collider2D environmentHit; //Pour stocker les throwables qu'on touche
    private IDamageable baddieBody; //Pour stocker le component IDamageable du truc touche

    //Publique
    public LayerMask baddiesLayer; //Le layer utilisé pour les gens sur lesquels on peut taper
    public LayerMask throwableLayer; //Le layer utilisé par les objets jetables dans l'environnement

    //Première méthode lancé
    private new void Awake()
    {
        base.Awake();

        playerRigidbody = GetComponent<Rigidbody2D>();
        playerMovement = GetComponent<PlayerMovementScript>();
    }

    //Fait pour les calculs physiques
    private void FixedUpdate()
    {
        BootOnHead();
        ThrowingStuff();
    }

    /// <summary>
    /// Utilise pour infliger des dégâts aux ennemis lorsque le joueur atterit dessus
    /// </summary>
    private void BootOnHead()
    {
        //On doit être en chute libre
        if(playerRigidbody.velocity.y < 0)
        {
            //On (essaye de) récupére(r) ce qu'on touche avec nos bottes
            baddieHit = Physics2D.OverlapArea(playerBackFeet.position, playerFrontFeet.position + Vector3.down * BootReach, baddiesLayer);
            if(baddieHit != null)
            {
                //Si le truc récupéré implémente l'interface IDamageable, on lui fait des dégâts
                baddieBody = baddieHit.gameObject.GetComponent<IDamageable>();
                if (baddieBody != null)
                {
                    baddieBody.TakeDamage(BootDamage);
                    playerMovement.ForceJump();
                }
            }
        }
    }

    /// <summary>
    /// Utilise pour detecter et lancer les elements de l'environnement
    /// </summary>
    private void ThrowingStuff()
    {
        //On cherche les jetables autour de nous
        environmentHit = Physics2D.OverlapArea(playerBackFeet.position + Vector3.left * ThrowableReach, playerFrontHead.position + Vector3.right * ThrowableReach, throwableLayer);
        //Si on en trouve un, on le lance !
        if (environmentHit != null)
        {
            environmentHit.gameObject.GetComponent<IThrowable>().getThrown(playerRigidbody.position.x, playerMovement.getCurrentSpeed());
            if (playerRigidbody.velocity.y < 0) playerMovement.ForceJump();
        }
    }

    /// <summary>
    /// Comment infliger des dégâts à notre personnage
    /// </summary>
    /// <param name="damages">Quantité de dégâts à infliger</param>
    public void TakeDamage(int damages)
    {
        healthPoints -= damages;
        if (healthPoints <= 0) OnDeath();
    }

    /// <summary>
    /// Quoi faire quand le personnage meurt
    /// </summary>
    void OnDeath()
    {
        //A FAIRE PTDR
    }
}
