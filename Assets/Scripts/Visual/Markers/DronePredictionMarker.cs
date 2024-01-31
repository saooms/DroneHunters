using Logic;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Visual
{
    public class DronePredictionMarker : DroneMarker
    {
        protected override Coordinate Position => predictionSelection(Drone) ?? Drone.Position;
        public Func<Drone, Coordinate> predictionSelection;
    }
}