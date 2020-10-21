using System;
using System.Collections.Generic;
using System.IO;
using AndeiYefimov.SortingSystem.Library;

namespace AndeiYefimov.SortingSystem
{
    public class Program
    {
        #region System variables
        public static List<SystemEvent> _eventList = new List<SystemEvent>();

        public static double _simTime;
        public static int _skipObjectsAmount = 5;

        public static int _arrives = 0;
        public static int _departures = 0;
        public static int _totalObjects = 0;

        public static int _workersAmount = 2;
        public const double _workerDefaultProbToSortCorrectly = 0.5f;
        public static double _workerMaxProbToSortCorrectly = 1.0f;
        public static int _workerProbToSortCorrectlyIterations = 10;
        public static double _workerProbToSortCorrectlyStep = (_workerMaxProbToSortCorrectly - _workerDefaultProbToSortCorrectly) / _workerProbToSortCorrectlyIterations;
        public static List<Worker> _workersList = new List<Worker>();

        public static double _minutesInHour = 60.0;
        public static double _deviceWorkHoursAmount = 0.05f;     // device work lenght during the day (hours)
        public static double _simulationDaysAmount = 1.0f;     // simulation lenght (days)
        public static double _timeToNextObject = 0.1f;
        public static double _serviceTime = 0.19f;

        public static int _curObjectIndex = 0;
        public static List<SortableObject> _objects = new List<SortableObject>();
        public static List<SortableObject> _sortedObjects = new List<SortableObject>();
        public static List<SortableObject> _onService = new List<SortableObject>(2);

        public const string _outputFilePath = "output.txt";
        #endregion

        #region Functions
        public static void Arrive()
        {
            Console.WriteLine("\nArrive: " + _simTime);
            
            bool objectInServise = false;
            foreach (var item in _objects)
            {
                Console.WriteLine("item: " + item._incomingTime);
                Console.WriteLine(item._incomingTime.Equals(_simTime));

                if (item._incomingTime == _simTime)
                {
                    _curObjectIndex = item._index;

                    if(_onService[0] == null)
                        _onService[0] = item;
                    else _onService[1] = item;

                    Console.WriteLine("[a:0] _onServise[0]: " + (_onService[0] == null));
                    Console.WriteLine("[a:0] _onServise[1]: " + (_onService[1] == null));
                    _objects.Remove(item);
                    objectInServise = true;
                    break;
                }
            }

            if (!objectInServise)
            {
                SortableObject newObject = new SortableObject(++_totalObjects, _simTime);
                if (_onService[0] == null)
                    _onService[0] = newObject;
                else _onService[1] = newObject;

                Console.WriteLine("[a:1] _onServise[0]: " + (_onService[0] == null));
                Console.WriteLine("[a:1] _onServise[1]: " + (_onService[1] == null));

                _curObjectIndex = _totalObjects;
            }
            
            AddSystemEvent(_simTime + _timeToNextObject, ModelData.EVENT_ARRIVAL);
            AddSystemEvent(_simTime + _serviceTime, ModelData.EVENT_DEPARTURE);
        }

        public static void Departure()
        {
            Console.WriteLine("Departure: "+_simTime);
            Service();
        }

        public static void Service()
        {
            Console.WriteLine("Service");

            
            Console.WriteLine("[s:0] _onServise[0]: " + (_onService[0] == null));
            Console.WriteLine("[s:0] _onServise[1]: " + (_onService[1] == null));

            for (int i = 0; i < _workersAmount; i++)
            {
                if (_onService[i] != null && _onService[i]._index == _curObjectIndex)
                {
                    SortableObject obj = _onService[i];
                    obj._sortingIterations++;
                    Console.WriteLine("index: " + i);
                    bool isSortedCorrectly = GetRandom() <= _workersList[i]._probToSortCorrectly;
                    if (isSortedCorrectly)
                        _sortedObjects.Add(obj);
                    else
                        _objects.Add(obj);

                    obj._incomingTime = Math.Round(_simTime + (2 * _timeToNextObject - _serviceTime) + _skipObjectsAmount * _timeToNextObject, 2);
                    _onService[i] = null;
                    break;
                }
            }
            
        }

        public static void Report()
        {
            Console.WriteLine("\n *** Report ***");

            ConsoleReport();
            //FileReport();
        }

