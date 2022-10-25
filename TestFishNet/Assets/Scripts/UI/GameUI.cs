using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Seance.Networking;
using Seance.TurnSystem;
using TMPro;

public class GameUI : MonoBehaviour
{
    LobbyManager _lobby;
    TurnStateMachine _machine;

    [SerializeField] PlayerTurnState _playerState;

    [Space]

    //p1
    [SerializeField] GameObject _p1UI;
    [SerializeField] TextMeshProUGUI _marbleNbP1;
    [SerializeField] TextMeshProUGUI _nbMarbleBet;
    int _currentNbMarblesP1 = -1;
    public int _bet;

    //p2
    [SerializeField] GameObject _p2UI;
    [SerializeField] TextMeshProUGUI _marbleNbP2;
    public bool _isOdd;
    int _currentNbMarblesP2 = -1;

    //final ui
    [SerializeField] GameObject _win;
    [SerializeField] GameObject _lose;

    void Start()
    {
        _lobby = LobbyManager.Instance;
        _machine = TurnStateMachine.Instance;

        Init();
    }

    void Init()
    {
        _p1UI.SetActive(false);
        _marbleNbP1.text = "You have " + _lobby._nbMarblesP1 + " marbles";

        _p2UI.SetActive(false);
        _marbleNbP2.text = "You have " + _lobby._nbMarblesP2 + " marbles";
        
        _bet = 0;
        _isOdd = false;
    }

    public void InitP1(int nbMarblesP1)
    {
        //Debug.LogError("init p1");
        _p1UI.SetActive(true);
        _p2UI.SetActive(false);
        _currentNbMarblesP1 = nbMarblesP1;
        _marbleNbP1.text = "You have " + _currentNbMarblesP1 + " marbles";
        _bet = 0;

        CheckIfEndGame();
    }

    public void InitP2(int nbMarblesP2)
    {
        //Debug.LogError("init p2");
        _p1UI.SetActive(false);
        _p2UI.SetActive(true);
        _currentNbMarblesP2 = nbMarblesP2;
        _marbleNbP2.text = "You have " + _currentNbMarblesP2 + " marbles";
        _isOdd = false;

        CheckIfEndGame();
    }

    public void CheckIfEndGame()
    {
        if((_lobby._ownedConnectionReferencePosition == 0 && _currentNbMarblesP1 == 20) || (_lobby._ownedConnectionReferencePosition == 1 && _currentNbMarblesP2 == 20))
        {
            _win.SetActive(true);
            _p1UI.SetActive(false);
            _p2UI.SetActive(false);
        }
        else if ((_lobby._ownedConnectionReferencePosition == 1 && _currentNbMarblesP2 == 20) || (_lobby._ownedConnectionReferencePosition == 0 && _currentNbMarblesP1 == 20))
        {
            _lose.SetActive(true);
            _p1UI.SetActive(false);
            _p2UI.SetActive(false);
        }
    }

    //// P1 ////
    public void More()
    {
        if (_bet == _currentNbMarblesP1)
            return;

        _nbMarbleBet.text = "Bet : " + ++_bet;
    }

    public void Less()
    {
        if (_bet == 0)
            return;

        _nbMarbleBet.text = "Bet : " + --_bet;
    }

    public void ValidateP1()
    {
        if (_bet == 0)
            return;

        _playerState.SetNumberMarblesBetted(_bet);
    }

    //// P2 ////
    public void Odd()
    {
        _isOdd = true;
        _playerState.SetPlayer2Bet(_isOdd);
    }

    public void Even()
    {
        _isOdd = false;
        _playerState.SetPlayer2Bet(_isOdd);
    }

}
