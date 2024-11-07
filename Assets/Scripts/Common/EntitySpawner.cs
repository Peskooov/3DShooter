using UnityEngine;

public class EntitySpawner : MonoBehaviour
{
    public enum SpawnMode
    {
        Start,
        Loop
    }

    [SerializeField] private Entity[] entityPreafabs;
    [SerializeField] private SpawnMode spawnMode;

    [SerializeField] private CubeArea cubeArea;

    [SerializeField] private int countSpawns;
    [SerializeField] private float respawnTime;

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
    }

    private void SpawnEntities()
    {
        for (int i = 0; i < countSpawns; i++)
        {
            int index = Random.Range(0, entityPreafabs.Length);

            GameObject entities = Instantiate(entityPreafabs[index].gameObject);

            entities.transform.position = cubeArea.GetRandomInsideZone();
        }
    }
}