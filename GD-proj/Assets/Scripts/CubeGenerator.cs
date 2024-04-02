using UnityEngine;
using System.Collections.Generic;

public class CubeGenerator : MonoBehaviour
{
    public GameObject cubePrefab;
    public int minNumCubesPerPlane = 2;
    public int maxNumCubesPerPlane = 10;
    public float minDistanceBetweenCubes = 0.5f; 
    private List<GameObject> generatedCubes = new List<GameObject>();

    public float minDim = 0.5f;

    public float maxDim = 2f;


    public Material cubeMaterial;

    public Material wallMaterial;

    public GameObject player;

    void Generate()
    {
             GameObject[] planes = GameObject.FindGameObjectsWithTag("Floor");

        foreach (GameObject plane in planes)
        {
            Bounds bounds = GetWorldBounds(plane);
            Vector3 planeCenter = bounds.center;
            Vector3 planeSize = bounds.size;

            Vector3 cubeSize = cubePrefab.transform.localScale;
            float halfCubeWidth = cubeSize.x * 0.5f;
            float halfCubeHeight = cubeSize.y * 0.5f;
            float halfCubeDepth = cubeSize.z * 0.5f;
            int numCubesPerPlane = Random.Range(minNumCubesPerPlane, maxNumCubesPerPlane + 1);

            List<Vector3> cubePositions = new List<Vector3>();

            for (int i = 0; i < numCubesPerPlane; i++)
            {
                Vector3 cubePos = Vector3.zero;
                bool validPosition = false;
                float scale = Random.Range(minDim, maxDim);
                
                int attempts = 0;
                while (!validPosition && attempts < 100)
                {
                    cubePos = new Vector3(
                        Random.Range(planeCenter.x - planeSize.x / 2 + halfCubeWidth * scale, planeCenter.x + planeSize.x / 2 - halfCubeWidth * scale),
                        planeCenter.y + halfCubeHeight * scale + 0.01f,
                        Random.Range(planeCenter.z - planeSize.z / 2 + halfCubeHeight * scale, planeCenter.z + planeSize.z / 2 - halfCubeHeight * scale)
                    );

                   
                    validPosition = true;
                    foreach (Vector3 existingPos in cubePositions)
                    {
                        if (Vector3.Distance(cubePos, existingPos) < 4 * halfCubeWidth *  Mathf.Sqrt(2))
                        {
                            validPosition = false;
                            break;
                        }
                    }

                    foreach (GameObject existingCube in generatedCubes)
                    {
                        Collider existingCollider = existingCube.GetComponent<Collider>();
                        if (existingCollider.bounds.Contains(cubePos) )
                        {
                            validPosition = false;
                            break;
                        
                        }

                        Vector3 existingCubeSize = existingCube.transform.localScale;
                        if (Vector3.Distance(cubePos, existingCube.transform.position) < (existingCubeSize.x + scale * cubeSize.x))
                        {
                            validPosition = false;
                            break;
                        }

                    }

                    Collider playerCollider = player.GetComponent<Collider>();
                    if (Vector3.Distance(cubePos, player.transform.position) < (scale + 1) * halfCubeWidth * Mathf.Sqrt(2)  || playerCollider.bounds.Contains(cubePos))
                    {
                        validPosition = false;
                    }

                    attempts++;
                }

                if (validPosition)
                {
                    Vector3 cubeScale = cubePrefab.transform.localScale * scale;
                    GameObject newCube = Instantiate(cubePrefab, cubePos, Quaternion.identity);
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


    void Start()
    {
       Generate();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            foreach (GameObject cube in generatedCubes)
            {
                Destroy(cube);

            }
            generatedCubes.Clear();
            Color randomColor = GenerateRandomColor();
            if (cubeMaterial != null)
            {
                Color newColor = randomColor * 1.5f;
                 cubeMaterial.SetColor("_EmissionColor", newColor);
                 cubeMaterial.EnableKeyword("_EMISSION");
            }
            if (wallMaterial != null)
            {
                wallMaterial.SetColor("_EmissionColor", randomColor);
                wallMaterial.EnableKeyword("_EMISSION");
            }
            Generate();
        }
    }

    private Bounds GetWorldBounds(GameObject obj)
    {
        Renderer[] renderers = obj.GetComponentsInChildren<Renderer>();
        Bounds bounds = new Bounds();

        foreach (Renderer renderer in renderers)
        {
            bounds.Encapsulate(renderer.bounds);
        }

        return bounds;
    }

    private Color previousColor;
    private const float colorDifferenceThreshold = 0.5f;
     public Color GenerateRandomColor()
    {
        Color randomColor;
  do
        {

            float r = Random.Range(0.1f, 1.0f);
            float g = Random.Range(0.1f, 1.0f);
            float b = Random.Range(0.1f, 1.0f);

            randomColor = new Color(r, g, b);
        } while (ColorDifference(previousColor, randomColor) < colorDifferenceThreshold);


        previousColor = randomColor;

        return randomColor;
    }

     private float ColorDifference(Color color1, Color color2)
    {
        return Mathf.Sqrt(Mathf.Pow(color1.r - color2.r, 2) + Mathf.Pow(color1.g - color2.g, 2) + Mathf.Pow(color1.b - color2.b, 2));
    }
}
