using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StopTime
{
    private static float startTime = Time.time;


    public static void resetTimer()
    {
        startTime = Time.time;
    }


    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T">Can be float or string</typeparam>
    /// <returns></returns>
    public static T GetTime<T>() where T : class
    {
        var currentTime = Time.time - startTime;


        if(typeof(T) == typeof(string))
        {

            int minutes = (int)Mathf.Floor(currentTime / 60);
            int seconds = (int)Mathf.Floor(currentTime % 60);

            return $"{minutes:0}:{seconds:00}" as T;
        }else if(typeof(T) == typeof(float))
        {
            return currentTime as T;
        }
        return default;
    }

}
