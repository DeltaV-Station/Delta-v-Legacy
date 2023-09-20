using Content.Shared.Interaction;
using Content.Shared.Mobs.Components;
using Content.Server.Chat.Systems;
using Robust.Shared.Prototypes;
using Robust.Shared.Random;
using Content.Shared.Random.Helpers;
using Content.Shared.Kitchen;
using Robust.Server.GameObjects;
using Content.Server.Materials;

namespace Content.Server.Roboisseur.Roboisseur
{
    public enum RobossieurVisualLayers : byte
    {
        Angry
    }
    public sealed partial class RoboisseurSystem : EntitySystem
    {
        [Dependency] private readonly IPrototypeManager _prototypeManager = default!;
        [Dependency] private readonly IRobustRandom _random = default!;
        [Dependency] private readonly ChatSystem _chat = default!;
        [Dependency] private readonly MaterialStorageSystem _material = default!;
        [Dependency] private readonly AppearanceSystem _appearance = default!;
        private readonly ISawmill _sawmill = default!;


        [ViewVariables(VVAccess.ReadWrite)]
        public readonly IReadOnlyList<string> DemandMessages = new[]
        {
            "roboisseur-request-1",
            "roboisseur-request-2",
            "roboisseur-request-3",
            "roboisseur-request-4",
            "roboisseur-request-5",
            "roboisseur-request-6"
        };
        public readonly IReadOnlyList<string> ImpatientMessages = new[]
        {
            "roboisseur-request-impatient-1",
            "roboisseur-request-impatient-2",
            "roboisseur-request-impatient-3",
        };
        public readonly IReadOnlyList<string> DemandMessagesTier2 = new[]
        {
            "roboisseur-request-second-1",
            "roboisseur-request-second-2",
            "roboisseur-request-second-3"
        };

        public readonly IReadOnlyList<String> RewardMessages = new[]
        {
            "roboisseur-thanks-1",
            "roboisseur-thanks-2",
            "roboisseur-thanks-3",
            "roboisseur-thanks-4",
            "roboisseur-thanks-5"
        };
        public readonly IReadOnlyList<String> RewardMessagesTier2 = new[]
        {
            "roboisseur-thanks-second-1",
            "roboisseur-thanks-second-2",
            "roboisseur-thanks-second-3",
            "roboisseur-thanks-second-4",
            "roboisseur-thanks-second-5"
        };
        public readonly IReadOnlyList<String> RejectMessages = new[]
        {
            "roboisseur-deny-1",
            "roboisseur-deny-2",
            "roboisseur-deny-3"
        };

        private List<String> _tier2Protos = new()
        {
            "FoodBurgerEmpowered",
            "FoodSoupClown",
            "FoodSoupChiliClown",
            "FoodBurgerSuper",
            "FoodNoodlesCopy",
            "FoodMothMallow",
            "FoodPizzaCorncob",
            "FoodPizzDonkpocket",
            "FoodSoupMonkey",
            "FoodMothSeedSoup",
            "FoodTartGrape",
            "FoodMealCubancarp",
            "FoodMealSashimi",
            "FoodBurgerCarp",
            "FoodMealTaco",
            "FoodMothMacBalls",
            "FoodSoupNettle",
            "FoodBurgerDuck",
            "FoodBurgerBaseball"
        };

        private List<String> _tier3Protos = new()
        {
            "FoodBurgerGhost",
            "FoodSaladWatermelonFruitBowl",
            "FoodBakedCannabisBrownieBatch",
            "FoodPizzaDank",
            "FoodBurgerBear",
            "FoodBurgerMime",
            "FoodCakeSuppermatter",
            "FoodSoupChiliCold",
            "FoodSoupBisque",
            "FoodCakeSlime",
            "FoodBurgerCrazy"
        };

        public readonly IReadOnlyList<String> RobossuierRewards = new[]
        {
            "DrinkIceCreamGlass",
            "FoodFrozenPopsicleOrange",
            "FoodFrozenPopsicleBerry",
            "FoodFrozenPopsicleJumbo",
            "FoodFrozenSnowconeBerry",
            "FoodFrozenSnowconeFruit",
            "FoodFrozenSnowconeClown",
            "FoodFrozenSnowconeMime",
            "FoodFrozenSnowconeRainbow",
            "FoodFrozenCornuto",
            "FoodFrozenSundae",
            "FoodFrozenFreezy",
            "FoodFrozenSandwichStrawberry",
            "FoodFrozenSandwich",
        };

