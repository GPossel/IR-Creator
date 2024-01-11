using System.Collections.Generic;
using UnityEngine;

public class PoissonDiscSamplingClouds : MonoBehaviour
{

    public float radius3D = 3;
    public Vector3 regionSize3D = Vector3.one;
    public int rejectionSamples3D = 30;
    public Vector3 displayRadius3D = Vector3.one;
    List<Vector3> points3D = new List<Vector3>();

	public float PosX = 0f;
	[SerializeField] public GameObject cloudPrefab;

    private void Awake()
    {
    }

    private void Start()
    {
		PosX = this.transform.position.x;
		points3D = Generate3DPoints(radius3D, regionSize3D, rejectionSamples3D);
		SetUpCloudsPoissionSampling();
	}

    public void SetUpCloudsPoissionSampling()
	{
		if (points3D.Count > 0)
		{
            foreach (var point in points3D)
            {
				var recalculateNegativeValues = point.x + PosX;
				GameObject gameObject = Instantiate(cloudPrefab);
                gameObject.transform.position = new Vector3(recalculateNegativeValues, point.y, point.z);
                gameObject.transform.localScale = displayRadius3D;
				gameObject.transform.SetParent(this.transform);
            }
		}	
	}

    private void OnDrawGizmos()
    {
        points3D = Generate3DPoints(radius3D, regionSize3D, rejectionSamples3D);

        if (points3D != null)
        {
            Gizmos.DrawWireCube(regionSize3D / 2, regionSize3D);

            foreach (var point in points3D)
            {
                Gizmos.DrawCube(new Vector3(point.x, point.y, point.z), displayRadius3D);
            }
        }
    }


    public List<Vector3> Generate3DPoints(float radius3D, Vector3 sampleRegionSize3D, int rejectionSamples3D = 10)
	{
		float cellSize = radius3D / Mathf.Sqrt(3);

		int[,,] grid = new int[Mathf.CeilToInt((sampleRegionSize3D.x) / cellSize), Mathf.CeilToInt(sampleRegionSize3D.y / cellSize), Mathf.CeilToInt(sampleRegionSize3D.z / cellSize)];
		List<Vector3> points = new List<Vector3>();
		List<Vector3> spawnPoints = new List<Vector3>();

		spawnPoints.Add(sampleRegionSize3D / 3);
		while (spawnPoints.Count > 0)
		{
			int spawnIndex = Random.Range(0, spawnPoints.Count);
			Vector3 spawnCentre = spawnPoints[spawnIndex];
			bool candidateAccepted = false;

			for (int i = 0; i < rejectionSamples3D; i++)
			{
				float angle3D = Random.value * Mathf.PI * 3; // times three sides
				Vector3 dir = new Vector3(Mathf.Sin(angle3D), Mathf.Cos(angle3D), Mathf.Tan(angle3D));
				Vector3 candidate = spawnCentre + dir * Random.Range(radius3D, 3 * radius3D);

				// debug to see what this random range is translated

				if (IsValid3D(candidate, sampleRegionSize3D, cellSize, radius3D, points, grid))
				{
					points.Add(candidate);
					spawnPoints.Add(candidate);
					grid[(int)(candidate.x / cellSize), (int)(candidate.y / cellSize), (int)(candidate.z / cellSize)] = points.Count;
					candidateAccepted = true;
					break;
				}
			}
			if (!candidateAccepted)
			{
				spawnPoints.RemoveAt(spawnIndex);
			}

		}

		return points;
	}


	static bool IsValid3D(Vector3 candidate, Vector3 sampleRegionSize, float cellSize, float radius, List<Vector3> points, int[,,] grid)
	{
		if (candidate.x >= 0 && candidate.x < sampleRegionSize.x &&
			candidate.y >= 0 && candidate.y < sampleRegionSize.y &&
			candidate.z >= 0 && candidate.z < sampleRegionSize.z) // cannot be lower? not minus - ?
		{
			int cellX = (int)(candidate.x / cellSize);
			int cellY = (int)(candidate.y / cellSize);
			int cellZ = (int)(candidate.z / cellSize);
			int searchStartX = Mathf.Max(0, cellX - 2);
			int searchEndX = Mathf.Min(cellX + 2, grid.GetLength(0) - 1);
			int searchStartY = Mathf.Max(0, cellY - 2);
			int searchEndY = Mathf.Min(cellY + 2, grid.GetLength(1) - 1);
			int searchStartZ = Mathf.Max(0, cellZ - 2);
			int searchEndZ = Mathf.Min(cellZ + 2, grid.GetLength(2) - 1);

			// make <= is you don't mind spawning close to the edge
			for (int x = searchStartX; x <= searchEndX; x++)
			{
				for (int y = searchStartY; y <= searchEndY; y++)
				{
					for (int z = searchStartZ; z <= searchEndZ; z++)
					{
						// this will be a problem with the sqrt, 
						int pointIndex = grid[x, y, z] - 1;
						if (pointIndex != -1)
						{
							var offset = (candidate - points[pointIndex]).sqrMagnitude;
							if (offset < radius * radius /** radius*/)
							{
								return false;
							}
						}
					}
				}
			}
			return true;
		}
		return false;
	}

}