using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Logic;

namespace Visual
{
    public class DroneMarker : MonoBehaviour
    {
        private Drone drone;

        public void AllocateDrone(Drone drone)
        {
            this.drone = drone;
        }

        private void Update()
        {
            if (drone != null && transform.position != drone.position.gamePosition)
                UpdatePosition(drone?.position);
        }

        public void UpdatePosition(Coordinate coordinate)
        {
            transform.position = coordinate.gamePosition;
        }
    }
}