        public static void ConsoleReport()
        {
            Console.WriteLine("Workers count: " + _workersAmount);
            Console.WriteLine("Workers probability to sort correctly:");
            for (int i = 0; i < _workersAmount; i++)
                Console.WriteLine("\t" + (i + 1) + " - " + _workersList[i]._probToSortCorrectly);
            Console.WriteLine("Simulation time: " + _simTime + " min(s)");
            Console.WriteLine("_arrives: " + _arrives);
            Console.WriteLine("_departures: " + _departures);
            Console.WriteLine("_totalObjects: " + _totalObjects);
            Console.WriteLine("Correct sorting persantage: " + GetCorrectSortingPercentage() + "%");
            Console.WriteLine("\n******************************\n");
        }

        public static void FileReport(string filePath = _outputFilePath)
        {
            string text = "";
            text += "Workers count: " + _workersAmount + "\n";
            text += "Workers probability to sort correctly:\n";
            for (int i = 0; i < _workersAmount; i++)
                text += "\t" + (i + 1) + " - " + _workersList[i]._probToSortCorrectly + "\n";
            text += "Simulation time: " + _simTime + " min(s)\n";
            text += "_arrives: " + _arrives + "\n";
            text += "_departures: " + _departures + "\n";
            text += "_totalObjects: " + _totalObjects + "\n";
            text += "Correct sorting persantage: " + GetCorrectSortingPercentage() + "%\n";
            text += "\n******************************\n\n";


            // Create a file to write to.
            using (StreamWriter sw = File.AppendText(filePath))
            {
                sw.WriteLine(text);
            }
        }

        public static void AddSystemEvent(double time, int eventType)
        {
            _eventList.Add(new SystemEvent(Math.Round(time, 2), eventType));
            _eventList.Sort(CompareSystemEvents);
        }

        public static int CompareSystemEvents(SystemEvent event1, SystemEvent event2)
        {
            return event1._time < event2._time ? 0 : 1;
        }

        public static void CreateDefaultWorkersList(int workersAmount = 1, double probToSortCorrectly = _workerDefaultProbToSortCorrectly)
        {
            for (int i = 0; i < workersAmount; i++)
            {
                _workersList.Add(new Worker(probToSortCorrectly));
            }
        }

        public static double GetRandom()
        {
            double number = new Random().Next() % 100.0f / 100.0f;
            //Console.WriteLine("number: " + number);
            return number;
        }

        public static double GetCorrectSortingPercentage()
        {
            double i = 0;
            foreach (var item in _sortedObjects)
            {
                if (item._sortingIterations == 1)
                    i++;
            }
            Console.WriteLine();
            Console.WriteLine("* i: " + i);
            Console.WriteLine("* _sortedObjects: " + _sortedObjects.Count);
            Console.WriteLine("* _objects: " + _objects.Count);
            Console.WriteLine("* _onServise: " + _onService.Count);
            Console.WriteLine("\n-----------------------------\n");

            return Math.Round(i / (_sortedObjects.Count + _objects.Count), 4);
        }

        public static void ResetData()
        {
            _arrives = 0;
            _simTime = 0.0f;
            _departures = 0;
            _totalObjects = 0;
            _curObjectIndex = 0;

            _objects.Clear();
            _eventList.Clear();
            _sortedObjects.Clear();
        }

        public static void TestRand()
        {
            double sum = 0;
            double number = 0;
            for (int i = 0; i < 100; i++)
            {
                number = GetRandom();
                sum += number;
            }
            Console.WriteLine("** avg: " + (sum / 100.0f));
        }
        #endregion

        static void Main()
        {
            CreateDefaultWorkersList(_workersAmount);
            for (int i = 0; i < _workersAmount; i++)
                _onService.Add(null);
            Console.WriteLine("* _onServise[0]: " + (_onService[0] == null));
            Console.WriteLine("* _onServise[1]: " + (_onService[1] == null));

            for (int iteration = 0; iteration < _workerProbToSortCorrectlyIterations; iteration++)
            {
                for (int worker = 0; worker < _workersAmount; worker++)
                {
                    ResetData();

                    AddSystemEvent(0.0, ModelData.EVENT_ARRIVAL);
                    AddSystemEvent(_minutesInHour * _deviceWorkHoursAmount * _simulationDaysAmount, ModelData.EVENT_END_SIMULATION);
                    int nextEventType;

                    do
                    {
                        _eventList.Sort(CompareSystemEvents);
                        nextEventType = _eventList[0]._eventType;
                        _simTime = _eventList[0]._time;
                        //Console.WriteLine("Event type: " + nextEventType + " | time: " + _simTime);
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
                                Report();
                                break;
                        }
                    } while (nextEventType != ModelData.EVENT_END_SIMULATION);

                    _workersList[worker]._probToSortCorrectly += _workerProbToSortCorrectlyStep;
                }
            }
        }
    }
}
