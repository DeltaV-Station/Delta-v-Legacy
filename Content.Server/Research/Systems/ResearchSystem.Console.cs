using Content.Server.Power.EntitySystems;
using Content.Server.Research.Components;
using Content.Server.UserInterface;
using Content.Shared.Access.Components;
using Content.Shared.Research.Components;

namespace Content.Server.Research.Systems;

public sealed partial class ResearchSystem
{
    private void InitializeConsole()
    {
        SubscribeLocalEvent<ResearchConsoleComponent, ConsoleUnlockTechnologyMessage>(OnConsoleUnlock);
        SubscribeLocalEvent<ResearchConsoleComponent, BeforeActivatableUIOpenEvent>(OnConsoleBeforeUiOpened);
        SubscribeLocalEvent<ResearchConsoleComponent, ResearchServerPointsChangedEvent>(OnPointsChanged);
        SubscribeLocalEvent<ResearchConsoleComponent, ResearchRegistrationChangedEvent>(OnConsoleRegistrationChanged);
        SubscribeLocalEvent<ResearchConsoleComponent, TechnologyDatabaseModifiedEvent>(OnConsoleDatabaseModified);
    }

    private void OnConsoleUnlock(EntityUid uid, ResearchConsoleComponent component, ConsoleUnlockTechnologyMessage args)
    {
        if (args.Session.AttachedEntity is not { } ent)
            return;

        if (!this.IsPowered(uid, EntityManager))
            return;

        if (TryComp<AccessReaderComponent>(uid, out var access) && !_accessReader.IsAllowed(ent, access))
        {
            _popup.PopupEntity(Loc.GetString("research-console-no-access-popup"), ent);
            return;
        }

        if (!UnlockTechnology(uid, args.Id, ent))
            return;

        SyncClientWithServer(uid);
        UpdateConsoleInterface(uid, component);
    }

    private void OnConsoleBeforeUiOpened(EntityUid uid, ResearchConsoleComponent component, BeforeActivatableUIOpenEvent args)
    {
        SyncClientWithServer(uid);
    }

    private void UpdateConsoleInterface(EntityUid uid, ResearchConsoleComponent? component = null, ResearchClientComponent? clientComponent = null)
    {
        if (!Resolve(uid, ref component, ref clientComponent, false))
            return;

        ResearchConsoleBoundInterfaceState state;

        if (TryGetClientServer(uid, out var server, out var serverComponent, clientComponent) &&
            clientComponent.ConnectedToServer)
        {
            // Begin Nyano-code: limit passive point generation.
            var points = serverComponent.Points;
            var pointsPerSecond = GetPointsPerSecond(server.Value, serverComponent);
            var pointsLimit = serverComponent.PassiveLimitPerSource * serverComponent.PointSourcesLastUpdate;
            state = new ResearchConsoleBoundInterfaceState(points, pointsPerSecond, pointsLimit);
            // End Nyano-code.
        }
        else
        {
            state = new ResearchConsoleBoundInterfaceState(default, default, default);
        }

        _uiSystem.TrySetUiState(uid, ResearchConsoleUiKey.Key, state);
    }

    private void OnPointsChanged(EntityUid uid, ResearchConsoleComponent component, ref ResearchServerPointsChangedEvent args)
    {
        if (!_uiSystem.IsUiOpen(uid, ResearchConsoleUiKey.Key))
            return;
        UpdateConsoleInterface(uid, component);
    }

    private void OnConsoleRegistrationChanged(EntityUid uid, ResearchConsoleComponent component, ref ResearchRegistrationChangedEvent args)
    {
        SyncClientWithServer(uid);
        UpdateConsoleInterface(uid, component);
    }

    private void OnConsoleDatabaseModified(EntityUid uid, ResearchConsoleComponent component, ref TechnologyDatabaseModifiedEvent args)
    {
        UpdateConsoleInterface(uid, component);
    }
}
