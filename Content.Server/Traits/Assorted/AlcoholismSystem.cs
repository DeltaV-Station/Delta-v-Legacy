using Content.Server.Camera;
using Content.Shared.Jittering;
using Content.Shared.StatusEffect;
using Content.Shared.Stunnable;
using Content.Shared.Traits.Assorted;
using Microsoft.Extensions.DependencyModel;
using Robust.Server.Player;
using Robust.Shared.Random;
using Robust.Shared.Timing;

namespace Content.Server.Traits.Assorted;

public sealed class AlcoholismSystem : SharedAlcoholismSystem
{
    [Dependency] private readonly IGameTiming _timing = default!;
    [Dependency] private readonly IPlayerManager _player = default!;
    [Dependency] private readonly IRobustRandom _random = default!;

    public const string DrunkardKey = "Alcoholic";

    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<AlcoholicComponent, ComponentStartup>(OnComponentStartup);
    }

    public override void Update(float frameTime)
    {
        base.Update(frameTime);

        if (!_timing.IsFirstTimePredicted)
            return;
        foreach(var player in _player.GetAllPlayerData())
        {
            if (_player.GetSessionByUserId(player.UserId).AttachedEntity is not EntityUid localPlayer)
                continue;
            IncrementTime(localPlayer, frameTime);
        }
    }

    private void OnComponentStartup(EntityUid uid, AlcoholicComponent component, ComponentStartup args)
    {
        component.NextIncidentTime = _timing.CurTime +
            TimeSpan.FromSeconds(_random.NextFloat(component.MinTimeBetweenDrinks, component.MaxTimeBetweenDrinks));
    }

    private void IncrementTime(EntityUid uid, float frameTime)
    {
        if (!TryComp<AlcoholicComponent>(uid, out var alc))
            return;
        if (_timing.CurTime <= alc.NextIncidentTime)
            return;

        // Set the new time.
        var timeInterval = _random.NextFloat(alc.MinTimeBetweenIncidents, alc.MaxTimeBetweenIncidents);
        alc.NextIncidentTime += TimeSpan.FromSeconds(timeInterval);

        //perform a withdrawal incident
        if (alc.IncidentSeverityCounter < 3)
            RaiseLocalEvent<AlcoholJitterEvent>(new(alc.IncidentSeverityCounter, uid));
        else
            RaiseLocalEvent<AlcoholStunEvent>(new(alc.IncidentSeverityCounter, uid));
        alc.IncidentSeverityCounter++;
    }
}
