using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Logic;
using System.IO;

namespace API
{
    public class DataReceiver // maybe use different name
    {
        private Timer stubTimer;
        private string stubSource;

        public DataReceiver(string source)
        {
            stubSource = source;
        }

        public void StartReceiveData(Action<Coordinate> callback)
        {
            StopReceiveData();

            List<Coordinate> data = DataStub();
            int index = 0;
            stubTimer = new Timer((e) =>{
                callback(data[index++ % data.Count]);
            }, null, 0, 500);
        }

        public void StopReceiveData()
        {
            stubTimer?.Dispose();
        }

        private List<Coordinate> DataStub()
        {
            string text = File.ReadAllText(stubSource);
            CoordinateCollectionWrapper wrapper = JsonUtility.FromJson<CoordinateCollectionWrapper>(text);
            return wrapper.data;
        }
    }

    [Serializable]
    public struct CoordinateCollectionWrapper
    {
        public List<Coordinate> data;
    }
}