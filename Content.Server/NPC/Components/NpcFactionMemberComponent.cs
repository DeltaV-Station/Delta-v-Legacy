using Content.Server.NPC.Systems;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom.Prototype.Set;

namespace Content.Server.NPC.Components
{
    [RegisterComponent]
    [Access(typeof(NpcFactionSystem))]
    public sealed class NpcFactionMemberComponent : Component
    {
        /// <summary>
        /// Factions this entity is a part of.
        /// </summary>
        [ViewVariables(VVAccess.ReadWrite),
         DataField("factions", customTypeSerializer:typeof(PrototypeIdHashSetSerializer<NpcFactionPrototype>))]
        public HashSet<string> Factions = new();

        /// <summary>
        /// Cached friendly factions.
        /// </summary>
        [ViewVariables]
        public readonly HashSet<string> FriendlyFactions = new();

        /// <summary>
        /// Cached hostile factions.
        /// </summary>
        [ViewVariables]
        public readonly HashSet<string> HostileFactions = new();

        // Begin Nyano-code: support for specific entities to be friendly.
        /// <summary>
        /// Permanently friendly specific entities. Our summoner, etc.
        /// </summary>
        public HashSet<EntityUid> ExceptionalFriendlies = new();
        // End Nyano-code.
    }
}
