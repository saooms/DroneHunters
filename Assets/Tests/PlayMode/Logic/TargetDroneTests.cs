using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using API;
using Logic;

namespace PlayModeTest
{
    public class TargetDroneTests
    {
        [UnityTest]
        public IEnumerator ReceiveData()
        {
            TestDataSource source = new TestDataSource("Data/test-data-moving-east.json");
            TargetDrone drone = new TargetDrone(source);

            yield return new WaitForSeconds(.5f);
            source.Step();

            Assert.AreEqual(source.PeekStep(-1), drone.Position);
        }
    }
}