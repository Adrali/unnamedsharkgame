using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Breaky : AThrowable
{
    //On initialise les variables de la classe abstraite en fonction de notre type
    private new void Awake()
    {
        base.Awake();

        speedMultiplier = 1.15f;
        damages = 2;
    }

    /// <summary>
    /// Lorsque qu'on touche quelque chose
    /// </summary>
    protected override void OnPoke()
    {
        gameObject.SetActive(false);
    }
}
