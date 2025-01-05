using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PrefabsDataBase : ScriptableObject
{
    public List<Entity> AllPrefabs;

    public GameObject CreateEntityFromId(long id)
    {
        foreach (var entity in AllPrefabs)
        {
            if ((entity is ISerializableEntity) == false) continue;

            if ((entity as ISerializableEntity).EntityId == id)
            {
                return Instantiate(entity.gameObject);
            }
        }

        return null;
    }
}