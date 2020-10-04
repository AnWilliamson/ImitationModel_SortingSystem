using System;
using System.Collections.Generic;
using System.IO;

namespace AndeiYefimov.SortingSystem.Library
{
    public class Program
    {
        public static int _sortableObjectType;
        public static int _totalObjects;
        public static int _arrives, _departures;

        public static float _depart1;
        public static float _depart2;
        public static float _meanSimTime;
        public static float _buff;

        public static float _meanInterarrival = 10.0f;
        public static float _lengthDeviceWorks = 24.0f;
        public static float _lengthSimulation = 7.0f;
        public static float[] _probDistribObjectType = new float[_numObjectsTypes];
        public static float[] _buffer = new float[50];

        public const int _workers = 2;
        public const int _numObjectsTypes = 3;


        public static List<SortableObject> _objects = new List<SortableObject>();
        public static List<Worker> _workersList = new List<Worker>();

        public static List<SortableObject> _sortedObjects = new List<SortableObject>();     
        public static List<List<SortableObject>> _onServise = new List<List<SortableObject>>();
        public static List<NextEvent> _eventList = new List<NextEvent>();

        public static string _outputFilePath = "output.txt";

        public static float _minutesInHour = 60.0f;

        #region Functions

        #endregion


        static void Main()
        {
            Console.WriteLine("Hello world");
            float probToCorrectlySortObject = 0.5f;
            float serviceTime = 0.4f;
            float departureTime = 0.39f;
            int arrives = 0;
            int departures = 0;

            StreamWriter writer = new StreamWriter(_outputFilePath);
            // adding workers
            for (int i = 0; i < _workers; i++)
            {
                _workersList.Add(new Worker(probToCorrectlySortObject));
            }

            _totalObjects = 0;
            _depart1 = 0;
            _depart2 = 0;
            _meanSimTime = 0;
            _buff = 0;

            // Записываем заголовок отчета и входные параметры.
            writer.WriteLine("\n\n*****************************************************");
            writer.WriteLine("\nSorting system");
            writer.WriteLine("\nWorkers number " + _workers);
            writer.WriteLine("\nProbability correctly sort object: " + probToCorrectlySortObject);
            writer.WriteLine("\nAvarage departure time" + departureTime);
            writer.WriteLine("\nAvarage service time" + serviceTime);
            writer.WriteLine("\nSimulation time " + _lengthSimulation + " " + _lengthDeviceWorks + "-hours working days\n\n");
            // конец заголовка

            // ***************
            // запуск имитации
            // ***************

            for (int i = 0; i < _workers; i++)
            {
                _onServise.Add(new List<SortableObject>());
            }
            //NextEvent firstEvent = new NextEvent(0.0f, ModelData.EVENT_ARRIVAL);
            //NextEvent lastEvent = new NextEvent(_minutesInHour * _lengthDeviceWorks * _lengthSimulation, ModelData.EVENT_ARRIVAL);
            _eventList.Add(new NextEvent(0.0f, ModelData.EVENT_ARRIVAL));
            _eventList.Add(new NextEvent(_minutesInHour * _lengthDeviceWorks * _lengthSimulation, ModelData.EVENT_ARRIVAL));

            float simTime = 0;
            int nextEventType = 0;
            do
            {
                nextEventType = _eventList[0]._eventType;
                simTime = _eventList[0]._time;
                _eventList.RemoveAt(0);

                bool flag;

                switch (nextEventType)
                {
                    case ModelData.EVENT_ARRIVAL:
                        flag = false;
                        for (int k = 0; k < nextDay_doctor_queue.size(); k++)
                            if (nextDay_doctor_queue[k].incomingTime == sim_time)
                            {
                                flag = true;
                                patient_ = nextDay_doctor_queue[k];
                                k = nextDay_doctor_queue.size();
                            }
                        if (flag)
                            arrive(1);
                        else
                        {
                            arrivses++;
                            arrive(0);
                        }
                        break;
                    case ModelData.EVENT_DEPARTURE:
                        departures++;
                        depart();
                        break;
                    case ModelData.EVENT_END_SIMULATION:
                        end = clock();
                        time_spent = (double)(end - begin) / CLOCKS_PER_SEC;
                        printf("Simulation time: %g\n", time_spent);
                        printf("############\n");
                        report();
                        break;
                }
            } while (nextEventType != ModelData.EVENT_END_SIMULATION);

            _onServise.Clear();
            _workersList.Clear();
            writer.Close();

        }
    }
}
