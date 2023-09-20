using Robust.Shared.Prototypes;

namespace Content.Server.Roboisseur.Roboisseur
{
    [RegisterComponent]
    public sealed class RoboisseurComponent : Component
    {

        [ViewVariables]
        [DataField("accumulator")]
        public float Accumulator = 0f;

        [ViewVariables(VVAccess.ReadOnly)]
        [DataField("impatient")]
        public Boolean Impatient { get; set; } = false;

        [ViewVariables]
        [DataField("resetTime")]
        public TimeSpan ResetTime = TimeSpan.FromMinutes(10);

        [DataField("barkAccumulator")]
        public float BarkAccumulator = 0f;

        [DataField("barkTime")]
        public TimeSpan BarkTime = TimeSpan.FromMinutes(1);

        [ViewVariables(VVAccess.ReadWrite)]
        public EntityPrototype DesiredPrototype = default!;
    }
}