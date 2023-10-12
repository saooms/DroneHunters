using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Visual;

namespace Logic
{
    public abstract class Drone
    {
        protected Coordinate _position;

        public readonly string identifier;
        public Coordinate position { get => _position; }

        protected Drone(Coordinate position, string identifier)
        {
            _position = position;
            this.identifier = identifier;
        }
    }
}