using System;
using System.Collections.Generic;
using UnityEngine;
using static System.Math;

namespace Logic
{
    public abstract class Drone
    {
        private readonly Dictionary<DateTime, Coordinate> _flightPath;
        private double _heading;

        public enum Type { hunter, target }

        public readonly string Identifier;
        public readonly Type type;

        public Coordinate Position => CurrentPosition();
        public double Speed => CalculateSpeed();
        public double Heading => CalculateHeading();


        private Coordinate CurrentPosition()
        {
            if (_flightPath.Count <= 0) return null;

            DateTime lastKey = CurrentTime();
            return lastKey == DateTime.MinValue ? null : _flightPath[lastKey];
        }
        
        protected Drone(Coordinate position, string identifier, Type type)
        {
            this.Identifier = identifier;
            this.type = type;

            _flightPath = new Dictionary<DateTime, Coordinate>();
            if (position != null)
                AddToFlightPath(position);
        }

        protected DateTime CurrentTime()
        {
            if (_flightPath.Count > 0)
            {
                List<DateTime> keys = new List<DateTime>(_flightPath.Keys);
                return keys[^1];
            }

            return DateTime.MinValue;
        }

        protected void AddToFlightPath(Coordinate coordinate)
        {
            DateTime now = DateTime.Now;

            if (_flightPath.Count == 0)
                _flightPath.Add(now, coordinate);
            else
            {
                DateTime previous = PreviousTime();
                if (Abs((previous - now).TotalSeconds) > 0.05)
                {
                    Coordinate lastCoordinate = _flightPath[previous];
                    if (!lastCoordinate.Equals(coordinate))
                        _flightPath.Add(now, coordinate);
                }
            }
        }

        private Coordinate PreviousPosition() 
        {
            if (_flightPath.Count > 0)
            {
                DateTime lastKey = PreviousTime();
                return _flightPath[lastKey];
            }
            return null;
        }

        private DateTime PreviousTime()
        {
            if (_flightPath.Count > 0)
            {
                List<DateTime> keys = new List<DateTime>(_flightPath.Keys);
                return _flightPath.Count > 1 ? keys[^2] : keys[^1];
            }
            return DateTime.MinValue;
        }

        protected double CalculateHeadingToCoordinate(Coordinate coord)
        {
            double phi1 = Position.WorldPosition.x * PI / 180; // φ is latitude, λ is longitude (in radians)
            double phi2 = coord.WorldPosition.x * PI / 180;
            double deltaLam = (coord.WorldPosition.z - Position.WorldPosition.z) * PI / 180;

            double y = Sin(deltaLam) * Cos(phi2);
            double x = Cos(phi1) * Sin(phi2) - Sin(phi1) * Cos(phi2) * Cos(deltaLam);
            double bearing = Atan2(y, x) * 180 / PI;

            return bearing; // in degrees
        }

        private double CalculateSpeed()
        {
            if (CurrentTime() == DateTime.MinValue || PreviousTime() == DateTime.MinValue) return 0;
            DateTime lastTime = PreviousTime();
            DateTime currentTime = CurrentTime();
            
            double elapsedTime = (currentTime - lastTime).TotalSeconds;
            
            Coordinate lastPosition = PreviousPosition();
            Coordinate currentPosition = Position;

            if (lastPosition == null) return 0;

            double distance = Coordinate.CalculateDistanceBetween2Coordinates(currentPosition, lastPosition);

            double speed = distance / elapsedTime;
            if (speed > 100) return 0; // 360 km/h is too fast for us
            
            return speed;
        }

        private double CalculateHeading()
        {
            if (Speed == 0) return _heading;
                
            Coordinate lastPosition = PreviousPosition();
            Coordinate currentPosition = Position;

            if (currentPosition == null || lastPosition == null) return 0;

            double deltaZ = currentPosition.WorldPosition.z - lastPosition.WorldPosition.z;
            double deltaX = currentPosition.WorldPosition.x - lastPosition.WorldPosition.x;

            _heading = Atan2(deltaZ, deltaX) * 180 / PI;

            return _heading;
        }
    }
}