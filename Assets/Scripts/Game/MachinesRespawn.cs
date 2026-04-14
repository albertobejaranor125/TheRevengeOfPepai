using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MachinesRespawn : MonoBehaviour
{
    #region Variables
    [Header("Machines Settings")]
    [SerializeField] private List<GameObject> _machinesList;
    [Header("Spawn Settings")]
    [SerializeField] private float _minX = -11.30f;
    [SerializeField] private float _maxX = 11.30f;
    [SerializeField] private float _pointY = -2.50f;
    [SerializeField] private float _delaySpawn = 1.5f;
    [SerializeField] private int _maxMachines = 3;
    private int _currentMachines;
    #endregion
    #region Unity Messages
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _currentMachines = 0;
        StartCoroutine(SpawnLoop());
    }
    private void OnEnable()
    {
        MachineController.OnMachineDestroyed += OnMachineDestroyed;
    }
    private void OnDisable()
    {
        MachineController.OnMachineDestroyed -= OnMachineDestroyed;
    }
    #endregion
    #region Methods
    private IEnumerator SpawnLoop()
    {
        while (TimerScript.Instance.IsPlaying)
        {
            if (_currentMachines < _maxMachines)
            {
                SpawnMachines();
            }
            yield return new WaitForSeconds(_delaySpawn);
        }
    }
    private void SpawnMachines()
    {
        int indexSpawn = Random.Range(0, _machinesList.Count);
        float randomX = Random.Range(_minX, _maxX);
        Vector3 spawnPos = new Vector3(randomX, _pointY, 0f);
        Instantiate(_machinesList[indexSpawn], spawnPos, Quaternion.identity);
        _currentMachines++;
    }
    public void OnMachineDestroyed()
    {
        _currentMachines--;
    }
    #endregion
}
