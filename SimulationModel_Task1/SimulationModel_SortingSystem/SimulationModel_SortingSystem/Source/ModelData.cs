using System;
using System.Collections.Generic;
using System.Text;

namespace AndeiYefimov.SortingSystem.Library
{
    public class ModelData
    {
        /* Define limits. */
        public const int MAX_LIST = 25;         /* Max number of lists. */
        public const int MAX_ATTR = 10;         /* Max number of attributes. */
        public const int MAX_SVAR = 25;         /* Max number of sampst variables. */
        public const int TIM_VAR = 25;          /* Max number of timest variables. */
        public const int MAX_TVAR = 50;         /* Max number of timest variables + lists. */
        public const float EPSILON = 0.001f;    /* Used in event_cancel. */

        public const int EVENT_ARRIVAL = 1;             // Тип события для поступления работы в систему
        public const int EVENT_DEPARTURE = 2;           // Тип события для ухода работы с определенного рабочего места. 
        public const int EVENT_END_SIMULATION = 3;      // Тип события для завершения моделирования
        public const int STREAM_INTERARRIVAL = 1;       // Поток случайных чисел для интервалов времени между поступлениями работ
        public const int STREAM_DESEASE_TYPE = 2;       // Поток случайных чисел для типов работ
        public const int STREAM_SERVICE = 3;            // Поток случайных чисел для времени обслуживания
        public const int OBJECT_TYPES = 3;
        public const int SAMPST_DELAYS = 1;				// переменная функции sampst для задержек в очереди(очередях). 
    }

    public enum SortableObjectType { FIRST_TYPE, SECOND_TYPE, THIRD_TYPE }

    public class SortableObject
    {
        public int _index;
        public SortableObjectType _objectType;
        public float _incomingTime;
        public float _departureTime;
        public int _sortingIterations;

        public SortableObject(int index, float incomingTime)
        {
            _index = index;
            _incomingTime = incomingTime;
            _departureTime = 0;
            _objectType = (SortableObjectType)new Random().Next(0, 3);
            _sortingIterations = 0;
        }
    }

    public class Worker
    {
        public float _probToSortCorrectly;

        public Worker(float probToSortCorrectly) => _probToSortCorrectly = probToSortCorrectly;
    }
    public class SystemEvent
    {
        public float _time;
        public int _eventType;

        public SystemEvent(float time, int eventType)
        {
            _time = time;
            _eventType = eventType;
        }
    };
}