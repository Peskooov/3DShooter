using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SceneSerializer : MonoBehaviour
{
    [System.Serializable]
    public class SceneObjectState
    {
        public int sceneId;
        public long entityId;
        public string state;
    }

    [SerializeField] private PrefabsDataBase m_PrefabsDataBase;

    public void SaveScene()
    {
        SaveToFile("Test.dat");
    }

    public void LoadScene()
    {
        LoadFromFile("Test.dat");
    }

    private void SaveToFile(string filePath)
    {
        List<SceneObjectState> savedObjects = new List<SceneObjectState>();

        // Получение всех сохраняемых объектов на сцене
        foreach (var entity in FindObjectsOfType<Entity>())
        {
            ISerializableEntity serializableEntity = entity as ISerializableEntity;

            if (serializableEntity == null) continue;

            if (serializableEntity.IsSerializable() == false) continue;

            SceneObjectState s = new SceneObjectState();

            s.entityId = serializableEntity.EntityId;
            s.state = serializableEntity.SerializeState();

            savedObjects.Add(s);
        }

        if (savedObjects.Count == 0)
        {
            Debug.Log("List saved objects is empty!");
            return;
        }

        // Записать в файл
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/" + filePath);

        bf.Serialize(file, savedObjects);

        file.Close();

        Debug.Log("Scene saved! Path file: " + Application.persistentDataPath + "/" + filePath);
    }

    private void LoadFromFile(string filePath)
    {
        Player.Instance.Destroy();

        foreach (var entity in FindObjectsOfType<Entity>())
        {
            Destroy(entity.gameObject);
        }

        // Заполняем список информации о всех загруженных объектах
        List<SceneObjectState> loadedObjects = new List<SceneObjectState>();

        if (File.Exists(Application.persistentDataPath + "/" + filePath))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/" + filePath, FileMode.Open);

            loadedObjects = (List<SceneObjectState>)bf.Deserialize(file);
            file.Close();
        }

        if (m_PrefabsDataBase == null)
        {
            Debug.LogError("m_PrefabsDataBase is not assigned!");
            return;
        }

        // Заспавниваем игрока
        foreach (var v in loadedObjects)
        {
            if (m_PrefabsDataBase.IsPlayerID(v.entityId))
            {
                GameObject p = m_PrefabsDataBase.CreatePlayer();

                p.GetComponent<ISerializableEntity>().DeserializeState(v.state);

                loadedObjects.Remove(v);
                break;
            }
        }


        // Заспавниваем все объекты
        foreach (var v in loadedObjects)
        {
            GameObject g = m_PrefabsDataBase.CreateEntityFromId(v.entityId);

            g.GetComponent<ISerializableEntity>().DeserializeState(v.state);
        }

        Debug.Log("Scene loaded! Path file: " + Application.persistentDataPath + "/" + filePath);
    }
}