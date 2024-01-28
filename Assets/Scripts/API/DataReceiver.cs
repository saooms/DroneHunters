using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Logic;
using System.IO;

namespace API
{
    public class DataReceiver : TestDataSource
    {
        private Timer _stubTimer;

        public DataReceiver(string source) : base(source) { }

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
        public override void StartReceiveData(Action<Coordinate> callback)
        {
            StopReceiveData();
            base.StartReceiveData(callback);

            _stubTimer = new Timer((e) =>{
                Step();
            }, null, 0, 500);
        }

        public override void StopReceiveData()
        {
            _stubTimer?.Dispose();
        }
    }
}