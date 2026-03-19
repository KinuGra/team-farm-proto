using UnityEngine;

public class FieldGenerator : MonoBehaviour
{
    public GameObject tilePrefab;
    public int size = 5;

    void Start()
    {
        float offset = size / 2f;

        for (int x = 0; x < size; x++)
        {
            for (int z = 0; z < size; z++)
            {
                Vector3 pos = transform.position + new Vector3(x - offset, 0, z - offset);
                Instantiate(tilePrefab, pos, Quaternion.identity, transform);
            }
        }
    }
}
