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
        public static float _deviceWorkHoursAmount = 1.0f;                     // device work lenght during the day
        public static float _simulationDaysAmount = 1.0f;                      // simulation lenght
        public static float _timeToNextObject = 0.5f;
        public static float _serviceTime = 0.49f;


        public static float[] _probDistribObjectType = new float[_numObjectsTypes];
        public static float[] _buffer = new float[50];

        public const int _workers = 2;
        public const int _numObjectsTypes = 3;

        public static List<SortableObject> _objects = new List<SortableObject>();
        public static List<Worker> _workersList = new List<Worker>();

        public static List<SortableObject> _sortedObjects = new List<SortableObject>();
        public static List<List<SortableObject>> _onServise = new List<List<SortableObject>>();

        public static string _outputFilePath = "output.txt";

        #region Functions
        public static void Arrive(bool firstArrival)
        {
            Console.WriteLine("Arrive");
            _totalObjects++;
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
        }

        public static void Report()
        {
            Console.WriteLine("Report");
            Console.WriteLine("_arrives: " + _arrives);
            Console.WriteLine("_departures: " + _departures);

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
            Console.WriteLine("Hello world");

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
                        Arrive(true);
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

        public delegate void P();
        static void Main2()
        {
            P p = Console.WriteLine; // P объявлен как delegate void P();
            foreach (var i in new[] { 1, 2, 3, 4 })
            {
                p += () => Console.Write(i);
            }
            Console.WriteLine("----");
            p();
            Console.WriteLine("----");
            // ----
            //
            // 1234----
        }
    }
}
