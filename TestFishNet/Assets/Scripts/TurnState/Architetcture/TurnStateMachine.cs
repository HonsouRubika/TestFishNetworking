/// Author: Nicolas Capelier
/// Last modified by: Nicolas Capelier

using FishNet.Object;
//using Seance.CardSystem;
using Seance.Networking;
using Seance.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Seance.Wayfarer;
//using Seance.Interactions;

namespace Seance.TurnSystem
{
	public class TurnStateMachine : MonoStateMachine
	{
		[Header("Params")]
		//public StartingDeck[] _startingDecks;
		[Space]

		//
		public int _nbMarblesP1 = 10;
		public int _nbMarblesP2 = 10;
		//

		[HideInInspector] public bool _gameStarted = false;
		int _activePlayer = -1;
		public int ActivePlayer { get { return _activePlayer; } }

		bool _isPlaying;
		public bool IsPlaying { get { return _isPlaying; } }

		[Header("References")]
		[SerializeField] PlayerTurnState _playerTurn;
		public PlayerTurnState PlayerTurn { get { return _playerTurn; } }

		//[HideInInspector] public int[] _chapterTurnOrder = new int[4];

		#region Singleton

		public static TurnStateMachine Instance;

		private void Awake()
		{
			Instance = this;
		}

		#endregion


		[ServerRpc(RequireOwnership = false)]
		public void ServerPlayNextTurn()
		{
			if (!IsServer)
				return;

			_activePlayer++;
			if (_activePlayer > 1)
			{
				_activePlayer = 0;
				//ObserversDecreaseAllPlayersDiceValue();
			}

			ObserversPlayNextTurn(_activePlayer);
		}

		[ObserversRpc]
		public void ObserversPlayNextTurn(int nextPlayer)
		{
			if (IsPlaying)
			{
				//Apply end of turn effects if needed
			}

			_isPlaying = false;
			_activePlayer = nextPlayer;

			if (LobbyManager.Instance._ownedConnectionReferencePosition == _activePlayer)
				_isPlaying = true;

			//WayfarerManager.Instance.MoveToPosition(nextPlayer, true);

			//SetState("PlayerTurn", true);
		}

		[ObserversRpc]
		public void ObserversDecreaseAllPlayersDiceValue()
		{
			//Dice20.Instance.DecreaseDiceValue();
		}

		[ServerRpc(RequireOwnership = false)]
		public void ServerSetWayfarerTarget(int playerIndex, bool punish)
		{
			ObserversSetWayfarerTarget(playerIndex);
			//if(punish)
				//WayfarerManager.Instance.TargetPunishPlayer(LobbyManager.Instance._connections[playerIndex]);
		}

		[ObserversRpc]
		public void ObserversSetWayfarerTarget(int playerIndex)
		{
			//WayfarerManager.Instance.MoveToPosition(playerIndex);
		}

		/// Turn order system with random first player

		//public void SetChapterTurnOrder(int firstPlayerIndex)
		//{
		//	_chapterTurnOrder[0] = firstPlayerIndex;
		//	for (int i = 1; i < 3; i++)
		//	{
		//		firstPlayerIndex++;
		//		if (firstPlayerIndex > 2)
		//			firstPlayerIndex = 0;

		//		_chapterTurnOrder[i] = firstPlayerIndex;
		//	}

		//	_chapterTurnOrder[3] = 3;
		//	_activePlayer = 0;
		//}

		//public void StartPlayerTurn(int playerIndex)
		//{
		//	if (_chapterTurnOrder[_activePlayer] != 3)
		//	{
		//		//send new turn command to player _chapterTurnOrder[_activePlayer]
		//	}
		//	else
		//	{
		//		//start wayfarer turn
		//	}
		//}

		//public void StartNextPlayerTurn()
		//{
		//	_activePlayer++;
		//	if (_activePlayer > 3)
		//	{
		//		_activePlayer = 0;
		//	}

		//	StartPlayerTurn(_activePlayer);
		//}


	}
}
