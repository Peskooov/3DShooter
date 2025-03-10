﻿using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PrefabsDataBase : ScriptableObject
{
    public Entity PlayerPrefabs;
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

    public bool IsPlayerID(long id)
    {
        return id == (PlayerPrefabs as ISerializableEntity).EntityId;
    }

    public GameObject CreatePlayer()
    {
        return Instantiate(PlayerPrefabs.gameObject);
    }
}