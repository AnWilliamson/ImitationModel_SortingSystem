using System;
using System.Collections.Generic;
using System.Text;

namespace AndeiYefimov.SortingSystem.Library
{
    public class ModelData
    {
        public const int EVENT_ARRIVAL = 1;             // Тип события для поступления работы в систему
        public const int EVENT_DEPARTURE = 2;           // Тип события для ухода работы с определенного рабочего места. 
        public const int EVENT_END_SIMULATION = 3;      // Тип события для завершения моделирования
    }

    public class SortableObject
    {
        public int _index;
        public int _sortingIterations;
        public double _incomingTime;

        public SortableObject(int index, double incomingTime)
        {
            _index = index;
            _incomingTime = incomingTime;
            _sortingIterations = 0;
        }
    }

    public class Worker
    {
        public double _probToSortCorrectly;

        public Worker(double probToSortCorrectly) => _probToSortCorrectly = probToSortCorrectly;
    }
    public class SystemEvent
    {
        public double _time;
        public int _eventType;

        public SystemEvent(double time, int eventType)
        {
            _time = time;
            _eventType = eventType;
        }
    };
}