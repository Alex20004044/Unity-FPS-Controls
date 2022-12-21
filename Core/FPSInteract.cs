using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HS
{
    public class FPSInteract : MonoBehaviour
    {
        [SerializeField]
        Camera playerCamera;
        [SerializeField]
        float interactDistance = 2f;
        [SerializeField]
        LayerMask layerMask;
        [SerializeField]
        bool isLogInteraction = false;

        private void Update()
        {
            if (!Input.GetMouseButtonDown(0))
                return;
            if (!Physics.Raycast(playerCamera.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, interactDistance, layerMask)) 
                return;

            if(hit.collider.TryGetComponent(out IInteractable interactable))
                if(isLogInteraction && interactable.TryInteract(gameObject))
                    Debug.Log($"{gameObject.name} interact with {hit.collider.name}");

        }
    }
}