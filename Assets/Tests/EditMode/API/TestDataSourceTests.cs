using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using API;
using Logic;

namespace EditModeTest
{
    public class TestDataSourceTests
    {
        [Test]
        public void LoadData()
        {
            TestDataSource source = new TestDataSource("Data/test-data-moving-east.json");

            Assert.Greater(source.Data.Count, 1);
        }

        [Test]
        public void SendData()
        {
            TestDataSource source = new TestDataSource("Data/test-data-moving-east.json");
            Coordinate receivedData = null;
            
            source.StartReceiveData(data => {
                receivedData = data;
            });

            Assert.AreEqual(source.PeekStep(-1), receivedData);
        }

        [Test]
        public void Step()
        {
            TestDataSource source = new TestDataSource("Data/test-data-moving-east.json");
            Coordinate receivedData = null;

            source.StartReceiveData(data => {
                receivedData = data;
            });

            source.Step();

            Assert.AreEqual(source.PeekStep(-1), receivedData);
        }

        [Test]
        public void Peek()
        {
            TestDataSource source = new TestDataSource("Data/test-data-moving-east.json");
            
            Coordinate expectedData = source.PeekStep();

            Coordinate receivedData = null;
            source.StartReceiveData(data => {
                receivedData = data;
            });

            Assert.AreEqual(expectedData, receivedData);
        }
    }
}