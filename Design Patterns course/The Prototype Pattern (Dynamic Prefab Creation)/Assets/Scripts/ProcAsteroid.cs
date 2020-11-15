using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProcAsteroid: Object
{
    static GameObject asteroid;

    public static GameObject Clone(Vector3 pos)
    {
        if (asteroid == null)
        {
            CreateAsteroid(pos);
            asteroid.SetActive(false);
        }
        GameObject asteroidClone = new GameObject();
        asteroidClone.AddComponent<MeshFilter>();
        asteroidClone.AddComponent<MeshRenderer>();
        asteroidClone.GetComponent<MeshFilter>().sharedMesh = asteroid.GetComponent<MeshFilter>().sharedMesh;
        MeshRenderer rend = asteroidClone.GetComponent<MeshRenderer>();
        rend.sharedMaterial = asteroid.GetComponent<MeshRenderer>().sharedMaterial;
        asteroidClone.name = "asteroid(Clone)";
        asteroidClone.gameObject.SetActive(true);
        asteroidClone.transform.position = pos;
        return asteroidClone;
    }


    public static void CreateAsteroid(Vector3 pos)
    {
        asteroid = new GameObject();
        asteroid.AddComponent<MeshFilter>();
        asteroid.AddComponent<MeshRenderer>();

        //Construct asteroid Mesh =======================
        Mesh mesh = new Mesh();
        mesh.name = "asteroid_" + Time.realtimeSinceStartup.ToString();
        mesh.Clear();

        float radius = 1f; 
        int LONGITUDE = 50;
        int LATITUDE = 50;

        Vector3[] vertices = new Vector3[(LONGITUDE + 1) * LATITUDE + 2 * LONGITUDE];
        float PI2 = Mathf.PI * 2f;

        //NORTH POLE
        for (int v = 0; v <= LONGITUDE; v++)
        {
            vertices[v] = Vector3.up * radius;
        }

        for (int lat = 0; lat < LATITUDE; lat++)
        {
            float a1 = Mathf.PI * (float)(lat + 1) / (LATITUDE + 1);
            float sin1 = Mathf.Sin(a1);
            float cos1 = Mathf.Cos(a1);

            for (int lon = 0; lon <= LONGITUDE; lon++)
            {
                float a2 = PI2 * (float)(lon == LONGITUDE ? 0 : lon) / LONGITUDE;
                float sin2 = Mathf.Sin(a2);
                float cos2 = Mathf.Cos(a2);

                vertices[lon + lat * (LONGITUDE + 1) + LONGITUDE] = new Vector3(sin1 * cos2, cos1, sin1 * sin2) * radius;
            }
        }

        //SOUTH POLE
        for (int v = 1; v <= LONGITUDE; v++)
        {
            vertices[vertices.Length - v] = Vector3.up * -radius;   //last vertex - bottom most
        }

        Vector3[] normals = new Vector3[vertices.Length];
        for (int n = 0; n < vertices.Length; n++)
        {
            normals[n] = vertices[n].normalized;
        }

        Vector2[] uvs = new Vector2[vertices.Length];
        uvs[0] = Vector2.up;
        uvs[uvs.Length - 1] = Vector2.zero;
        for (int lat = 0; lat < LATITUDE; lat++)
            for (int lon = 0; lon <= LONGITUDE; lon++)
            {
                int uvpos = lon + lat * (LONGITUDE + 1) + LONGITUDE;
                uvs[uvpos] =
                        new Vector2((float)lon / LONGITUDE,
                            1f - (float)(lat + 1) / (LATITUDE + 1));

            }

        //top cap uvs
        float uvOffset = 1 / (float)LONGITUDE * 0.5f;
        for (int v = 0; v < LONGITUDE; v++)
        {
            uvs[v] = new Vector2(1 / (float)LONGITUDE * v + uvOffset, 1);
        }

        //bottom cap uvs
        int u = 0;
        for (int v = vertices.Length - LONGITUDE; v < vertices.Length; v++)
        {
            uvs[v] = new Vector2(1 / (float)LONGITUDE * u + uvOffset, 0);
            u++;
        }

        int totalFaces = vertices.Length;
        int totalTris = totalFaces * 2;
        int triIndexes = totalTris * 3;
        int[] triangles = new int[triIndexes];

        //Top Cap
        int i = 0;
        for (int lon = 0; lon <= LONGITUDE; lon++)
        {
            triangles[i++] = LONGITUDE + lon + 1;
            triangles[i++] = LONGITUDE + lon;
            triangles[i++] = lon;
        }

        //Middle
        for (int lat = 0; lat < LATITUDE - 1; lat++)
        {
            for (int lon = 0; lon < LONGITUDE; lon++)
            {
                int current = lon + lat * (LONGITUDE + 1) + LONGITUDE;
                int next = current + LONGITUDE + 1;

                triangles[i++] = current;
                triangles[i++] = current + 1;
                triangles[i++] = next + 1;

                triangles[i++] = current;
                triangles[i++] = next + 1;
                triangles[i++] = next;
            }
        }

        //Bottom Cap
        for (int lon = 0; lon <= LONGITUDE; lon++)
        {
            triangles[i++] = vertices.Length - 1 - lon;
            triangles[i++] = vertices.Length - LONGITUDE - (lon + 2);
            triangles[i++] = vertices.Length - LONGITUDE - (lon + 1);
        }

        Vector3 offsets = new Vector3(Random.Range(100, 200),
                                        Random.Range(100, 200),
                                        Random.Range(100, 200));
        for (int v = 0; v < vertices.Length; v++)
        {
            vertices[v] += normals[v] * 
                    Mathf.PerlinNoise((vertices[v].x + offsets.x)/0.4f, 
                                        (vertices[v].y + offsets.y)/0.5f)
                    * Mathf.PerlinNoise((vertices[v].x + offsets.x) / 0.4f,
                                        (vertices[v].z + offsets.z) / 0.5f)
                     * Mathf.PerlinNoise((vertices[v].y + offsets.y) / 0.6f,
                                        (vertices[v].z + offsets.z) / 0.5f);
            vertices[v].x *= 2;
        }

        mesh.vertices = vertices;
        mesh.normals = normals;
        mesh.uv = uvs;
        mesh.triangles = triangles;

        mesh.RecalculateBounds();
        //=========================================================

        asteroid.GetComponent<MeshFilter>().mesh = mesh;

        asteroid.name = "asteroid";
        asteroid.gameObject.SetActive(true);
        asteroid.transform.position = pos;
    }
}
