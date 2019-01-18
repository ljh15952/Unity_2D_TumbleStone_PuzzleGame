using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton  {
    public int selectLevel = 0;
    private static Singleton instance = null;

    public static Singleton Instance
    {
        get
        {
            if (instance == null)
                instance = new Singleton();
            return instance;
        }
    }
}
