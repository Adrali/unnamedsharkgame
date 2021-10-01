using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bouncy : AThrowable
{
    //On initialise les variables de la classe abstraite en fonction de notre type
    private new void Awake()
    {
        base.Awake();

        speedMultiplier = 0.85f;
        damages = 1;
    }

    /// <summary>
    /// Lorsque qu'on touche quelque chose
    /// </summary>
    protected override void OnPoke()
    {
        direction = -direction;
    }
}
