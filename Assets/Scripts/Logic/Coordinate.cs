using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.Serialization;
using Visual;

namespace Logic
{
    [Serializable]
    public class Coordinate
    {
        [FormerlySerializedAs("_worldPosition")] [SerializeField] 
        private CoordinateVector worldPosition;

        public CoordinateVector WorldPosition => worldPosition;
        public CoordinateVector MapPosition => WorldPositionToMapPosition(worldPosition);
        public Vector3 GamePosition => MapPositionToGamePosition(MapPosition);

        public static Vector2 MapOffset = Vector2.zero;

        public enum Type { World, Map, Game }

        public Coordinate(CoordinateVector position, Type type = Type.World)
        {
            worldPosition = type switch
            {
                Type.World => position,
                Type.Map => MapPositionToWorldPosition(position),
                Type.Game => MapPositionToWorldPosition(GamePositionToMapPosition(position)),
                _ => worldPosition
            };
        }

        public static CoordinateVector MapPositionToWorldPosition(CoordinateVector mapPosition)
        {
            double n = (Math.PI - ((2.0 * Math.PI * mapPosition.z) / Math.Pow(2, Map.Zoom)));

            return new CoordinateVector(
                x: (180.0 / Math.PI * Math.Atan((Math.Exp(n) - Math.Exp(-n)) / 2)),
                y: mapPosition.y,
                z: ((mapPosition.x / Math.Pow(2, Map.Zoom) * 360.0) - 180.0)
            );
        }

        public static CoordinateVector WorldPositionToMapPosition(CoordinateVector worldPosition)
        {
            return new CoordinateVector(
                x: ((worldPosition.z + 180.0) / 360.0 * (1 << Map.Zoom)),
                y: worldPosition.y,
                z: ((1.0 - Math.Log(Math.Tan(worldPosition.x * Math.PI / 180.0) +
                1.0 / Math.Cos(worldPosition.x * Math.PI / 180.0)) / Math.PI) / 2.0 * (1 << Map.Zoom)
            ));
        }

        public static CoordinateVector GamePositionToMapPosition(CoordinateVector gamePosition)
        {
            return new CoordinateVector(
                x: Math.Clamp((gamePosition.x + MapOffset.x) / Map.PlaneScale, 0, Map.MapSize),
                y: gamePosition.y,
                z: Math.Clamp(Map.MapSize - (gamePosition.z + MapOffset.y) / Map.PlaneScale, 0, Map.MapSize)
            );
        }

        public static Vector3 MapPositionToGamePosition(CoordinateVector mapPosition)
        {
            return new Vector3(
                x: (float)((mapPosition.x * Map.PlaneScale) - MapOffset.x), 
                y: (float)mapPosition.y, 
                z: (float)(((Map.MapSize - mapPosition.z) * Map.PlaneScale) - MapOffset.y)
            );
        }

        /// https://www.movable-type.co.uk/scripts/latlong.html
        /// Doesnt take altitude in consideration.
        public static double CalculateDistanceBetween2Coordinates(Coordinate coordinate1, Coordinate coordinate2)
        {
            const double radius = 6371e3; // Radius of the Earth in meters
            double phi1 = coordinate1.WorldPosition.x * Math.PI / 180; // φ is latitude, λ is longitude (in radians)
            double phi2 = coordinate2.WorldPosition.x * Math.PI / 180;
            double deltaPhi = (coordinate2.WorldPosition.x - coordinate1.WorldPosition.x) * Math.PI / 180;
            double deltaLam = (coordinate2.WorldPosition.z - coordinate1.WorldPosition.z) * Math.PI / 180;

            double a = Math.Sin(deltaPhi / 2) * Math.Sin(deltaPhi / 2) + Math.Cos(phi1) * Math.Cos(phi2) * Math.Sin(deltaLam / 2) * Math.Sin(deltaLam / 2);
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            double distance = radius * c;

            return distance; // in meters
        }

        public override string ToString()
        {
            return $"{Math.Round(worldPosition.x, 13)}, {Math.Round(worldPosition.z, 13)}";
        }
    }
}