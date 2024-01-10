using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Logic;
using System.IO;

namespace API
{
    public class DataReceiver : IDataSource
    {
        private Timer _stubTimer;
        private readonly string _stubSource;

        /// <param name="source">
        ///     Path to a JSON file with flight path data.
        /// </param>
        public DataReceiver(string source)
        {
            _stubSource = source;
        }

        /// <summary>
        ///     Starts the process of receiving data.
        /// </summary>
        /// <remarks>
        ///     This process is a stub to simulate the real world implementation.
        /// </remarks>
        ///
        /// <param name="callback">
        ///     A method that will get called when data is received.
        /// </param>
        public void StartReceiveData(Action<Coordinate> callback)
        {
            StopReceiveData();

            List<Coordinate> data = DataStub();
            int index = 0;
            _stubTimer = new Timer((e) =>{
                callback(data[index++ % data.Count]);
            }, null, 0, 500);
        }

        public void StopReceiveData()
        {
            _stubTimer?.Dispose();
        }

        private List<Coordinate> DataStub()
        {
            string text = File.ReadAllText(_stubSource);
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