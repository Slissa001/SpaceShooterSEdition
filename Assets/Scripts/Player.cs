using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class Player : MonoBehaviour

{
    // Start is called before the first frame update

    [SerializeField]
    private float _speed = 5.0f;
    [SerializeField]
    private float _speedboost = 2.0f;
    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _tripleshotprefab;
    [SerializeField]
    private GameObject _shieldVisualizer;
    private Vector3 _tripleshotOffset = new Vector3(0, -1.05f, 0);
    private Vector3 _laserOffset = new Vector3(0, 1.05f, 0);
    [SerializeField]
    private float _fireRate = 0.15f;
    private float _canFire = -1f;
    [SerializeField]
    private int _lives = 3;
    [SerializeField]
    private GameObject _rightEngine;
    [SerializeField]
    private GameObject _leftEngine;
    private SpawnManager _spawnManager;

    [SerializeField]
    private int _score;

    private UIManager _uiManager; 
    
    private bool _tripleShotActive = false;
    private bool _speedBoostActive = true;
    private bool _playerShieldActive = false;
    

    void Start()
    {
        //take the current position = new position (0, 0, 0)
        transform.position = new Vector3(0, 0, 0);
        
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();

        if (_spawnManager == null)
        {
            Debug.Log("The Spawn Manager is NULL");
        }

        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();

        if (_uiManager == null)
        {
            Debug.LogError("The UI Manager is NULL");
        }
        
               
    }

    // Update is called once per frame
    void Update()
    {
        CalculateMovement();
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            fireLaser();
        }

    }
    void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 direction = new Vector3(horizontalInput, verticalInput, 0);
        
        
        transform.Translate(direction * _speed * Time.deltaTime);
        
        
        
        if (transform.position.y >= 0)
        {
            transform.position = new Vector3(transform.position.x, 0, 0);
        }
        else if (transform.position.y <= -3.8f)
        {
            transform.position = new Vector3(transform.position.x, -3.8f, 0);
        }

        if (transform.position.x >= 13)
        {
            transform.position = new Vector3(-13, transform.position.y, 0);
        }
        else if (transform.position.x <= -13)
        {
            transform.position = new Vector3(13, transform.position.y, 0);
        }
    }
    void fireLaser()
    {

        _canFire = Time.time + _fireRate;

        if (_tripleShotActive == true)
        {
            Instantiate(_tripleshotprefab, transform.position + _tripleshotOffset, Quaternion.identity);
        }
        else
        {
            Instantiate(_laserPrefab, transform.position + _laserOffset, Quaternion.identity);
        }
    }
    public void AddScore(int points)
    {
        _score += points;
        _uiManager.UpdateScore(_score);
    }

    public void ShieldActive()
    {
        _playerShieldActive = true;
        _shieldVisualizer.SetActive(true);
    }
    
    public void Damage()
    {
        if (_playerShieldActive == true)
        {
            _playerShieldActive = false;
            _shieldVisualizer.SetActive(false);
            return;
        }
        
        _lives--; //lives = _lives - 1
        _uiManager.UpdateLives(_lives);

        if (_lives == 2)
        {
            _rightEngine.SetActive(true);
        }

        if (_lives == 1)
        {
            _leftEngine.SetActive(true);
        }

        if (_lives < 1)
        {
            _spawnManager.OnPlayerDeath();
            Destroy(this.gameObject);
        }
    }
    
    public void tripleShotActivate()
    {
        _tripleShotActive = true;
        StartCoroutine(tripleShotPowerDownRoutine());
        //start the coroutine for the tripleshot to power down after so many seconds
    }
    IEnumerator tripleShotPowerDownRoutine()
    {
        while (_tripleShotActive == true)
        {
            yield return new WaitForSeconds(5.0f);
            _tripleShotActive = false;
        }
    }
    public void SpeedBoostActivate()
    {
        _speedBoostActive = true;
        _speed *= _speedboost;
        StartCoroutine(SpeedBoostCoolDownRoutine());        
    }
    IEnumerator SpeedBoostCoolDownRoutine()
    {
       yield return new WaitForSeconds(5.0f);
       _speedBoostActive = false;
       _speed /= _speedboost;
    }
 }
