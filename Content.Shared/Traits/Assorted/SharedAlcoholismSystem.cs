using Content.Shared.GameTicking;
using Content.Shared.Jittering;
using Content.Shared.StatusEffect;
using Content.Shared.Stunnable;
using Robust.Shared.GameStates;

namespace Content.Shared.Traits.Assorted;

public abstract class SharedAlcoholismSystem : EntitySystem
{
    [Dependency] private readonly StatusEffectsSystem _status = default!;
    [Dependency] private readonly SharedJitteringSystem _jitter = default!;


    public const string DrunkardKey = "Alcoholic";
    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<AlcoholicComponent, ComponentGetState>(GetCompState);
        SubscribeLocalEvent<AlcoholicComponent, ComponentHandleState>(HandleCompState);

        SubscribeLocalEvent<AlcoholJitterEvent>(HandleAlcoholJitter);
        SubscribeLocalEvent<AlcoholStunEvent>(HandleAlcoholStun);
    }

    private void GetCompState(EntityUid uid, AlcoholicComponent component, ref ComponentGetState args)
    {
        args.State = new AlcoholicComponentState(component.MinTimeBetweenIncidents, component.MaxTimeBetweenIncidents,
            component.DurationOfIncident, component.MinTimeBetweenDrinks, component.MaxTimeBetweenDrinks);
    }

    private void HandleCompState(EntityUid uid, AlcoholicComponent component, ref ComponentHandleState args)
    {
        if (args.Current is not AlcoholicComponentState state)
            return;

        component.MinTimeBetweenIncidents = state.MinTimeBetweenIncidents;
        component.MaxTimeBetweenIncidents = state.MaxTimeBetweenIncidents;
        component.MinTimeBetweenDrinks = state.MinTimeBetweenIncidents;
        component.MaxTimeBetweenDrinks = state.MaxTimeBetweenDrinks;
        component.DurationOfIncident = state.DurationOfIncident;
    }

    void HandleAlcoholJitter(AlcoholJitterEvent args)
    {
        _status.TryAddStatusEffect<JitteringComponent>
            (args.UID, "Jitter", TimeSpan.FromSeconds(5f * args.Severity), true);
        _jitter.AddJitter(args.UID, 10, 4);
        _status.TryAddStatusEffect<SlowedDownComponent>
            (args.UID, "SlowedDown", TimeSpan.FromSeconds(5f * args.Severity), true);
    }
    void HandleAlcoholStun(AlcoholStunEvent args)
    {
        _status.TryAddStatusEffect<JitteringComponent>
            (args.UID, "SlurredSpeech", TimeSpan.FromSeconds(4f * MathF.Min(args.Severity - 2, 20f)), true);

        _status.TryAddStatusEffect<JitteringComponent>
            (args.UID, "Jitter", TimeSpan.FromSeconds(3f * MathF.Min(args.Severity - 2, 15f)), true);
        _jitter.AddJitter(args.UID, 10, 4);
        _status.TryAddStatusEffect<SlowedDownComponent>
            (args.UID, "SlowedDown", TimeSpan.FromSeconds(3f * MathF.Min(args.Severity - 2, 15f)), true);

        _status.TryAddStatusEffect<KnockedDownComponent>
            (args.UID, "KnockedDown", TimeSpan.FromSeconds(2f * MathF.Min(args.Severity - 2, 10f)), false);
        _status.TryAddStatusEffect<StunnedComponent>
        (args.UID, "Stun", TimeSpan.FromSeconds(2f * MathF.Min(args.Severity - 2, 10f)), false);
    }

    public readonly struct AlcoholJitterEvent
    {
        public readonly int Severity;
        public readonly EntityUid UID;
        public AlcoholJitterEvent(int severity, EntityUid uid)
        {
            Severity = severity;
            UID = uid;
        }

    }
    public readonly struct AlcoholStunEvent
    {
        public readonly int Severity;
        public readonly EntityUid UID;
        public AlcoholStunEvent(int severity, EntityUid uid)
        {
            Severity = severity;
            UID = uid;
        }
    }
}
