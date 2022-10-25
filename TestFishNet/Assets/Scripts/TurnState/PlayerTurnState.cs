/// Author: Nicolas Capelier
/// Last modified by: Julien Haigron

//using Seance.Interactions;
using Seance.Networking;
using Seance.Utility;
//using Seance.Wayfarer;
using Seance.Interactions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Seance.TurnSystem
{
	public class PlayerTurnState : MonoState
	{
		TurnStateMachine _machine;
		LobbyManager _lobby;
		//WayfarerManager _wayfarer;

		[Header("Params")]
		[SerializeField] LayerMask _interactableLayer;

		[Header("References")]
		[SerializeField] AudioClip[] _knocksClips;
		int _lastKnockClip = -1;
		[SerializeField] AudioClip _miTurnClip;
		[SerializeField] AudioClip _fullTurnClip;

		//UI
		[SerializeField] GameUI _inGameUI;

		//CountdownTimer _miTurnTimer = new();
		//CountdownTimer _fullTurnTimer = new();

		private void Start()
		{
			_machine = TurnStateMachine.Instance;
			_lobby = LobbyManager.Instance;
			//_wayfarer = WayfarerManager.Instance;
		}

		public override void OnStateEnter()
		{
			if (!_machine.IsPlaying)
			{
				return;
			}

			//Debug.LogError("player : " + _lobby._ownedConnectionReferencePosition + " , num " + _machine._playerOrder);

			//activate UI
			if(_lobby._ownedConnectionReferencePosition == 0 && _machine._playerOrder == 0)
            {
				//Debug.LogError("1");
				_inGameUI.InitP1(_machine._nbMarblesP1);
            }
			else if(_lobby._ownedConnectionReferencePosition == 0 && _machine._playerOrder == 1)
            {
				//Debug.LogError("2");
				_inGameUI.InitP2(_machine._nbMarblesP1);
            }
			else if (_lobby._ownedConnectionReferencePosition == 1 && _machine._playerOrder == 0)
			{
				//Debug.LogError("3");
				_inGameUI.InitP2(_machine._nbMarblesP2);
			}
			else if (_lobby._ownedConnectionReferencePosition == 1 && _machine._playerOrder == 1)
			{
				//Debug.LogError("4");
				_inGameUI.InitP1(_machine._nbMarblesP2);
			}

		}

		public void SetPlayer2Bet(bool isOdd)
        {
			//Debug.LogError("click on bet button");
			_machine.ServerSetPlayer2Bet(isOdd);
        }

		public void SetNumberMarblesBetted(int nbMarblesBetted)
        {
			_machine.ServerSetNumberMarblesBetted(nbMarblesBetted);
		}

		public void OnClick(InputAction.CallbackContext context)
		{
			
			if (!context.started)
				return;

			RaycastHit hit;

			Ray ray = _lobby._ownedPlayerInstance.CameraController.Camera.ScreenPointToRay(Input.mousePosition);

			Debug.DrawRay(ray.origin, ray.direction * 50f, Color.red, .8f);

			if (Physics.Raycast(ray, out hit, 50f, _interactableLayer))
			{
				Interactor interactor;

				if (hit.transform.TryGetComponent(out interactor))
				{
					interactor.Interact(this);
				}
			}
			
		}

		public void EndTurn()
		{
			//Debug.LogError("end turn");
			if (!_machine.IsPlaying)
				return;

			//_miTurnTimer.Cancel();
			//_fullTurnTimer.Cancel();

			_machine.ServerGetResult();
		}

		public void PlayerKnocks()
		{
			int clip = Random.Range(0, _knocksClips.Length);

			while (_lastKnockClip == clip)
			{
				clip = Random.Range(0, _knocksClips.Length);
			}
			_lastKnockClip = clip;

			//AudioManager.Instance.PlayEffectOnTmpSource(_knocksClips[clip]);
			_machine.ServerSetWayfarerTarget(_lobby._ownedConnectionReferencePosition, false);
		}
	}
}
