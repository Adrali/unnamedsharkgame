using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMonster : MonoBehaviour
{
    public Transform bottomArea;
    public Collider2D frontCollider;
    public bool checkBottom = true;
    public LayerMask groundLayer;
    Rigidbody2D m_Rigidbody;
    float m_Speed = 1.0f;
    Vector2 sens =new Vector2(1.0f, 0);

    private bool IsInLayerMask(GameObject obj, LayerMask layerMask)
    {
        return ((layerMask.value & (1 << obj.layer)) > 0);
    }


    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        /*if (Physics2D.OverlapCollider(frontCollider, null, null))
        {
            gameObject.transform.Rotate(0, 0, 180, Space.World);
        }*/
        m_Rigidbody.velocity = sens * m_Speed;
        //Debug.Log("Vitesse : " + transform.forward * m_Speed);
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (IsInLayerMask(col.gameObject, groundLayer))
            sens *= -1;
            gameObject.transform.Rotate(0, 180, 0, Space.World);
    }
}
