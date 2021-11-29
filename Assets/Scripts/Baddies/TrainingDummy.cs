using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainingDummy : MonoBehaviour, IDamageable
{
    //Privates
    private int healthPoints = 3;
    private GameObject theDummy;

    private void Awake()
    {
        theDummy = gameObject;
    }

    public void TakeDamage(int damages)
    {
        healthPoints -= damages;
        if (healthPoints <= 0) OnDeath();
    }

    void OnDeath()
    {
        Destroy(gameObject);
    }
}
