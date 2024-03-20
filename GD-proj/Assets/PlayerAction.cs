using UnityEngine;

[DisallowMultipleComponent]
public class PlayerAction : MonoBehaviour
{
    [SerializeField]
    private PlayerGunSelector GunSelector;

    private void Update()
    {
        if (Input.GetButton("Fire1"))
        {
            GunSelector.ActiveGun.Shoot();
        }
    }
}
