using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Security.Claims;
using System.Security.Cryptography;
//using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

public class Recoil : MonoBehaviour
{
    public bool isForCameraRecoil;

    Vector3 startTargetPosition;
    Vector3 startTargetRotation;

    bool aiming;
    [SerializeField] bool canAim;

    [Header("Recoil Curves")]
    [SerializeReference] AnimationCurve positionRecoilCurve;
    float positionCurve_LastKeyTime;
    float positionCurve_CurveTime;
    [SerializeReference] AnimationCurve rotationRecoilCurve;
    float rotationCurve_LastKeyTime;
    float rotationCurve_CurveTime;

    [Header("Recoil Strenghts")]
    [SerializeField] Vector3 positionRecoilStrenght;
    [SerializeField] Vector3 RNG_positionRecoilStrenght;

    [SerializeField] Vector3 rotationRecoilStrenght;
    [SerializeField] Vector3 RNG_rotationRecoilStrenght;

    [Header("Aiming Recoil Strenght")]
    [SerializeField] Vector3 Aiming_positionRecoilStrenght;
    [SerializeField] Vector3 Aiming_RNG_positionRecoilStrenght;

    [SerializeField] Vector3 Aiming_rotationRecoilStrenght;
    [SerializeField] Vector3 Aiming_RNG_rotationRecoilStrenght;

    [Header("ShootPublicInfo")]
    Vector3 curPositionStrenght;
    Vector3 curRotationStrenght;

    private void Awake()
    {
        AwakeWork();
    }
    void AwakeWork()
    {
        startTargetPosition = transform.localPosition;
        startTargetRotation = transform.localRotation.eulerAngles;

        //Setting last frame times of both curves
        if (positionRecoilCurve != null)
        {
            var positionCurve_lastframe = positionRecoilCurve[positionRecoilCurve.length - 1];
            positionCurve_LastKeyTime = positionCurve_lastframe.time;

            var rotationCurve_lastframe = rotationRecoilCurve[rotationRecoilCurve.length - 1];
            rotationCurve_LastKeyTime = rotationCurve_lastframe.time;
        }
        
    }

    public void Shoot()
    {
        positionCurve_CurveTime = 0;
        rotationCurve_CurveTime = 0;

        if (!aiming)
        {
            curPositionStrenght = new Vector3(Random.Range(positionRecoilStrenght.x - RNG_positionRecoilStrenght.x, positionRecoilStrenght.x + RNG_positionRecoilStrenght.x), Random.Range(positionRecoilStrenght.y - RNG_positionRecoilStrenght.y, positionRecoilStrenght.y + RNG_positionRecoilStrenght.y), Random.Range(positionRecoilStrenght.z - RNG_positionRecoilStrenght.z, positionRecoilStrenght.z + RNG_positionRecoilStrenght.z));
            curRotationStrenght = new Vector3(Random.Range(rotationRecoilStrenght.x - RNG_rotationRecoilStrenght.x, rotationRecoilStrenght.x + RNG_rotationRecoilStrenght.x), Random.Range(rotationRecoilStrenght.y - RNG_rotationRecoilStrenght.y, rotationRecoilStrenght.y + RNG_rotationRecoilStrenght.y), Random.Range(rotationRecoilStrenght.z - RNG_rotationRecoilStrenght.z, rotationRecoilStrenght.z + RNG_rotationRecoilStrenght.z));
        } else
        {
            curPositionStrenght = new Vector3(Random.Range(Aiming_positionRecoilStrenght.x - Aiming_RNG_positionRecoilStrenght.x, Aiming_positionRecoilStrenght.x + Aiming_RNG_positionRecoilStrenght.x), Random.Range(Aiming_positionRecoilStrenght.y - Aiming_RNG_positionRecoilStrenght.y, Aiming_positionRecoilStrenght.y + Aiming_RNG_positionRecoilStrenght.y), Random.Range(Aiming_positionRecoilStrenght.z - Aiming_RNG_positionRecoilStrenght.z, Aiming_positionRecoilStrenght.z + Aiming_RNG_positionRecoilStrenght.z));
            curRotationStrenght = new Vector3(Random.Range(Aiming_rotationRecoilStrenght.x - Aiming_RNG_rotationRecoilStrenght.x, Aiming_rotationRecoilStrenght.x + Aiming_RNG_rotationRecoilStrenght.x), Random.Range(Aiming_rotationRecoilStrenght.y - Aiming_RNG_rotationRecoilStrenght.y, Aiming_rotationRecoilStrenght.y + Aiming_RNG_rotationRecoilStrenght.y), Random.Range(Aiming_rotationRecoilStrenght.z - Aiming_RNG_rotationRecoilStrenght.z, Aiming_rotationRecoilStrenght.z + Aiming_RNG_rotationRecoilStrenght.z));
        }
    }

    private void Update()
    {
        aiming = canAim ? Input.GetMouseButton(1) : false;
    }
    void FixedUpdate()
    {
        positionCurve_CurveTime += Time.fixedDeltaTime;
        rotationCurve_CurveTime += Time.fixedDeltaTime;


        if (positionRecoilCurve != null && positionCurve_CurveTime < positionCurve_LastKeyTime && rotationCurve_CurveTime < rotationCurve_LastKeyTime)
        {
            Vector3 curPositionRecoilValue = curPositionStrenght * positionRecoilCurve.Evaluate(positionCurve_CurveTime);
            Vector3 curRotationRecoilValue = curRotationStrenght * rotationRecoilCurve.Evaluate(rotationCurve_CurveTime);

            transform.SetLocalPositionAndRotation
                (startTargetPosition + curPositionRecoilValue,
                Quaternion.Euler(startTargetRotation + curRotationRecoilValue));
        } else
        {
            ResetRecoilAnimation();
        }
    }
    public void ResetRecoilAnimation()
    {
        transform.localPosition = startTargetPosition;
        transform.localRotation = Quaternion.Euler(startTargetRotation);
    }

    public void SetSettingsFromOtherRecoil(Recoil targetSettings)
    {
        canAim = targetSettings.canAim;

        if (positionRecoilCurve != null)
        {
            positionRecoilCurve = targetSettings.positionRecoilCurve;
        }
        positionRecoilStrenght = targetSettings.positionRecoilStrenght;
        positionCurve_LastKeyTime = targetSettings.positionCurve_LastKeyTime;
        Aiming_positionRecoilStrenght = targetSettings.Aiming_positionRecoilStrenght;
        Aiming_RNG_positionRecoilStrenght = targetSettings.Aiming_RNG_positionRecoilStrenght;

        rotationRecoilCurve = targetSettings.rotationRecoilCurve;
        rotationRecoilStrenght = targetSettings.rotationRecoilStrenght;
        rotationCurve_LastKeyTime = targetSettings.rotationCurve_LastKeyTime;
        Aiming_rotationRecoilStrenght = targetSettings.Aiming_rotationRecoilStrenght;
        Aiming_RNG_rotationRecoilStrenght = targetSettings.Aiming_RNG_rotationRecoilStrenght;

        AwakeWork();
    }
}
