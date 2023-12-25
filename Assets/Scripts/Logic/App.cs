using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using API;
using Visual;

namespace Logic
{
    public class App : MonoBehaviour
    {
        private List<TargetDrone> drones;

        /// <remarks>
        ///     This method removes the target drones safely from memory.
        /// </remarks>
        private void OnDestroy()
        {
            foreach (TargetDrone drone in drones)
            {
                drone.Destroy();
            }
        }

        void Start()
        {
            drones = new List<TargetDrone>()
            {
                new TargetDrone(new DataReceiver(@"assets\data\stub.json"))
            };
        }
    }
}