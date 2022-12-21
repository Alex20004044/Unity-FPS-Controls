using CorD.SparrowInterfaceField;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Sirenix.OdinInspector;

namespace HS
{
    [RequireComponent(typeof(CharacterController))]
    public class FPSMovement : MonoBehaviour
    {
        [SerializeField]
        float speed = 10;
        [SuffixLabel("Normalized")]
        [SerializeField]
        InterfaceField<IObservable<Vector2>> inputSource;
        [SerializeField]
        float gravitySpeed = -1f;

        Vector2 input;
        CharacterController characterController;

        private void Awake()
        {
            inputSource.i.Subscribe((x) => input = x);
            characterController = GetComponent<CharacterController>();
        }

        private void FixedUpdate()
        {
            Vector3 displacement = Vector3.zero;
            if (input != Vector2.zero)
            {
                Vector3 direction = (transform.forward * input.y + transform.right * input.x).normalized;
                displacement += direction * speed;
            }
            Vector3 gravityDisplacement = Vector3.up * gravitySpeed;
            displacement += gravityDisplacement;
            characterController.Move(displacement * Time.fixedDeltaTime);
        }
    }
}