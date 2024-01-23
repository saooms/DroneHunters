using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Logic;

public class HunterDroneTests
{
    [Test]
    public void SomeTest()
    {
        HunterDrone drone = new HunterDrone(new Coordinate(Vector3.zero), null);

        Assert.AreEqual(drone.Position.WorldPosition, new Coordinate(new CoordinateVector(0,0,0)).WorldPosition);
    }
}
