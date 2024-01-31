using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Logic;

namespace Visual
{
    public abstract class MapMarker : MonoBehaviour, IInteractable
    {
        public enum MarkerType { Target, Hunter, Prediction }

        protected abstract Coordinate Position { get; }

        private void Update()
        {
            if (Position != null && transform.position != Position.GamePosition)
                transform.position = Position.GamePosition;
        }

        public virtual void OnInteraction()
        {
            Debug.Log("waat");
        }
    }
}