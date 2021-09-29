using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatScript : APlayer, IDamageable
{
    //Constantes
    private const int bootDamage = 5; //D�g�ts inflig�s lorsqu'on aterrit sur un ennemi
    private const float bootReach = 1.6f; //La "port�e" de nos bottes pour toucher les ennemis directement en dessous de nous

    //Privates
    private Rigidbody2D playerRigidbody; //Le rigidbody de notre personnage, pour le faire bouger
    private int healthPoints = 1; //Le nombre de points de vie dispo pour notre perso
    private RaycastHit2D baddieHit; //Pour stocker les ennemis touches par un raycast
    private IDamageable baddieBody; //Pour stocker le component IDamageable du truc touche

    //Publique
    public LayerMask baddiesLayer; //Le layer utilis� pour les gens sur lesquels on peut taper

    //Premi�re m�thode lanc�
    private void Awake()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
    }

    //Fait pour les calculs physiques
    private void FixedUpdate()
    {
        BootOnHead();
    }

    /// <summary>
    /// Utilise pour infliger des d�g�ts aux ennemis lorsque le joueur atterit dessus
    /// </summary>
    private void BootOnHead()
    {
        //On doit �tre en l'air, mais surtout en chute libre
        if(!isGrounded && !isDashing && !isJumping)
        {
            //On (essaye de) r�cup�re(r) ce qu'on touche avec nos bottes
            baddieHit = Physics2D.Raycast(playerRigidbody.position, Vector2.down, bootReach, baddiesLayer);
            Debug.DrawLine(playerRigidbody.position, playerRigidbody.position + Vector2.down * bootReach, Color.yellow);
            if(baddieHit.collider != null)
            {
                //Si le truc r�cup�r� impl�mente l'interface IDamageable, on lui fait des d�g�ts
                baddieBody = baddieHit.collider.gameObject.GetComponent<IDamageable>();
                if (baddieBody != null) baddieBody.TakeDamage(bootDamage);
            }
        }
    }

    /// <summary>
    /// Comment infliger des d�g�ts � notre personnage
    /// </summary>
    /// <param name="damages">Quantit� de d�g�ts � infliger</param>
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
