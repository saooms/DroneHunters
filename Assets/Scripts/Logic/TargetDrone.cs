using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using API;
using Visual;

namespace Logic
{
    public class TargetDrone : Drone
    {
        private IDataSource _source;

        public delegate void UpdateEvent();
        public UpdateEvent OnUpdate;

        ~TargetDrone()
        {
            Destroy();
        }

        public void Destroy()
        {
            _source?.StopReceiveData();
        }

        public TargetDrone(IDataSource receiver, Coordinate position, string identifier = "Targetdrone") : base(position, identifier, Type.target) 
        {
            Initialize(receiver);
        }

        public TargetDrone(IDataSource source, string identifier = "Targetdrone") : base(null, identifier, Type.target)
        {
            Initialize(source);
        }

        private void Initialize(IDataSource source)
        {
            SetDataSource(source);
        }

        public IDataSource SetDataSource(IDataSource source)
        {
            _source?.StopReceiveData();
            IDataSource tmp = _source;
            
            _source = source;
            source?.StartReceiveData(OnReceiveData);

            return tmp;
        }

        private void OnReceiveData(Coordinate coordinate) 
        {
            AddToFlightPath(coordinate);
            
            if (OnUpdate != null)
                OnUpdate();
        }
    }
}