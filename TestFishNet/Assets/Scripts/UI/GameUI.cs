using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Seance.Networking;
using Seance.TurnSystem;
using TMPro;

public class GameUI : MonoBehaviour
{

    LobbyManager _lobby;
    [SerializeField] PlayerTurnState _playerState;

    [Space]

    //p1
    [SerializeField] GameObject _p1UI;
    [SerializeField] TextMeshProUGUI _marbleNbP1;
    [SerializeField] TextMeshProUGUI _nbMarbleBet;
    public int _bet;

    //p2
    [SerializeField] GameObject _p2UI;
    [SerializeField] TextMeshProUGUI _marbleNbP2;
    public bool _isOdd;

    void Start()
    {
        _lobby = LobbyManager.Instance;

        Init();
    }

    void Init()
    {
        _p1UI.SetActive(false);
        _marbleNbP1.text = "You have " + _playerState._nbMarblesP1 + " marbles";

        _p2UI.SetActive(false);
        _marbleNbP2.text = "You have " + _playerState._nbMarblesP2 + " marbles";
        _bet = 0;
    }

    //// P1 ////
    public void More()
    {
        if (_bet == _playerState._nbMarblesP1)
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
