using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataType
{
    public delegate void Callback();
    public delegate void Callback<T>(T arg);
}
