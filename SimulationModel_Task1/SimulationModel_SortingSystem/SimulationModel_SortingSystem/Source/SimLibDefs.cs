namespace SimulationModel_SortingSystem.Source
{
    public class SimLibDefs
    {
        /* This is simlibdefs.h. */

        /* Define limits. */

        public int MAX_LIST = 25;       /* Max number of lists. */
        public int MAX_ATTR = 10;       /* Max number of attributes. */
        public int MAX_SVAR = 25;       /* Max number of sampst variables. */
        public int TIM_VAR = 25;        /* Max number of timest variables. */
        public int MAX_TVAR = 50;       /* Max number of timest variables + lists. */
        public float EPSILON = 0.001f;  /* Used in event_cancel. */

        /* Define array sizes. */

        public int LIST_SIZE = 26;      /* MAX_LIST + 1. */
        public int ATTR_SIZE = 11;      /* MAX_ATTR + 1. */
        public int SVAR_SIZE = 26;      /* MAX_SVAR + 1. */
        public int TVAR_SIZE = 51;      /* MAX_TVAR + 1. */

        /* Define options for list_file and list_remove. */

        public int FIRST = 1;           /* Insert at (remove from) head of list. */
        public int LAST = 2;            /* Insert at (remove from) end of list. */
        public int INCREASING = 3;      /* Insert in increasing order. */
        public int DECREASING = 4;      /* Insert in decreasing order. */

        /* Define some other values. */

        public int LIST_EVENT = 25;     /* Event list number. */
        public float INFINITY = 1E+30f; /* Not infinity, but a very large number. */

        /* Pre-define attribute numbers of transfer for event list. */

        public int EVENT_TIME = 1;      /* Attribute 1 in event list is event time. */
        public int EVENT_TYPE = 2;      /* Attribute 2 in event list is event type. */
    }
}