        public readonly IReadOnlyList<String> BlacklistedProtos = new[]
        {
            "FoodMothPesto",
            "FoodBurgerSpell",
            "FoodBreadBanana",
            "FoodMothSqueakingFry",
            "FoodMothFleetSalad",
            "FoodBreadMeatSpider",
            "FoodBurgerHuman",
            "FoodNoodlesBoiled",
            "FoodMothOatStew",
            "FoodMeatLizardtailKebab",
            "FoodSoupTomato",
            "FoodDonkpocketGondolaWarm",
            "FoodDonkpocketBerryWarm",
            "LockboxDecloner",
            "FoodBreadButteredToast",
            "FoodMothCottonSoup",
            "LeavesTobaccoDried",
            "FoodSoupEyeball",
            "FoodMothKachumbariSalad",
            "FoodMeatHumanKebab",
            "FoodMeatRatdoubleKebab",
            "FoodBurgerCorgi",
            "FoodBreadPlain",
            "FoodMeatKebab",
            "FoodBreadBun",
            "FoodBurgerCat",
            "FoodSoupTomatoBlood",
            "FoodMothSaladBase",
            "FoodPieXeno",
            "FoodDonkpocketTeriyakiWarm",
            "FoodMothBakedCheese",
            "FoodMothTomatoSauce",
            "FoodMothPizzaCotton",
            "AloeCream",
            "FoodSnackPopcorn",
            "FoodBurgerSoy",
            "FoodMothToastedSeeds",
            "FoodMothCornmealPorridge",
            "FoodMothBakedCorn",
            "FoodBreadMoldySlice",
            "FoodRiceBoiled",
            "FoodMothEyeballSoup",
            "FoodMeatRatKebab",
            "FoodBreadCreamcheese",
            "FoodSoupOnion",
            "FoodBurgerAppendix",
            "FoodBurgerRat",
            "RegenerativeMesh",
            "FoodCheeseCurds",
            "FoodDonkpocketHonkWarm",
            "FoodOatmeal",
            "FoodBreadJellySlice",
            "FoodMothCottonSalad",
            "FoodBreadMoldy",
            "FoodDonkpocketSpicyWarm",
            "FoodCannabisButter",
            "FoodNoodles",
            "FoodBreadMeat",
            "LeavesCannabisDried",
            "FoodBurgerCheese",
            "FoodDonkpocketDankWarm",
            "FoodSpaceshroomCooked",
            "FoodMealFries",
            "MedicatedSuture",
            "FoodDonkpocketWarm",
            "FoodCakePlain",
            "DisgustingSweptSoup",
            "FoodBurgerPlain",
            "FoodBreadGarlicSlice",
            "FoodSoupMushroom",
            "FoodSoupWingFangChu",
            "FoodBreadMeatXeno",
            "FoodCakeBrain",
            "FoodBurgerBrain",
            "FoodSaladCaesar"
        };
        public override void Initialize()
        {
            base.Initialize();
            SubscribeLocalEvent<RoboisseurComponent, ComponentInit>(OnInit);
            SubscribeLocalEvent<RoboisseurComponent, InteractHandEvent>(OnInteractHand);
            SubscribeLocalEvent<RoboisseurComponent, InteractUsingEvent>(OnInteractUsing);
        }
        private void OnInit(EntityUid uid, RoboisseurComponent component, ComponentInit args)
        {
            NextItem(component);
        }

        public override void Update(float frameTime)
        {
            base.Update(frameTime);
            foreach (var roboisseur in EntityQuery<RoboisseurComponent>())
            {
                roboisseur.Accumulator += frameTime;
                roboisseur.BarkAccumulator += frameTime;
                if (roboisseur.BarkAccumulator >= roboisseur.BarkTime.TotalSeconds)
                {
                    roboisseur.BarkAccumulator = 0;
                    string message = Loc.GetString(_random.Pick(DemandMessages), ("item", roboisseur.DesiredPrototype.Name));
                    if (roboisseur.ResetTime.TotalSeconds - roboisseur.Accumulator < 120)
                    {
                        roboisseur.Impatient = true;
                        message = Loc.GetString(_random.Pick(ImpatientMessages), ("item", roboisseur.DesiredPrototype.Name));
                    }
                    else if (CheckTier(roboisseur.DesiredPrototype.ID) > 2)
                        message = Loc.GetString(_random.Pick(DemandMessagesTier2), ("item", roboisseur.DesiredPrototype.Name));
                    _chat.TrySendInGameICMessage(roboisseur.Owner, message, InGameICChatType.Speak, false);
                }

                if (roboisseur.Accumulator >= roboisseur.ResetTime.TotalSeconds)
                {
                    roboisseur.Impatient = false;
                    NextItem(roboisseur);
                }
            }
        }

