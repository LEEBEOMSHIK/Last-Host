using System.Collections.Generic;
using LastHost.Prototype.Core;
using UnityEngine;

namespace LastHost.Prototype.VirusMinigame
{
    public sealed class VirusMinigameController : MonoBehaviour
    {
        public PrototypeSessionController session;
        public VirusPlayerController virusPlayer;
        public WhiteBloodCellChaser[] whiteBloodCells;
        public MutationFragmentPickup[] mutationFragments;

        private readonly HashSet<MutationFragmentPickup> pendingFragments = new HashSet<MutationFragmentPickup>();
        private bool pendingWhiteBloodCellHit;
        private bool gameplayActive;

        private void Awake()
        {
            if (virusPlayer == null)
            {
                virusPlayer = GetComponentInChildren<VirusPlayerController>(true);
            }

            if (whiteBloodCells == null || whiteBloodCells.Length == 0)
            {
                whiteBloodCells = GetComponentsInChildren<WhiteBloodCellChaser>(true);
            }

            if (mutationFragments == null || mutationFragments.Length == 0)
            {
                mutationFragments = GetComponentsInChildren<MutationFragmentPickup>(true);
            }

            WireChildren();
        }

        private void LateUpdate()
        {
            if (!gameplayActive || session == null)
            {
                pendingFragments.Clear();
                pendingWhiteBloodCellHit = false;
                return;
            }

            if (pendingFragments.Count == 0 && !pendingWhiteBloodCellHit)
            {
                return;
            }

            var collectedFragment = pendingFragments.Count > 0;
            var outcome = session.ResolveVirusFrame(collectedFragment, pendingWhiteBloodCellHit);

            if (collectedFragment)
            {
                foreach (var fragment in pendingFragments)
                {
                    fragment.MarkCollected();
                }
            }

            pendingFragments.Clear();
            pendingWhiteBloodCellHit = false;

            if (outcome != VirusMinigameOutcome.Running)
            {
                SetGameplayActive(false);
            }
        }

        public void QueueFragmentCollected(MutationFragmentPickup fragment)
        {
            if (fragment != null && gameplayActive)
            {
                pendingFragments.Add(fragment);
            }
        }

        public void QueueWhiteBloodCellHit()
        {
            if (gameplayActive)
            {
                pendingWhiteBloodCellHit = true;
            }
        }

        public void ResetRun()
        {
            pendingFragments.Clear();
            pendingWhiteBloodCellHit = false;

            if (virusPlayer != null)
            {
                virusPlayer.ResetToStart();
            }

            foreach (var whiteBloodCell in whiteBloodCells)
            {
                whiteBloodCell.ResetAgent();
            }

            foreach (var fragment in mutationFragments)
            {
                fragment.ResetPickup();
            }
        }

        public void SetGameplayActive(bool active)
        {
            gameplayActive = active;

            if (virusPlayer != null)
            {
                virusPlayer.enabled = active;
            }

            foreach (var whiteBloodCell in whiteBloodCells)
            {
                whiteBloodCell.enabled = active;
            }
        }

        private void WireChildren()
        {
            foreach (var whiteBloodCell in whiteBloodCells)
            {
                whiteBloodCell.controller = this;
                if (virusPlayer != null)
                {
                    whiteBloodCell.target = virusPlayer.transform;
                }
            }

            foreach (var fragment in mutationFragments)
            {
                fragment.controller = this;
            }
        }
    }
}
