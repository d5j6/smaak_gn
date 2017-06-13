using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hover : MonoBehaviour
{
    [Header("Horizontal Rotation")]
    [Space(5)]
    [SerializeField]
    private float m_HorizontalRotationSpeed;
    private float m_HorizontalRotationTimer;

    [Space(10)]
    [Header("Vertical Movement")]
    [Space(5)]
    [SerializeField]
    private float m_VerticalMovementDistance;

    [SerializeField]
    private float m_VerticalMovementSpeed;
    private float m_VerticalMovementTimer;

    private Vector3 m_OriginalPosition;
    private Quaternion m_OriginalRotation;

    private void Awake()
    {
        //Create a copy of the original transform values.
        Vector3 pos = transform.position;
        Quaternion rot = transform.rotation;

        m_OriginalPosition = new Vector3(pos.x, pos.y, pos.z);
        m_OriginalRotation = new Quaternion(rot.x, rot.y, rot.z, rot.w);
    }

    private void Update()
    {
        float sin = Mathf.Sin(m_VerticalMovementTimer);

        transform.position = m_OriginalPosition + new Vector3(0.0f, sin * m_VerticalMovementDistance, 0.0f);

        transform.rotation = m_OriginalRotation * Quaternion.Euler(0.0f, m_HorizontalRotationTimer * 360.0f, 0.0f);

        //Increase timers
        m_VerticalMovementTimer += m_VerticalMovementSpeed * Time.deltaTime;
        if (m_VerticalMovementTimer > (Mathf.PI * 2)) { m_VerticalMovementTimer -= Mathf.PI * 2; }

        m_HorizontalRotationTimer += m_HorizontalRotationSpeed * Time.deltaTime;
        if (m_HorizontalRotationTimer > 1.0f) { m_HorizontalRotationTimer -= 1.0f; }
    }
}
