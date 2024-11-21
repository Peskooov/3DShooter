using System.Collections.Generic;
using UnityEngine;

public class EntitySpawner : MonoBehaviour
{
    public enum SpawnMode
    {
        Start,
        Loop
    }

    [SerializeField] private Entity[] entityPrefabs;
    [SerializeField] private SpawnMode spawnMode;

    [SerializeField] private CubeArea cubeArea;

    [SerializeField] private int countSpawns;
    [SerializeField] private float respawnTime;
    
    private List<Drone> spawnedEntities = new List<Drone>();
    private Drone[] drones;
    
    private float timer;

    private void Start()
    {
        if (spawnMode == SpawnMode.Start)
        {
            SpawnEntities();
        }

        timer = respawnTime;
    }

    private void Update()
    {
        if (timer > 0)
            timer -= Time.deltaTime;

        if (spawnMode == SpawnMode.Loop && timer < 0)
        {
            SpawnEntities();

            timer = respawnTime;
        }
        
        CheckForDestroyedEntities();
    }

    private void SpawnEntities()
    {
        if (entityPrefabs.Length <= 0) return;

        for (int i = 0; i < countSpawns; i++)
        {
            int index = Random.Range(0, entityPrefabs.Length);
            
            GameObject entities = Instantiate(entityPrefabs[index].gameObject);
            
            entities.transform.position = cubeArea.GetRandomInsideZone();

            Drone drone = entities.GetComponent<Drone>();
            spawnedEntities.Add(drone);
        }
    }
    
    private void CheckForDestroyedEntities()
    {
        for (int i = spawnedEntities.Count - 1; i >= 0; i--)
        {
            if (spawnedEntities[i] == null)
            {
                spawnedEntities.RemoveAt(i);
            }
        }
    }

    public void DisableEntities()
    {
        for (int i = 0; i < spawnedEntities.Count; i++)
        {
            spawnedEntities[i].Disable();
        }
    }
}