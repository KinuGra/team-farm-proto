using UnityEngine;

public class FieldGenerator : MonoBehaviour
{
    [SerializeField] private GameObject tilePrefab;
    [SerializeField] private GameObject framePrefab;
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
        float framePos = ((int)offset) - offset;
        GameObject createdFrame = Instantiate(framePrefab, transform.position + new Vector3(framePos, -0.4f, framePos), Quaternion.identity, transform);
        createdFrame.transform.localScale = new Vector3(size + 0.5f, 1.0f, size + 0.5f);
    
    }

}
