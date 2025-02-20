using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Interface for objects that can be distorted in time (platforms, characters) , as well as those that can cause distortion in time ( arrows, explosions, etc) 
/// </summary>
public interface IDistortable
{
    float CustomTimeScale { get; set; } 
    void Distort(float timeScale); //simple, permanent distortion
    void Distort(float timeScale, float duration) //distortion with a duration. 
    {
        Distort(timeScale); //default to simple distortion if no duration is provided
    }
    //void Distort(float timeScale, float duration, float distortionRadius) //distortion with a duration and radius for things like shockwaves and explosions 
    //{
    //    Distort(timeScale, duration); //default to simple distortion if no radius is provided
    //}

    


}
