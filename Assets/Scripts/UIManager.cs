using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    //create a handle to text
    [SerializeField]
    private TMP_Text _scoreText;
    
    [SerializeField]
    private int _score;

    [SerializeField]
    private Image _livesImg;

    [SerializeField]
    private Sprite[] _liveSprites;

    [SerializeField]
    private TMP_Text _gameOver;

    [SerializeField]
    private TMP_Text _restart;

    [SerializeField]
    private TMP_Text _esc;

    private GameManager _gameManager;

  
    
    // Start is called before the first frame update
    void Start()
    {
        _gameManager = GameObject.Find("Game_Manager").GetComponent<GameManager>();
       
        if(_gameManager == null)
        {
            Debug.LogError("The Game Manager is NULL");
        }

        _scoreText.text = "Score " + 0;
        _gameOver.gameObject.SetActive(false);
        _restart.gameObject.SetActive(false);
        _esc.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void UpdateScore(int playerScore)
    {
        _scoreText.text = "Score: " + playerScore.ToString();
    }

    public void UpdateLives(int currentLives)
    {
        //display img sprites
        //give it a new one based on the currentLives index
        _livesImg.sprite = _liveSprites[currentLives];
        if (currentLives == 0)
        {
            GameOverSequence();
        }
        
    }
    public void GameOverSequence()
    {
        _gameManager.GameOver();
        _gameOver.gameObject.SetActive(true);
        _restart.gameObject.SetActive(true);
        _esc.gameObject.SetActive(true);
        StartCoroutine(FlickerTime());
    }
    IEnumerator FlickerTime()
    {
        while (_gameOver == true)
        {
            _gameOver.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.5f);

            _gameOver.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.5f);
        }
    }
}
