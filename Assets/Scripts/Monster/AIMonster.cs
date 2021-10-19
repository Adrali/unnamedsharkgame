using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMonster : MonoBehaviour
{
    public Transform bottomArea;
    public GameObject frontColliderObject;
    public bool checkBottom = true;
    public LayerMask groundLayer;
    public LayerMask collisionLayer;
    public LayerMask playerLayers;



    Rigidbody2D m_Rigidbody;
    float m_Speed = 3.0f;
    Vector2 sens = new Vector2(1.0f, 0);
    bool isMoving = true;

    private bool IsInLayerMask(GameObject obj, LayerMask layerMask)
    {
        return ((layerMask.value & (1 << obj.layer)) > 0);
    }


    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody2D>();
    }

    private void flipMob()
    {
        sens *= -1;
        Vector3 localScale = frontColliderObject.transform.localPosition;
        localScale.x *= -1;
        frontColliderObject.transform.localPosition = localScale;
        localScale = bottomArea.localPosition;
        localScale.x *= -1;
        bottomArea.localPosition = localScale;
       
    }
    // Update is called once per frame
    void Update()
    {
        if (!Physics2D.OverlapCircle(bottomArea.position, 0.5f,groundLayer))
        {
            flipMob();
        }
        if (isMoving)
        {
            m_Rigidbody.velocity = sens * m_Speed;
        }
        //Debug.Log("Vitesse : " + transform.forward * m_Speed);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (IsInLayerMask(col.gameObject, collisionLayer))
            flipMob();
        else if (IsInLayerMask(col.gameObject, playerLayers))
            isMoving = false;
            m_Rigidbody.velocity = new Vector2(0.0f,0.0f);

    }
    void OnTriggerExit2D(Collider2D col)
    {
        if (IsInLayerMask(col.gameObject, playerLayers))
            isMoving = true;

    }
}
