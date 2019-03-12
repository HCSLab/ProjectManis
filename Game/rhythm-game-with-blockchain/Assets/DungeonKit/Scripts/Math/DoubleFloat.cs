using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace DungeonKIT
{
    [Serializable]
    public class DoubleFloat
    {
        public float current;
        public float max;

        //Constructor for initialization in other scripts
        public DoubleFloat(float currentFloat, float maxFloat)
        {
            current = currentFloat;
            max = maxFloat;
        }

        public float RandomFloat() //Random value of variables
        {
            return UnityEngine.Random.Range(current, max);
        }
    }
}
