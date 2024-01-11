using System.Collections.Generic;
using UnityEngine;

public static class SpawnHelper
{
    public static Transform PickRandomSpawnLocation(List<GameObject> allPrefabsCreated, List<GameObject> allPrefabScenesCreated, GameMode gameMode)
    {
        var randomFieldindex = UnityEngine.Random.Range(0, allPrefabScenesCreated.Count);
        var chosenField = allPrefabScenesCreated[randomFieldindex];

        Transform locationForCoin = null;

        switch (gameMode)
        {
            case GameMode.None:
                break;
            case GameMode.Classic:
                locationForCoin = chosenField.GetComponent<PrefabRunnerSpawnPoints>().ReturnRandomLocationForITemToSpawn();
                break;
            case GameMode.Treasure:
                locationForCoin = chosenField.GetComponent<PrefabTreasureSpawnPoints>().ReturnRandomLocationForITemToSpawn();
                break;
            case GameMode.Mission:
                break;
            default:
                break;
        }

        if(locationForCoin == null)
            locationForCoin = chosenField.GetComponent<PrefabTreasureSpawnPoints>().ReturnRandomLocationForITemToSpawn();

        return locationForCoin;
    }

    public static GameObject ReturnRandomPrefab(GameObject[] objectOfFolder)
    {
        var randomIndex = Random.Range(0, objectOfFolder.Length);
        var area = objectOfFolder[randomIndex];

        return area;
    }

    public static GameObject ReturnRandomPrefab(List<ArrayPrefabObjectBasic> objectSceneCollection)
    {
        var randomIndex = Random.Range(0, objectSceneCollection[0].Objects.Count);
        var prefabToCreate = objectSceneCollection[0].Objects[randomIndex];
        // we do the randomization a bit better
        // the position now is very stakato
        var prefabTreasureSpawn = new GameObject($"treasurespawn_{randomIndex}");
        prefabTreasureSpawn = prefabToCreate.Object;
        return prefabTreasureSpawn;

    }

    public static GameObject CreateSinglePrefabScenes(Transform[] spawnDirectionOrAreasOnWorld, List<ArrayPrefabObjectBasic> objectSceneCollection)
    {
        for (int i = 0; i < spawnDirectionOrAreasOnWorld.Length; i++)
        {
            var randomIndex = Random.Range(0, objectSceneCollection.Count);

            var collectionToUse = objectSceneCollection[randomIndex];

            var randomObjectIndex = Random.Range(0, collectionToUse.Objects.Count);

            var prefabToCreate = collectionToUse.Objects[randomObjectIndex];

            if (prefabToCreate != null)
            {
                // we do the randomization a bit better
                // the position now is very stakato
                var prefabTreasureSpawn = new GameObject($"treasurespawn_{i}");
                prefabTreasureSpawn = prefabToCreate.Object;
                prefabTreasureSpawn.transform.position = spawnDirectionOrAreasOnWorld[i].position;
                prefabTreasureSpawn.transform.rotation = spawnDirectionOrAreasOnWorld[i].rotation;
                return prefabTreasureSpawn;
            }
        }

        return null;
    }

    public static GameObject CreateSinglePrefabScenesSpawnOnOnePosition(Transform exactSpawnLocation, List<ArrayPrefabObjectBasic> objectSceneCollection)
    {
        var randomIndex = Random.Range(0, objectSceneCollection.Count);

        var collectionToUse = objectSceneCollection[randomIndex];

        var randomObjectIndex = Random.Range(0, collectionToUse.Objects.Count);

        var prefabToCreate = collectionToUse.Objects[randomObjectIndex];

        if (prefabToCreate != null)
        {
            var prefabTreasureSpawn = new GameObject($"exactSpawn_posx_{exactSpawnLocation.position.x}");
            prefabTreasureSpawn = prefabToCreate.Object;
            prefabTreasureSpawn.transform.position = exactSpawnLocation.position;
            prefabTreasureSpawn.transform.rotation = exactSpawnLocation.rotation;

            return prefabTreasureSpawn;
        }

        return null;
    }

    public static void RotateObjectSlightlyRandom(Transform transform)
    {
        Vector3 euler = transform.localEulerAngles;
        euler.z = Mathf.Lerp(euler.z, transform.rotation.z, 2.0f * Time.deltaTime); // Mathf.Lerp(euler.z, z, 2.0f * Time.deltaTime);
        transform.localEulerAngles = euler;
    }

    // ref: https://answers.unity.com/questions/861745/what-is-the-best-way-to-check-a-spawn-position.html
    public static bool IsSpawnPosLegalSphere(Vector3 position, float radius = 1)
    {
        Collider[] colliders = Physics.OverlapSphere(position, radius);

        foreach (Collider collider in colliders)
        {
            // look if a coin is alrady spawned
            if (collider.gameObject.GetComponent<IPickUp<int>>() != null || collider.gameObject.GetComponent<ItemController>() != null)
            {
                return false;
            }
        }
        return true;
    }

    public static bool IsSpawnPosLegalBox(Vector3 position, float sizeXYZ = 1)
    {
        Collider[] colliders = Physics.OverlapBox(position, new Vector3(sizeXYZ, sizeXYZ, sizeXYZ));

        foreach (Collider collider in colliders)
        {
            if (collider.gameObject.GetComponent<IPickUp<int>>() != null)
            {
                return false;
            }
        }
        return true;
    }

    internal static int GetRandomNumber(int count)
    {
        var randomIndex = Random.Range(0, count);
        return randomIndex;
    }
}