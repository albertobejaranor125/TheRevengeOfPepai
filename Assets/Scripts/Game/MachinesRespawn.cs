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
    [SerializeField] private float _minY = -2.50f;
    [SerializeField] private float _maxY = 2.50f;
    [SerializeField] private float _delaySpawn = 1.5f;
    [SerializeField] private int _maxMachines = 3;
    [SerializeField] private LayerMask _machineMask;
    [SerializeField] private LayerMask _platformMask;
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
        Vector3 spawnPos = Vector3.zero;
        bool validPos = false;
        int attempts = 0;
        while(attempts < 50 && !validPos)
        {
            float randomX = Random.Range(_minX, _maxX);
            float randomY = Random.Range(_minY, _maxY);
            spawnPos = new Vector3(randomX, randomY, 0f);
            if (IsValidPosition(spawnPos))
            {
                validPos = true;
            }
            attempts++;
        }
        if (validPos)
        {
            Instantiate(_machinesList[indexSpawn], spawnPos, Quaternion.identity);
            _currentMachines++;
        }
    }
    private bool IsValidPosition(Vector3 position)
    {
        float platformRadius = 2.5f;
        float machineRadius = 1.5f;
        Collider2D[] machinesScene = Physics2D.OverlapCircleAll(position, machineRadius, _machineMask);
        if (machinesScene.Length > 0)
        {
            return false;
        }
        Collider2D[] platformsScene = Physics2D.OverlapCircleAll(position, platformRadius, _platformMask);
        if (platformsScene.Length > 0)
        {
            return false;
        }
        return true;
    }
    public void OnMachineDestroyed()
    {
        _currentMachines--;
    }
    #endregion
}
