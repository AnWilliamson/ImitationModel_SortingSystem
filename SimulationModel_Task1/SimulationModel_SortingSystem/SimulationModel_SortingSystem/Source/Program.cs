using System;
using System.Collections.Generic;

namespace AndeiYefimov.SortingSystem.Library
{
    public class Program
    {
        #region System variables
        public static int _eventQueueMaxAmount = 25;
        public static List<SystemEvent> _eventList = new List<SystemEvent>();

        public static float _simTime;
        public static int _skipObjectsAmount = 5;
        public static Random rand = new Random();
        #endregion

        public static int _sortableObjectType;
        public static int _totalObjects = 0;
        public static int _arrives = 0, _departures = 0;

        public static float _depart1;
        public static float _depart2;
        public static float _meanSimTime;
        public static float _buff;

        public static float _workerDefaultProbToSortCorrectly = 0.5f;
        public static int _workersAmount = 2;

        public static float _meanInterarrival = 10.0f;
        public static float _minutesInHour = 60.0f;
        public static float _deviceWorkHoursAmount = 0.05f;                     // device work lenght during the day
        public static float _simulationDaysAmount = 1.0f;                      // simulation lenght
        public static float _timeToNextObject = 0.2f;
        public static float _serviceTime = 0.19f;


        public static float[] _probDistribObjectType = new float[_numObjectsTypes];
        public static float[] _buffer = new float[50];

        public const int _workers = 2;
        public const int _numObjectsTypes = 3;

        public static List<Worker> _workersList = new List<Worker>();

        public static int _curObjectIndex = 0;
        public static List<SortableObject> _objects = new List<SortableObject>();
        public static List<SortableObject> _sortedObjects = new List<SortableObject>();
        public static List<SortableObject> _onServise = new List<SortableObject>();

        public static string _outputFilePath = "output.txt";

        #region Functions
        public static void Arrive()
        {
            Console.WriteLine("Arrive");
            bool objectInServise = false;
            foreach (var item in _objects)
            {
                if(item._incomingTime == _simTime)
                {
                    _curObjectIndex = item._index;
                    _onServise.Add(item);
                    Console.WriteLine("**** _onServise count: " + _onServise.Count);
                    _objects.Remove(item);
                    objectInServise = true;
                    break;
                }
            }

            if (!objectInServise)
            {
                _onServise.Add(new SortableObject(++_totalObjects, _simTime));
                Console.WriteLine("**** _onServise count: " + _onServise.Count);
                _curObjectIndex = _totalObjects;
            }
            AddSystemEvent(_simTime + _timeToNextObject, ModelData.EVENT_ARRIVAL);
            AddSystemEvent(_simTime + _serviceTime, ModelData.EVENT_DEPARTURE);
        }

        public static void Departure()
        {
            Console.WriteLine("Departure");
            Service();
        }

        public static void Service()
        {
            Console.WriteLine("Service");
            foreach (var item in _onServise)
            {
                if (item._index == _curObjectIndex)
                {
                    bool isSortedCorrectly = (rand.Next(0, 101) / 100.0f) <= _workersList[_onServise.IndexOf(item)]._probToSortCorrectly;
                    if (isSortedCorrectly)
                        _sortedObjects.Add(item);
                    else
                        _objects.Add(item);
                    _onServise.Remove(item);

                    item._incomingTime = _simTime + Math.Abs(_serviceTime - _timeToNextObject) + _skipObjectsAmount * _timeToNextObject;
                    break;
                }
            }
        }

        public static void Report()
        {
            Console.WriteLine("Report");
            Console.WriteLine("_arrives: " + _arrives);
            Console.WriteLine("_departures: " + _departures);
            Console.WriteLine("_totalObjects: " + _totalObjects);
        }

        public static void AddSystemEvent(float time, int eventType)
        {
            _eventList.Add(new SystemEvent(time, eventType));
            _eventList.Sort(CompareSystemEvents);
        }

        public static int CompareSystemEvents(SystemEvent event1, SystemEvent event2)
        {
            return event1._time < event2._time ? 0 : 1;
        }

        public static void CreateDefaultWorkersList(int workersAmount = 1)
        {
            for (int i = 0; i < workersAmount; i++)
            {
                _workersList.Add(new Worker(_workerDefaultProbToSortCorrectly));
            }
        }
        #endregion

        static void Main()
        {
            AddSystemEvent(0.0f, ModelData.EVENT_ARRIVAL);
            AddSystemEvent(_minutesInHour * _deviceWorkHoursAmount * _simulationDaysAmount, ModelData.EVENT_END_SIMULATION);
            CreateDefaultWorkersList(_workersAmount);
            int nextEventType;

            do
            {
                _eventList.Sort(CompareSystemEvents);
                nextEventType = _eventList[0]._eventType;
                _simTime = _eventList[0]._time;
                Console.WriteLine("Event type: " + nextEventType + " | time: " + _simTime);
                _eventList.RemoveAt(0);

                switch (nextEventType)
                {
                    case ModelData.EVENT_ARRIVAL:
                        _arrives++;
                        Arrive();
                        break;
                    case ModelData.EVENT_DEPARTURE:
                        _departures++;
                        Departure();
                        break;
                    case ModelData.EVENT_END_SIMULATION:
                        Console.WriteLine("Event end simulation - time passed: " + _simTime);
                        Report();
                        break;
                }
            } while (nextEventType != ModelData.EVENT_END_SIMULATION);
        }
    }
}
