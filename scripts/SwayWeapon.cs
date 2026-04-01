using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;

public class SwayWeapon : MonoBehaviour
{
    PlayerCameraControl playerCamControl;
    Vector2 mouseInput;
    float sensitivityX;
    float sensitivityY;

    Quaternion startWeaponOriginRotation;
    Vector3 startWeaponOriginOffset;
    [SerializeField] Transform weaponOrigin;

    [SerializeField] Transform swayTarget;
    [Space]
    [SerializeField] float swaySpeed;
    [SerializeField] float weaponRotationReturnSpeed;
    [Space]
    [SerializeField] float weaponRotationMultiplier;
    [SerializeField] float originRotationMultiplier;

    [Header("Aiming")]
    [SerializeField] Vector3 aimingWeaponOriginOffset;
    [SerializeField] float aimingSpeed;

    bool aiming;


    private void Awake()
    {
        playerCamControl = FindObjectOfType<PlayerCameraControl>();
        sensitivityX = playerCamControl.sensitivityX;
        sensitivityY = playerCamControl.sensitivityY;

        startWeaponOriginRotation = weaponOrigin.localRotation;
        startWeaponOriginOffset = weaponOrigin.localPosition;
    }

    private void Update()
    {
        aiming = Input.GetMouseButton(1);

        mouseInput = new Vector2(-Input.GetAxis("Mouse Y") * sensitivityY, Input.GetAxis("Mouse X") * sensitivityX);
        weaponOrigin.localEulerAngles -= (Vector3)mouseInput * weaponRotationMultiplier;
        transform.localEulerAngles += (Vector3)mouseInput * originRotationMultiplier;
    }
    private void LateUpdate()
    {
        transform.localRotation = Quaternion.Lerp(transform.localRotation, swayTarget.localRotation, swaySpeed);
        weaponOrigin.localRotation = Quaternion.Lerp(weaponOrigin.localRotation, startWeaponOriginRotation, weaponRotationReturnSpeed);

        weaponOrigin.localPosition = Vector3.Lerp(weaponOrigin.localPosition, aiming ? aimingWeaponOriginOffset : startWeaponOriginOffset, aimingSpeed);
    }
}
