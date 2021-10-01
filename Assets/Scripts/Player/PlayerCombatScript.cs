using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatScript : APlayer, IDamageable
{
    //Constantes
    private const int BootDamage = 5; //Dégâts infligés lorsqu'on aterrit sur un ennemi

    //Privates
    private Rigidbody2D playerRigidbody; //Le rigidbody de notre personnage, pour le faire bouger
    private PlayerMovementScript playerMovement; //Le script de mouvement pour forcer le personnage à bouger
    private int healthPoints = 1; //Le nombre de points de vie dispo pour notre perso
    private Collider2D baddieHit; //Pour stocker les ennemis touches par les bottes
    private Collider2D throwableHit; //Pour stocker les throwables qu'on touche
    private IDamageable baddieBody; //Pour stocker le component IDamageable du truc touche
    private IThrowable throwableBody; //Pour stocker le component IThrowable du truc touche

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
            baddieHit = Physics2D.OverlapArea(playerBackFeet.position, playerFrontFeet.position + Vector3.down * CheckingRadius, baddiesLayer);
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
        throwableHit = Physics2D.OverlapArea(playerBackFeet.position + (Vector3.left + Vector3.down) * CheckingRadius, playerFrontHead.position + (Vector3.up + Vector3.right) * CheckingRadius, throwableLayer);
        //Si on en trouve un, on le lance !
        if (throwableHit != null)
        {
            //Si l'objet a la bonne interface...
            throwableBody = throwableHit.gameObject.GetComponent<IThrowable>();
            if(throwableBody != null)
            {
                //... on peut le jeter
                if (playerRigidbody.velocity.y < 0) playerMovement.ForceJump();
                throwableBody.getThrown(playerRigidbody.position.x, playerMovement.getCurrentSpeed());
            }
            
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
    private void OnDeath()
    {
        //A FAIRE PTDR
    }
}
