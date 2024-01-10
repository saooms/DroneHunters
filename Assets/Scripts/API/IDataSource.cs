using Logic;
using System;

namespace API
{
    public interface IDataSource
    {
        public void StartReceiveData(Action<Coordinate> callback);
        public void StopReceiveData();
    }
}