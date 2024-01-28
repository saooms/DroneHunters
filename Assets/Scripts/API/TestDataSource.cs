using API;
using Logic;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace API
{
    public class TestDataSource : IDataSource
    {
        private Action<Coordinate> _callback;
        private List<Coordinate> _data;
        private int _currentStep = 0;

        public List<Coordinate> Data => _data;

        /// <param name="source">
        ///     Path to a JSON file with flight path data.
        /// </param>
        public TestDataSource(string source)
        {
            string text = File.ReadAllText(source);
            _data = JsonUtility.FromJson<CoordinateCollectionWrapper>(text).data;
        }

        public virtual void StartReceiveData(Action<Coordinate> callback)
        {
            _callback = callback;
            Step();
        }

        public void Step()
        {
            if (_callback == null)
                return;

            _callback(_data[_currentStep++ % _data.Count]);
        }

        public Coordinate PeekStep(int stepOffset = 0)
        {
            return _data[(_currentStep + stepOffset) % _data.Count];
        }

        public virtual void StopReceiveData()
        {

        }
    }

    [Serializable]
    public struct CoordinateCollectionWrapper
    {
        public List<Coordinate> data;
    }
}