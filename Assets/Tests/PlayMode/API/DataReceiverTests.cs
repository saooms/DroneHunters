using System.Collections;
using System.Collections.Generic;
using API;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace PlayModeTest
{
    public class DataReceiverTests
    {
        [UnityTest]
        public IEnumerator ReceiveDataOverTime()
        {
            DataReceiver source = new DataReceiver("Data/test-data-moving-east.json");
            int callbackCount = 0;

            source.StartReceiveData(data => {
                callbackCount++;
            });
            yield return new WaitForSeconds(1);

            Assert.AreEqual(3, callbackCount);

            source.StopReceiveData();
        }
    }
}