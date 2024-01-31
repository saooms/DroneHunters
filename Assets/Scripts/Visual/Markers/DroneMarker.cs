using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Logic;

namespace Visual
{
    public class DroneMarker : MapMarker
    {
        protected Drone Drone;
        protected override Coordinate Position => Drone.Position;

        public virtual void AllocateDrone(Drone drone)
        {
            this.Drone = drone;
        }

        public override void OnInteraction()
        {
            if (Input.GetMouseButtonDown(0))
                SideMenu.activeMenu.DisplayDroneData(Drone);
        }
    }
}
