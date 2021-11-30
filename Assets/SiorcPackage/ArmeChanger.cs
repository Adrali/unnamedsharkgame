using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.U2D.Animation;
using System.Linq;
using UnityEditor.Animations;
using UnityEditor.Presets;
using UnityEngine.U2D.Animation;


public class ArmeChanger : MonoBehaviour
{
    [SerializeField]
    private SpriteLibrary spriteLibrary = default;

    [SerializeField]
    private SpriteResolver targetResolver = default;

    [SerializeField]
    private string targetCategory = "Arme";

    [SerializeField]
    public int Arme = 1;

    [SerializeField]
    private Animator SiorcAnim  ;

    [SerializeField]
    bool Attack = false;


    [SerializeField]
    bool Advance = false;


    [SerializeField]
    bool Saut = false;

    /* [SerializeField]
     Preset preset49 = default;

     [SerializeField]
     Preset preset12 = default;

     [SerializeField]
     SpriteSkin spriteskin49 = default;

     [SerializeField]
     SpriteSkin spriteskin12 = default;*/

    bool currentlyAttack = false;

    private SpriteLibraryAsset LibraryAsset => spriteLibrary.spriteLibraryAsset;

    GameObject ArmeObject;

    bool change = false;
    int lastWeapon = -1;


    void Update()
    {
        string[] labels = LibraryAsset.GetCategoryLabelNames(targetCategory).ToArray();
        string label = labels[Arme];

        targetResolver.SetCategoryAndLabel(targetCategory, label);

        ArmeObject = GameObject.Find("/Siorc/Arme");



        if (Input.GetKeyDown("space"))
        {
            Attack = true;
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Arme++;
            if (Arme == 3)
                Arme = 0;
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Advance = !Advance;
        }
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Saut = true;
        }
        if (Input.GetKeyUp(KeyCode.UpArrow))
        {
            Saut = false;
        }



        if (lastWeapon != Arme)
        {
            lastWeapon = Arme;
            change = true;
        }

        if (change)
        {
            if (Arme == 0)
            {
                Vector3 deplacement = new Vector3( 0.5f, 0.5f, 0);
                ArmeObject.transform.Translate(deplacement);
                SiorcAnim.SetInteger("ArmeAnim", 0);
            }
            else if (Arme == 1)
            {
                Vector3 deplacement = new Vector3(-0.1f, -0.4f, 0);
                ArmeObject.transform.Translate(deplacement);
                SiorcAnim.SetInteger("ArmeAnim", 1);
               // preset49.ApplyTo(spriteskin49);

            }
            else if (Arme == 2)
            {
                Vector3 deplacement = new Vector3(1f, -0.05f, 0);
                ArmeObject.transform.Translate(deplacement);
                SiorcAnim.SetInteger("ArmeAnim", 2);
               // preset12.ApplyTo(spriteskin12);
            }
            change = false;
        }

        if (currentlyAttack)
        {
            currentlyAttack = false;
            SiorcAnim.SetBool("Attack", false);
        }
        if (Attack)
        {
            SiorcAnim.SetBool("Attack", true);
            currentlyAttack = true;
            //SiorcAnim.SetBool("Attack", false);
            Attack = false;
        }
        if (Advance)
        {
            SiorcAnim.SetBool("Advance", true);
        }
        else
        {
            SiorcAnim.SetBool("Advance", false);
        }
        if (Saut)
        {
            SiorcAnim.SetBool("Saut", true);
        }
        else 
        {
            SiorcAnim.SetBool("Saut", false);
        }
    }
}
