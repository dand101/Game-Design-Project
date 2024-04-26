using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

abstract public class PowerUp : MonoBehaviour
{
    protected GameObject player;

    private float amplitude = 0.4f;
    private float speed = 5f;
    private Vector3 startPosition;

    protected ShootConfigScriptableObject GetPlayerGunConfig()
    {
        var playerGunSelector = player.GetComponent<PlayerController>().GunSelector;
        return playerGunSelector.ActiveGun.ShootConfig;
    }

    protected GunAmmoConfig GetPlayerAmmoConfig()
    {
        var playerGunSelector = player.GetComponent<PlayerController>().GunSelector;
        return playerGunSelector.ActiveGun.gunAmmoConfig;
    }

    Light GetPointLight()
    {
        return transform.Find("Light").gameObject.GetComponent<Light>();
    }

    protected void SetupEmissiveColor(Color color)
    {
        Material material = new Material(Shader.Find("Standard"));

        material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        material.SetInt("_ZWrite", 0);
        material.DisableKeyword("_ALPHATEST_ON");
        material.EnableKeyword("_ALPHABLEND_ON");
        material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;

        material.color = color;

        material.EnableKeyword("_EMISSION");
        material.SetColor("_EmissionColor", color);
        material.globalIlluminationFlags = MaterialGlobalIlluminationFlags.RealtimeEmissive;

        GetComponent<Renderer>().material = material;
        GetPointLight().color = color;
    }

    private void Awake()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectsWithTag("Player")[0];
        }
    }

    protected virtual void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        float newY = startPosition.y + 0.75f + Mathf.Sin(Time.time * speed) * amplitude;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }

    protected abstract void ApplyPowerUp();

    public void DestroyPowerUp()
    {
        // event for when the animation finishes
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player")
        {
            // animation
            Animator animator = GetComponent<Animator>();
            if (animator != null)
            {
                animator.SetTrigger("Disappear");
            }

            GetComponent<Collider>().enabled = false;
            ApplyPowerUp();
        }
    }
}
