using System.Text;
using System.Text.RegularExpressions;
using Content.Server.Speech.Components;
using Content.Shared.Speech.EntitySystems;
using Content.Shared.StatusEffect;
using Robust.Shared.Random;

namespace Content.Server.Speech.EntitySystems
{
    public sealed class MinorStutterSystem : SharedMinorStutterSystem
    {
        [Dependency] private readonly StatusEffectsSystem _statusEffectsSystem = default!;
        [Dependency] private readonly IRobustRandom _random = default!;

        private const string StutterKey = "MinorStutter";

        // Regex of characters to stutter.
        private static readonly Regex Stutter = new(@"\b[a-zA-Z]",
            RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public override void Initialize()
        {
            SubscribeLocalEvent<MinorStutterAccentComponent, AccentGetEvent>(OnAccent);
        }

        public override void DoStutter(EntityUid uid, TimeSpan time, bool refresh, StatusEffectsComponent? status = null)
        {
            if (!Resolve(uid, ref status, false))
                return;

            _statusEffectsSystem.TryAddStatusEffect<MinorStutterAccentComponent>(uid, StutterKey, time, refresh, status);
        }

        private void OnAccent(EntityUid uid, MinorStutterAccentComponent component, AccentGetEvent args)
        {
            args.Message = Accentuate(args.Message);
        }

        public string Accentuate(string message)
        {
            var length = message.Length;

            var finalMessage = new StringBuilder();

            string newLetter;

            for (var i = 0; i < length; i++)
            {
                newLetter = message[i].ToString();
                if (Stutter.IsMatch(newLetter) && (i == 0 || message[i - 1].ToString() == " "))
                {
                    if (_random.Prob(0.5f))
                    {
                        newLetter = $"{newLetter}-{newLetter}";
                    }
                    else if (_random.Prob(0.5f))
                    {
                        newLetter = $"{newLetter}-{newLetter}-{newLetter}";
                    }
                    else
                    {
                        newLetter = $"{newLetter}";
                    }
                }

                finalMessage.Append(newLetter);
            }

            return finalMessage.ToString();
        }
    }
}
