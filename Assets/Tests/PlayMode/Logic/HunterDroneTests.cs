using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Logic;
using API;
using static System.Math;

namespace PlayModeTest
{
    public class HunterDroneTests
    {
        [UnityTest]
        public IEnumerator MovingLocationPrediction()
        {
            TestDataSource source = new TestDataSource("Data/test-data-moving-east.json");
            TargetDrone target = new TargetDrone(source);

            yield return new WaitForSeconds(0.6f);
            source.Step();
            HunterDrone drone = new HunterDrone(new Coordinate(Vector3.zero), target);
            Coordinate posA = drone.PredictTargetLocation();

            yield return new WaitForSeconds(0.6f);
            source.Step();
            drone = new HunterDrone(new Coordinate(Vector3.zero), target);
            Coordinate posB = drone.PredictTargetLocation();

            double DistA = Coordinate.CalculateDistanceBetween2Coordinates(posA, drone.Position);
            double DistB = Coordinate.CalculateDistanceBetween2Coordinates(posB, drone.Position);
            Assert.Less(DistA, DistB);
        }

        [UnityTest]
        public IEnumerator ApproachTargetTimePrediction()
        {
            TestDataSource source = new TestDataSource("Data/test-data-idle.json");
            TargetDrone target = new TargetDrone(source);
            HunterDrone drone = new HunterDrone(new Coordinate(Vector3.zero), target);
            double estimA = drone.CalculateTimeToTarget(drone.DistanceToTarget());

            yield return new WaitForSeconds(0.5f);
            source.Step();
            double estimB = drone.CalculateTimeToTarget(drone.DistanceToTarget());

            Assert.Less(Abs(estimA - 0.5 - estimB), 0.02); // 2 milli second error margin.
        }
    }
}