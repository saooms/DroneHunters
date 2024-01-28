using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Logic;
using Visual;

namespace EditModeTest
{
    public class CoordinateTests
    {
        [Test]
        public void GameToMapPosition()
        {
            Map.Zoom = 1;
            Coordinate fromGamePosition = new Coordinate(Vector3.zero, Coordinate.Type.Game);

            Assert.AreEqual(new CoordinateVector(0, 0, 2), fromGamePosition.MapPosition);
        }

        [Test]
        public void MapToGamePosition()
        {
            Map.Zoom = 1;
            Coordinate fromMapPosition = new Coordinate(new Vector3(0, 0, 2), Coordinate.Type.Map);

            Assert.LessOrEqual((fromMapPosition.GamePosition - new Vector3(0, 0, 0)).magnitude, 0.0000001);
        }

        [Test]
        public void WordToMapPosition()
        {
            Map.Zoom = 1;
            Coordinate fromWorldPosition = new Coordinate(Vector3.zero, Coordinate.Type.World);

            Assert.AreEqual(new CoordinateVector(1, 0, 1), fromWorldPosition.MapPosition);
        }

        [Test]
        public void MapToWorldPosition()
        {
            Map.Zoom = 1;
            Coordinate fromWorldPosition = new Coordinate(new CoordinateVector(1, 0, 1), Coordinate.Type.Map);

            Assert.AreEqual(new CoordinateVector(0, 0, 0), fromWorldPosition.WorldPosition);
        }

        [Test]
        public void DistanceBetweenCoordinates()
        {
            Map.Zoom = 1;
            Coordinate fromWorldPosition = new Coordinate(new CoordinateVector(2, 0, 0), Coordinate.Type.World);
            double distance = Coordinate.CalculateDistanceBetween2Coordinates(fromWorldPosition, new Coordinate(new CoordinateVector(1, 0, 0)));

            Assert.LessOrEqual(distance, 111200.0);
        }

        [Test]
        public void NoDistanceBetweenCoordinates()
        {
            Map.Zoom = 1;
            Coordinate fromWorldPosition = new Coordinate(new CoordinateVector(0, 0, 0), Coordinate.Type.World);
            double distance = Coordinate.CalculateDistanceBetween2Coordinates(fromWorldPosition, new Coordinate(new CoordinateVector(0, 0, 0)));

            Assert.AreEqual(0.0, distance);
        }
    }
}