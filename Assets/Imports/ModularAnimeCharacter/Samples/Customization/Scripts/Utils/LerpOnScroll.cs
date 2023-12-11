using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Rukha93.ModularAnimeCharacter.Customization.Utils
{
    public class LerpOnScroll : MonoBehaviour
    {
        [SerializeField] private Transform m_Target;
        [SerializeField] private Transform m_PositionA;
        [SerializeField] private Transform m_PositionB;
        [Space]
        [SerializeField] private float m_Step = 0.15f;
        [SerializeField] private float m_LerpSpeed = 4;

        private float m_LerpAmount;
        private float m_LerpTarget;

        // Update is called once per frame
        void Update()
        {
            if (Input.mouseScrollDelta.y < 0)
                m_LerpTarget = Mathf.Max(m_LerpTarget - m_Step, 0);
            else if (Input.mouseScrollDelta.y > 0)
                m_LerpTarget = Mathf.Min(m_LerpTarget + m_Step, 1);

            m_LerpAmount = Mathf.Lerp(m_LerpAmount, m_LerpTarget, Time.deltaTime * m_LerpSpeed);
            m_Target.position = Vector3.Lerp(m_PositionA.position, m_PositionB.position, m_LerpAmount);
        }
    }
}