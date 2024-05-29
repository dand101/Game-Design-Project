using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CubeGenerator : MonoBehaviour
{
    private const float colorDifferenceThreshold = 0.5f;
    public GameObject cubePrefab;
    public int minNumCubesPerPlane = 2;
    public int maxNumCubesPerPlane = 10;
    public float minDistanceBetweenCubes = 0.5f;

    public NavMeshSurface navMeshSurface;

    public float minDim = 0.5f;

    public float maxDim = 2f;


    public Material cubeMaterial;

    public Material wallMaterial;

    public GameObject player;
    private readonly List<GameObject> generatedCubes = new();

    private Color previousColor;


    private void Start()
    {
        foreach (var cube in generatedCubes) Destroy(cube);

        generatedCubes.Clear();
        var randomColor = GenerateRandomColor();
        if (cubeMaterial != null)
        {
            var newColor = randomColor * 1.5f;
            cubeMaterial.SetColor("_EmissionColor", newColor);
            cubeMaterial.EnableKeyword("_EMISSION");
        }

        if (wallMaterial != null)
        {
            wallMaterial.SetColor("_EmissionColor", randomColor);
            wallMaterial.EnableKeyword("_EMISSION");
        }

        Generate();
        navMeshSurface.BuildNavMesh();
    }

    private void Generate()
    {
        var planes = GameObject.FindGameObjectsWithTag("Floor");

        if (PlayerPrefs.GetInt("HardCoreDifficulty") == 1)
        {
            minNumCubesPerPlane += 3;
            maxNumCubesPerPlane += 3;
        }

        foreach (var plane in planes)
        {
            var bounds = GetWorldBounds(plane);
            var planeCenter = bounds.center;
            var planeSize = bounds.size;

            var cubeSize = cubePrefab.transform.localScale;
            var halfCubeWidth = cubeSize.x * 0.5f;
            var halfCubeHeight = cubeSize.y * 0.5f;
            var halfCubeDepth = cubeSize.z * 0.5f;
            var numCubesPerPlane = Random.Range(minNumCubesPerPlane, maxNumCubesPerPlane + 1);

            var cubePositions = new List<Vector3>();

            for (var i = 0; i < numCubesPerPlane; i++)
            {
                var cubePos = Vector3.zero;
                var validPosition = false;
                var scale = Random.Range(minDim, maxDim);

                var attempts = 0;
                while (!validPosition && attempts < 100)
                {
                    cubePos = new Vector3(
                        Random.Range(planeCenter.x - planeSize.x / 2 + halfCubeWidth * scale,
                            planeCenter.x + planeSize.x / 2 - halfCubeWidth * scale),
                        planeCenter.y + halfCubeHeight * scale + 0.01f,
                        Random.Range(planeCenter.z - planeSize.z / 2 + halfCubeHeight * scale,
                            planeCenter.z + planeSize.z / 2 - halfCubeHeight * scale)
                    );


                    validPosition = true;
                    foreach (var existingPos in cubePositions)
                        if (Vector3.Distance(cubePos, existingPos) < 4 * halfCubeWidth * Mathf.Sqrt(2))
                        {
                            validPosition = false;
                            break;
                        }

                    foreach (var existingCube in generatedCubes)
                    {
                        var existingCollider = existingCube.GetComponent<Collider>();
                        if (existingCollider.bounds.Contains(cubePos))
                        {
                            validPosition = false;
                            break;
                        }

                        var existingCubeSize = existingCube.transform.localScale;
                        if (Vector3.Distance(cubePos, existingCube.transform.position) <
                            existingCubeSize.x + scale * cubeSize.x)
                        {
                            validPosition = false;
                            break;
                        }
                    }

                    var playerCollider = player.GetComponent<Collider>();
                    if (Vector3.Distance(cubePos, player.transform.position) <
                        (scale + 1) * halfCubeWidth * Mathf.Sqrt(2) || playerCollider.bounds.Contains(cubePos))
                        validPosition = false;

                    attempts++;
                }

                if (validPosition)
                {
                    var cubeScale = cubePrefab.transform.localScale * scale;
                    var newCube = Instantiate(cubePrefab, cubePos, Quaternion.identity);
                    newCube.transform.localScale = cubeScale;
                    cubePositions.Add(cubePos);
                    generatedCubes.Add(newCube);
                }
                else
                {
                    break;
                }
            }
        }
    }

    // void Update()
    // {
    //     if (Input.GetKeyDown(KeyCode.Space))
    //     {
    //         foreach (GameObject cube in generatedCubes)
    //         {
    //             Destroy(cube);
    //         }
    //
    //         generatedCubes.Clear();
    //         Color randomColor = GenerateRandomColor();
    //         if (cubeMaterial != null)
    //         {
    //             Color newColor = randomColor * 1.5f;
    //             cubeMaterial.SetColor("_EmissionColor", newColor);
    //             cubeMaterial.EnableKeyword("_EMISSION");
    //         }
    //
    //         if (wallMaterial != null)
    //         {
    //             wallMaterial.SetColor("_EmissionColor", randomColor);
    //             wallMaterial.EnableKeyword("_EMISSION");
    //         }
    //
    //         Generate();
    //     }
    // }

    private Bounds GetWorldBounds(GameObject obj)
    {
        var renderers = obj.GetComponentsInChildren<Renderer>();
        var bounds = new Bounds();

        foreach (var renderer in renderers) bounds.Encapsulate(renderer.bounds);

        return bounds;
    }

    public Color GenerateRandomColor()
    {
        Color randomColor;
        do
        {
            var r = Random.Range(0.1f, 1.0f);
            var g = Random.Range(0.1f, 1.0f);
            var b = Random.Range(0.1f, 1.0f);

            randomColor = new Color(r, g, b);
        } while (ColorDifference(previousColor, randomColor) < colorDifferenceThreshold);


        previousColor = randomColor;

        return randomColor;
    }

    private float ColorDifference(Color color1, Color color2)
    {
        return Mathf.Sqrt(Mathf.Pow(color1.r - color2.r, 2) + Mathf.Pow(color1.g - color2.g, 2) +
                          Mathf.Pow(color1.b - color2.b, 2));
    }
}