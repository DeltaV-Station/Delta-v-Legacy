using Content.Server.Speech.Components;
using Content.Shared.Speech.EntitySystems;
using Robust.Shared.Random;

namespace Content.Server.Speech.EntitySystems
{
    public sealed class GermanAccentSystem : GermanAccentSharedSystem
    {
        //[Dependency] private readonly IRobustRandom _random = default!;
        private const string GermanKey = "German";

        private static readonly IReadOnlyDictionary<string, string> SpecialWords = new Dictionary<string, string>()
        {
            { "yeah",       "ja"                },
            { "yes",        "ja"                },
            { "thank you",  "danke"             },
            { "thanks",     "danke"             },
            { "shit",       "sheiße"            },
            { "damned",     "verdammt"          },
            { "fucking",    "verdammt"          },
            { "fucked",     "gefickt"           },
            { "hello",      "guten tag"         },
            { "bye",        "tschüss"           },
            { "goodbye",    "auf viedersehen"   },
            { "hell",       "hölle"             },
            { "please",     "bitte"             },
            { "true",       "stimmt"            },
            { "my god",     "mein gott"         },
        };

        public override void Initialize()
        {
            SubscribeLocalEvent<GermanAccentComponent, AccentGetEvent>(OnAccent);
        }

        public string Accentuate(string message)
        {
            foreach (var (word, repl) in SpecialWords)
            {
                message = message.Replace(word, repl);
            }

            return message
                .Replace("f", "v").Replace("F", "V")
                .Replace("w", "v").Replace("W", "V")
                .Replace("th", "z").Replace("TH", "Z");
        }

        private void OnAccent(EntityUid uid, GermanAccentComponent component, AccentGetEvent args)
        {
            args.Message = Accentuate(args.Message);
        }
    }
}
