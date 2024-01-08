using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxSpawner : MonoBehaviour
{
    [SerializeField] private SpawnPoint[] _spawners;
    [SerializeField] private float _delay;
    [SerializeField] private Box _prefab;

    private void Start()
    {
        StartCoroutine(CreateBox());
    }

    private IEnumerator CreateBox()
    {
        var time = new WaitForSeconds(_delay);
        int minimalIndex = 0;

        while (true)
        {
            int index = Random.Range(minimalIndex, _spawners.Length);
            Box box = Instantiate(_prefab, _spawners[index].transform.position, Quaternion.identity);

            yield return time;
        }
    }
}
