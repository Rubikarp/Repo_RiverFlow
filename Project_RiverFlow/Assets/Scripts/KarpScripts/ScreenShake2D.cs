using System.Collections;
using System.Collections.Generic;
using UnityEngine;

///Source https://youtu.be/tu-Qe66AvtY
public class ScreenShake2D : MonoBehaviour
{
    [Header("View")]
    [Range(0, 1)] public float trauma;
    [Range(0, 1)] public float cameraShake;
    [Header("Parameter")]
    public Transform target;
    [Header("Parameter")]
    [Range(1, 4)] public float traumaStrenght = 2;
    [Range(1, 60)] public float traumaFrequency = 12;
    [Range(0, 10)] public float traumaMagnitudeTranslate = 12f;
    [Range(0, 90)] public float traumaMagnitudeRotation = 18f;
    [Range(0, 3)] public float traumaFallFactor = 1.24f;

    float cameraTime = 0;

    private void Update()
    {
        trauma = Mathf.Clamp01(trauma);
        cameraShake = trauma * trauma;

        if (cameraShake > 0)
        {
            //Le temps de la camera avance
            cameraTime += Time.deltaTime * traumaFrequency;

            //Calcul du mouvement de la caméra pour trembler
            Vector2 newPosTranslate = PerlinNoiseVector(100,200);
            Vector3 newPosRotate = new Vector3(0, 0, PerlinNoiseRandom(400));

            //Movement de la camera multiplié par la puissance du screenshake
            target.localPosition = newPosTranslate * traumaMagnitudeTranslate * cameraShake;
            //Rotation de la camera
            target.localRotation = Quaternion.Euler(newPosRotate * traumaMagnitudeRotation * cameraShake);

            //Déclin du screen shake
            trauma -= Time.deltaTime * (1/traumaFallFactor) + (cameraShake / 300);
        }
        else 
        {
            transform.localPosition = Vector2.zero;
            transform.localRotation = Quaternion.Euler(new Vector3(0,0,0));
        }
    }

    public void AddShake(float power)
    {
        trauma += power;
    }

    private Vector2 PerlinNoiseVector(float seed1, float seed2)
    {
        return new Vector2(PerlinNoiseRandom(seed1), PerlinNoiseRandom(seed2));
    }
    private float PerlinNoiseRandom(float seed)
    {
        float value;

        //[0:1]
        value = Mathf.PerlinNoise(seed, cameraTime);
        //Remap to [-1:1]
        value = (value - 0.5f) / 2;
        //valeur entre -traumaStrenght et traumaStrenght
        value *= traumaStrenght;

        return value;
    }
}
