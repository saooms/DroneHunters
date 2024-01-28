using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Logic;
using API;

namespace EditModeTest
{
    public class HunterDroneTests
    {
        [Test]
        public void Position()
        {
            HunterDrone drone = new HunterDrone(new Coordinate(Vector3.zero), null);

            Assert.AreEqual(new Coordinate(Vector3.zero), drone.Position);
        }

        [Test]
        public void UnMovingLocationPrediction()
        {
            TestDataSource source = new TestDataSource("Data/test-data-moving-east.json");
            TargetDrone target = new TargetDrone(source);
            HunterDrone drone = new HunterDrone(new Coordinate(Vector3.zero), target);

            Assert.AreEqual(target.Position, drone.PredictTargetLocation());
        }

        [Test]
        public void LocationBehindPrediction()
        {
            TestDataSource source = new TestDataSource("Data/test-data-moving-east.json");
            TargetDrone target = new TargetDrone(source);
            HunterDrone drone = new HunterDrone(new Coordinate(Vector3.zero), target);
            Coordinate posA = drone.PredictTargetLocation();
            Coordinate posB = drone.CalculateLocationBehindPredictedLocation(posA);

            double dist = Coordinate.CalculateDistanceBetween2Coordinates(posA, posB);
            Assert.AreEqual(dist, 50);
        }
    }
}