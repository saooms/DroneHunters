using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using API;
using System;
using static System.Math;

namespace Logic
{
    public class UserDroneController : MonoBehaviour,  IDataSource
    {
        public static UserDroneController ActiveController;
        private Action<Coordinate> sendData;
        private Drone _selectedDrone;
        private float _speed = 9;
        private IDataSource _previousDataSource;

        private void Awake()
        {
            ActiveController = this;    
        }

        private void Update()
        {
            if (sendData == null)
                return;

            if (Round(Input.GetAxis("Horizontal")) == 0 && Round(Input.GetAxis("Vertical")) == 0)
            {
                sendData(_selectedDrone.Position);
                return;
            }

            double heading = Atan2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            double distance = _speed * Time.deltaTime;

            double deltaZ = distance * Sin(heading) / (6371e3 * Cos(_selectedDrone.Position.WorldPosition.x * PI / 180));
            double deltaX = distance * Cos(heading) / 6371e3;

            //// convert to degrees
            deltaZ *= 180 / PI;
            deltaX *= 180 / PI;

            double newZ = _selectedDrone.Position.WorldPosition.z + deltaZ;
            double newX = _selectedDrone.Position.WorldPosition.x + deltaX;

            Coordinate data = new Coordinate(new CoordinateVector
            (
                x: newX,
                y: _selectedDrone.Position.GamePosition.y,
                z: newZ
            ));
            sendData(data);
        }

        public void AllocateDrone(Drone drone)
        {
            if (ReferenceEquals(drone, _selectedDrone))
            {
                (drone as TargetDrone).SetDataSource(_previousDataSource);
                StopReceiveData();
                return;
            }
            _selectedDrone = drone;
            _previousDataSource = (drone as TargetDrone).SetDataSource(this);
        }

        public void StartReceiveData(Action<Coordinate> callback)
        {
            sendData = callback;
        }

        public void StopReceiveData()
        {
            sendData = null;
            _selectedDrone = null;
        }

    }
}