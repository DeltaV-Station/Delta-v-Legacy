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

        public List<String> Tier2Protos = new()
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

        public List<String> Tier3Protos = new()
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
    }
}
