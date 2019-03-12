using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace DungeonKIT
{
    [Serializable]
    public class DoubleInt
    {
        public int current;
        public int max;

        //Constructor for initialization in other scripts
        public DoubleInt(int currentInt, int maxInt)
        {
            current = currentInt;
            max = maxInt;
        }
    }
}