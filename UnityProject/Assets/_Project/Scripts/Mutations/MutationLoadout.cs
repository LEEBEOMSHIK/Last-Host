using System.Collections.Generic;

namespace LastHost.Prototype.Mutations
{
    public sealed class MutationLoadout
    {
        private readonly HashSet<MutationType> appliedMutations = new HashSet<MutationType>();

        public float ImmuneAlertRateMultiplier => Has(MutationType.Dormancy) ? 0.55f : 1f;
        public float RatSpeedMultiplier => Has(MutationType.NeuralControl) ? 1.35f : 1f;
        public bool CanUseMammalPassage => Has(MutationType.MammalAdaptation);

        public bool Has(MutationType type)
        {
            return appliedMutations.Contains(type);
        }

        public void Apply(MutationType type)
        {
            appliedMutations.Add(type);
        }
    }
}