        private void RewardServicer(EntityUid uid, RoboisseurComponent component, int tier)
        {
            Random r = new Random();
            int rewardToDispense = r.Next(100, 350) + 250 * tier;

            _material.SpawnMultipleFromMaterial(rewardToDispense, "Credit", Transform(uid).Coordinates);
            if(tier > 1){
                while (tier != 0)
                {
                    EntityManager.SpawnEntity(_random.Pick(RobossuierRewards), Transform(uid).Coordinates);
                    tier--;
                }
            }
            return;
        }

        private void OnInteractHand(EntityUid uid, RoboisseurComponent component, InteractHandEvent args)
        {
            if (!TryComp<ActorComponent>(args.User, out var actor))
                return;
            string message = Loc.GetString(_random.Pick(DemandMessages), ("item", component.DesiredPrototype.Name));
            if (CheckTier(component.DesiredPrototype.ID) > 1)
                message = Loc.GetString(_random.Pick(DemandMessagesTier2), ("item", component.DesiredPrototype.Name));
            _chat.TrySendInGameICMessage(component.Owner, message, InGameICChatType.Speak, false);
        }

        private void OnInteractUsing(EntityUid uid, RoboisseurComponent component, InteractUsingEvent args)
        {
            if (HasComp<MobStateComponent>(args.Used))
                return;

            if (!TryComp<MetaDataComponent>(args.Used, out var meta))
                return;

            if (meta.EntityPrototype == null)
                return;

            var validItem = CheckValidity(meta.EntityPrototype, component.DesiredPrototype);

            var nextItem = true;

            if (!validItem)
            {
                _chat.TrySendInGameICMessage(uid, Loc.GetString(_random.Pick(RejectMessages)), InGameICChatType.Speak, true);
                return;
            }
            component.Impatient = false;
            EntityManager.QueueDeleteEntity(args.Used);
            int tier = CheckTier(component.DesiredPrototype.ID);
            string message = Loc.GetString(_random.Pick(RewardMessages));
            if (tier > 1)
                message = Loc.GetString(_random.Pick(RewardMessagesTier2));
            _chat.TrySendInGameICMessage(uid, message, InGameICChatType.Speak, true);
            RewardServicer(args.User, component, tier);

            if (nextItem)
                NextItem(component);
        }
        private bool CheckValidity(EntityPrototype given, EntityPrototype target)
        {
            // 1: directly compare Names
            // name instead of ID because the oracle asks for them by name
            // this could potentially lead to like, labeller exploits maybe but so far only mob names can be fully player-set.
            if (given.Name == target.Name)
                return true;

            return false;
        }
        private int CheckTier(String target)
        {
            if (_tier2Protos.Contains(target))
                return 2;
            if (_tier3Protos.Contains(target))
                return 3;
            return 1;
        }

        private void NextItem(RoboisseurComponent component)
        {
            component.Accumulator = 0;
            component.BarkAccumulator = 0;
            var protoString = GetDesiredItem();
            if (_prototypeManager.TryIndex<EntityPrototype>(protoString, out var proto))
                component.DesiredPrototype = proto;
            else
                _sawmill.Error("Oracle can't index prototype " + protoString);
        }

        private string GetDesiredItem()
        {
            return _random.Pick(GetAllProtos());
        }
        public List<string> GetAllProtos()
        {

            var allRecipes = _prototypeManager.EnumeratePrototypes<FoodRecipePrototype>();
            var allProtos = new List<String>();

            foreach (var recipe in allRecipes)
            {
                allProtos.Add(recipe.Result);
            }
            foreach (var proto in BlacklistedProtos)
                allProtos.Remove(proto);
            return allProtos;
        }
    }
}