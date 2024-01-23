using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Logic;
using Visual;

public class CoordinateTests
{
    [Test]
    public void GameToMapPosition()
    {
        Coordinate fromGamePosition = new Coordinate(Vector3.zero, Coordinate.Type.Game);

        Assert.AreEqual(fromGamePosition.MapPosition, new CoordinateVector(0, 0, 2));
    }

    [Test]
    public void MapToGamePosition()
    {
        Coordinate fromMapPosition = new Coordinate(new Vector3(0, 0, 2), Coordinate.Type.Map);

        Assert.AreEqual(fromMapPosition.GamePosition, new Vector3(0,0,0));
    }
}
