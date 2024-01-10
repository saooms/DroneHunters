using System;
using System.Collections.Generic;
using static System.Math;


namespace Logic
{
    public class HunterDrone : Drone
    {
        private readonly TargetDrone _target;
        private const double MaxSpeed = 13.8889; // 50km/h in m/s

        private double _previousTargetSpeed;
        private double _previousTargetHeading;
        private int _currentWaypoint = 0;
        private List<Coordinate> _wayPoints;

        public HunterDrone(Coordinate position,  TargetDrone target, string identifier = "Hunterdrone") : base(position, identifier, Type.hunter)
        {
            if (target == null)
                return;

            _target = target;

            target.OnUpdate += OnTargetUpdate;
        }

        private void OnTargetUpdate()
        {
            double targetSpeed = _target.Speed;
            double targetHeading = _target.Heading;
            if (Abs(targetHeading - _previousTargetHeading) > 0.01 ||
                Abs(targetSpeed - _previousTargetSpeed) > 0.1)
            {
                _wayPoints = GetWayPoints();
            }
            MoveToTarget();
            _previousTargetSpeed = targetSpeed;
            _previousTargetHeading = targetHeading;
        }

        public Coordinate PredictTargetLocation()
        {
            if (_target == null)
                return Position;

            // Calculate the time to target so we can compare it.
            double distanceFromTarget = Coordinate.CalculateDistanceBetween2Coordinates(Position, CalculateLocationBehindPredictedLocation(_target.Position));
            double timeToTarget =  CalculateTimeToTarget(distanceFromTarget);
            double speed = _target.Speed;
            double heading = _target.Heading * PI / 180; // convert to radians

            double previousTime = 0;
            int iteration = 0;

            Coordinate predictedLocation = _target.Position;

            while (Abs(timeToTarget - previousTime) > 0.1 && iteration < 20 && speed > 0)
            {
                double distance = speed * timeToTarget;
                double deltaZ = distance * Sin(heading) / (6371e3 * Cos(_target.Position.WorldPosition.x * PI / 180));
                double deltaX = distance * Cos(heading) / 6371e3;

                // convert to degrees
                deltaZ *= 180 / PI;
                deltaX *= 180 / PI;

                double newZ = _target.Position.WorldPosition.z + deltaZ;
                double newX = _target.Position.WorldPosition.x + deltaX;

                predictedLocation = new Coordinate(new CoordinateVector(newX, _target.Position.WorldPosition.y, newZ));

                previousTime = timeToTarget;
                timeToTarget = CalculateTimeToTarget(Coordinate.CalculateDistanceBetween2Coordinates(Position, CalculateLocationBehindPredictedLocation(predictedLocation)));
                iteration++;
            }

            return predictedLocation;
        }

        public Coordinate CalculateLocationBehindPredictedLocation(Coordinate predictedLocation)
        {
            //Coordinate predictedLocation = PredictTargetLocation();

            double targetHeading = _target.Heading * PI / 180; // convert to radians

            double behindZ = 50 * Sin(targetHeading) /
                             (6371e3 * Cos(predictedLocation.WorldPosition.x * PI / 180));
            double behindX = 50 * Cos(targetHeading) / 6371e3;
            
            // convert to degrees
            behindZ *= 180 / PI;
            behindX *= 180 / PI;

            double newZ = predictedLocation.WorldPosition.z - behindZ;
            double newX = predictedLocation.WorldPosition.x - behindX;

            Coordinate locationBehindTarget =
                new Coordinate(new CoordinateVector(newX, predictedLocation.WorldPosition.y, newZ));
            
            return locationBehindTarget;
        }

        public double CalculateTimeToTarget(double distance)
        {
            double time = distance / MaxSpeed;
            return time;
        }

        public double DistanceToTarget()
        {
            return Coordinate.CalculateDistanceBetween2Coordinates(Position, _target.Position);
        }
        
        public void MoveToTarget()
        {
            double speed = MaxSpeed;
            if (Coordinate.CalculateDistanceBetween2Coordinates(Position, _target.Position) < 56)
            {
                _wayPoints = GetWayPoints();
                speed = Min(_target.Speed, MaxSpeed);
            }
            else if (Coordinate.CalculateDistanceBetween2Coordinates(Position, _wayPoints[_currentWaypoint]) < 2)
                _currentWaypoint = Min(_currentWaypoint + 1, _wayPoints.Count - 1);
            
            double heading = CalculateHeadingToCoordinate(_wayPoints[_currentWaypoint]) * PI / 180; // convert to radians
            double timeDifference = Abs((DateTime.Now - CurrentTime()).TotalSeconds);
            double distance = speed * timeDifference;
                
            double deltaZ = distance * Sin(heading) / (6371e3 * Cos(Position.WorldPosition.x * PI / 180));
            double deltaX = distance * Cos(heading) / 6371e3;
            
            // convert to degrees
            deltaZ *= 180 / PI;
            deltaX *= 180 / PI;

            double newZ = Position.WorldPosition.z + deltaZ;
            double newX = Position.WorldPosition.x + deltaX;
                
            AddToFlightPath(new Coordinate(new CoordinateVector(newX, Position.WorldPosition.y, newZ)));
        }

        /*
         * This method should check wheter the target drone is heading towards the hunter drone
         * TODO: Write the actual code 
         */
        private bool ShouldTakeAvoidingAction()
        {
            return false;
        }

        private List<Coordinate> GetWayPointsForAvoiding()
        {
            List <Coordinate> waypoints = new List<Coordinate>();
            return waypoints;
        }

        public List<Coordinate> GetWayPoints()
        {
            List<Coordinate> waypoints = new List<Coordinate>();
            
            // It is not yet implemented! but this is how it should work
            if (ShouldTakeAvoidingAction()) GetWayPointsForAvoiding();

            waypoints.Add(CalculateLocationBehindPredictedLocation(PredictTargetLocation()));
            _currentWaypoint = 0;
            return waypoints;
        }
    }
}