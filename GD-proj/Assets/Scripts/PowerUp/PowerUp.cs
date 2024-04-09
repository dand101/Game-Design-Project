using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

abstract public class PowerUp : MonoBehaviour
{
    [SerializeField] private GameObject player;

    protected ShootConfigScriptableObject GetPlayerGunConfig()
    {
        var playerGunSelector = player.GetComponent<PlayerController>().GunSelector;
        return playerGunSelector.ActiveGun.ShootConfig;
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

    protected abstract void Start();

    void Update()
    {
        
    }

    protected abstract void ApplyPowerUp();

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.tag == "Player")
        {
            ApplyPowerUp();
        }
    }
}
