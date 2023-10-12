using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using API;
using Visual;

namespace Logic
{
    public class TargetDrone : Drone
    {
        private DataReceiver receiver;

        ~TargetDrone()
        {
            Destroy();
        }

        public void Destroy()
        {
            receiver?.StopReceiveData();
        }

        public TargetDrone(DataReceiver receiver, Coordinate position, string identifier = "Targetdrone") : base(position, identifier) 
        {
            Initialize(receiver);
        }

        public TargetDrone(DataReceiver receiver, string identifier = "Targetdrone") : base(new Coordinate(), identifier)
        {
            Initialize(receiver);
        }

        private void Initialize(DataReceiver receiver)
        {
            this.receiver = receiver;
            receiver.StartReceiveData(OnReceiveData);

            Visual.Map.activeMap.PlaceDrone(this);
        }

        private void OnReceiveData(Coordinate coordinate) 
        {
            _position = coordinate;
        }
    }
}