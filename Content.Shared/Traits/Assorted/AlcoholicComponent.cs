using Content.Shared.Drunk;
using Robust.Shared.Serialization;
using Robust.Shared.Serialization.TypeSerializers.Implementations.Custom;

namespace Content.Shared.Traits.Assorted;

/// <summary>
/// This is used for the alcoholism (alcohol dependency) trait.
/// </summary>
[RegisterComponent, Access(typeof(SharedAlcoholismSystem), typeof(SharedDrunkSystem))]
public sealed class AlcoholicComponent : Component
{
    /// <summary>
    /// The maximum time between withdrawal incidents in seconds
    /// </summary>
    [DataField("maxTimeBetweenIncidents", required: true), ViewVariables(VVAccess.ReadWrite)]
    public float MaxTimeBetweenIncidents = 60f;

    /// <summary>
    /// The minimum time between withdrawal incidents in seconds
    /// </summary>
    [DataField("minTimeBetweenIncidents", required: true), ViewVariables(VVAccess.ReadWrite)]
    public float MinTimeBetweenIncidents = 20f;

    /// <summary>
    /// The maximum time between needed drinks in seconds
    /// </summary>
    [DataField("maxTimeBetweenDrinks", required: true), ViewVariables(VVAccess.ReadWrite)]
    public float MaxTimeBetweenDrinks = 300f;

    /// <summary>
    /// The minimum time between needed drinks in seconds
    /// </summary>
    [DataField("minTimeBetweenDrinks", required: true), ViewVariables(VVAccess.ReadWrite)]
    public float MinTimeBetweenDrinks = 180f;

    /// <summary>
    /// The number of times withdrawl incidents have occured
    /// </summary>
    [DataField("incidentSeverity", required: false), ViewVariables(VVAccess.ReadWrite)]
    public int IncidentSeverityCounter = 0;

    /// <summary>
    /// The duration of withdrawal incidents in seconds
    /// </summary>
    [DataField("durationOfIncident", required: true), ViewVariables(VVAccess.ReadWrite)]
    public float DurationOfIncident;

    /// <summary>
    /// When the next withdrawal incident will occur, in seconds
    /// </summary>
    [DataField("timeBetweenIncidents", customTypeSerializer: typeof(TimeOffsetSerializer)), ViewVariables(VVAccess.ReadWrite)]
    public TimeSpan NextIncidentTime;
}

[Serializable, NetSerializable]
public sealed class AlcoholicComponentState : ComponentState
{
    public readonly float MinTimeBetweenIncidents;
    public readonly float MaxTimeBetweenIncidents;
    public readonly float MinTimeBetweenDrinks;
    public readonly float MaxTimeBetweenDrinks;
    public readonly float DurationOfIncident;

    public AlcoholicComponentState(float minTimeBetweenIncidents, float maxTimeBetweenIncidents,
        float minTimeBetweenDrinks, float maxTimeBetweenDrinks, float durationOfIncident)
    {
        MinTimeBetweenIncidents = minTimeBetweenIncidents;
        MaxTimeBetweenIncidents = maxTimeBetweenIncidents;
        MinTimeBetweenDrinks = minTimeBetweenDrinks;
        MaxTimeBetweenDrinks = maxTimeBetweenDrinks;
        DurationOfIncident = durationOfIncident;
    }
}

