using Content.Server.Mail.Components;
using Content.Server.Power.Components;
using Content.Shared.Examine;

namespace Content.Server.Mail;

/// <summary>
/// Handles spawning mail and showing time before next mail batch when examined.
/// </summary>
public sealed class MailTeleporterSystem : EntitySystem
{
    [Dependency] private readonly MailSystem _mail = default!;

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<MailTeleporterComponent, ExaminedEvent>(OnExamined);
    }

    private void OnExamined(EntityUid uid, MailTeleporterComponent comp, ExaminedEvent args)
    {
        if (!args.IsInDetailsRange)
            return;

        var time = Math.Ceiling(comp.TeleportInterval.TotalSeconds - comp.Accumulator);
        args.PushMarkup(Loc.GetString("mail-teleporter-desc", ("timeLeft", time)));
    }

    public override void Update(float frameTime)
    {
        base.Update(frameTime);

        var query = EntityQueryEnumerator<MailTeleporterComponent>();
        while (query.MoveNext(out var uid, out var comp))
        {
            if (TryComp<ApcPowerReceiverComponent>(uid, out var power) && !power.Powered)
                return;

            comp.Accumulator += frameTime;

            if (comp.Accumulator < comp.TeleportInterval.TotalSeconds)
                continue;

            comp.Accumulator -= (float) comp.TeleportInterval.TotalSeconds;

            _mail.SpawnMail(uid, comp);
        }
    }
}
