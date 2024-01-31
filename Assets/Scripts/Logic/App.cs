using System.Collections.Generic;
using UnityEngine;
using API;
using Visual;
using Map = Visual.Map;

namespace Logic
{
    public class App : MonoBehaviour
    {
        private List<TargetDrone> _drones;
        private HunterDrone _hunter;

        /// <remarks>
        ///     This method removes the target drones safely from memory.
        /// </remarks>
        private void OnDestroy()
        {
            foreach (TargetDrone drone in _drones)
            {
                drone.Destroy();
            }
        }

        void Start()
        {
            _drones = new List<TargetDrone>()
            {
                    new TargetDrone(new DataReceiver(@"Data\stub-data-demo.json"))
            };
            Map.ActiveMap.PlaceDroneMarker(_drones[0], MapMarker.MarkerType.Target);

            _hunter = new HunterDrone(new Coordinate(new Vector3(51.9171581691157f, 1, 4.483959781570087f)), _drones[0]);
            Map.ActiveMap.PlaceDroneMarker(_hunter, MapMarker.MarkerType.Hunter);
        }
    }
}