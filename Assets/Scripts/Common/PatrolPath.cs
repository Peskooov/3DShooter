using UnityEngine;

public class PatrolPath : MonoBehaviour
{
    [SerializeField] private PatrolPathNode[] nodes;

    private void Start()
    {
        UpdatePathNode();
    }

    [ContextMenu("Update path Node")]
    private void UpdatePathNode()
    {
        nodes = new PatrolPathNode[transform.childCount];

        for (int i = 0; i < nodes.Length; i++)
        {
            nodes[i] = transform.GetChild(i).GetComponent<PatrolPathNode>();
        }
    }

    public PatrolPathNode GetRandomPathNode()
    {
        return nodes[Random.Range(0, nodes.Length)];
    }

    public PatrolPathNode GetNextPathNode(ref int index)
    {
        index = Mathf.Clamp(index, 0, nodes.Length - 1);

        index++;

        if (index >= nodes.Length)
            index = 0;

        return nodes[index];
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (nodes == null) return;

        Gizmos.color = Color.red;

        for (int i = 0; i < nodes.Length - 1; i++)
        {
            Gizmos.DrawLine(nodes[i].transform.position + new Vector3(0, 0.5f, 0),
                nodes[i + 1].transform.position + new Vector3(0, 0.5f, 0));
        }

        Gizmos.DrawLine(nodes[0].transform.position + new Vector3(0, 0.5f, 0),
            nodes[nodes.Length - 1].transform.position + new Vector3(0, 0.5f, 0));
    }
#endif
}