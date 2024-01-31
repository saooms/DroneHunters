using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Logic;
using API;

namespace EditModeTest
{
    public class TargetDroneTests
    {
        [Test]
        public void Position()
        {
            TargetDrone drone = new TargetDrone(null, new Coordinate(Vector3.zero));

            Assert.AreEqual(new Coordinate(Vector3.zero), drone.Position);
        }

        [Test]
        public void UpdateEvent()
        {
            TestDataSource source = new TestDataSource("Data/test-data-idle.json");
            TargetDrone drone = new TargetDrone(source);
            
            bool activated = false;
            drone.OnUpdate += () => { activated = true; };
            
            source.Step();

            Assert.AreEqual(true, activated);
        }
    }
}