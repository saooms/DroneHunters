using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using API;
using Visual;

namespace Logic
{
    public class App : MonoBehaviour
    {
        [SerializeField] private Visual.Map map;

        private List<TargetDrone> drones;

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