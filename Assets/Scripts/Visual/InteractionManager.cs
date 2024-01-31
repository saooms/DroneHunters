using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Linq;

namespace Visual
{
    public class InteractionManager : MonoBehaviour
    {
        [SerializeField] private GameObject defaultInteractable = null;

        private void Update()
        {
            if (Input.anyKey || Input.mouseScrollDelta.y != 0)
            {
                IInteractable interactableA = null;
                IInteractable interactableB = null;

                // 3D
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out RaycastHit hit))
                    hit.transform.TryGetComponent(out interactableA);

                // 2D
                PointerEventData eventData = new PointerEventData(EventSystem.current);
                eventData.position = Input.mousePosition;
                List<RaycastResult> results = new List<RaycastResult>();
                EventSystem.current.RaycastAll(eventData, results);

                if (results.Count > 0)
                    results.Any(e => e.gameObject.TryGetComponent(out interactableB));

                if(defaultInteractable != null)
                    defaultInteractable.TryGetComponent(out interactableB);

                interactableA?.OnInteraction();
                interactableB?.OnInteraction();
            }
        }
    }
}
