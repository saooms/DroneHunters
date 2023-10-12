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

        void Update()
        {
            if (Input.anyKey || Input.mouseScrollDelta.y != 0)
            {
                var eventData = new PointerEventData(EventSystem.current);
                eventData.position = Input.mousePosition;
                var results = new List<RaycastResult>();
                EventSystem.current.RaycastAll(eventData, results);

                IInteractable interactable = null;

                if (results.Count > 0)
                    results.Any(e => e.gameObject.TryGetComponent(out interactable));

                else if(defaultInteractable ?? true)
                    defaultInteractable.TryGetComponent(out interactable);

                interactable?.OnInteraction();
            }
        }
    }
}
