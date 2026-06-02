using ModSmith.Models;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Events;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Logging;


namespace JacksMod;

// An example event that allows the player to take gold or attempt to double it.

public sealed class TheGoldCoinRoom : ModSmithEventModel
{
  public static Logger Logger { get; } = new(JacksModMain.ModId, LogType.Generic);

  protected override IReadOnlyList<EventOption> GenerateInitialOptions()
  {
    Logger.Info("The gold coin room GIO");

    IReadOnlyList<EventOption> rv = [
      new EventOption(this, TakeGold, "THE_GOLD_COIN_ROOM.pages.INITIAL.options.TAKE"),
      new EventOption(this, DoubleIt, "THE_GOLD_COIN_ROOM.pages.INITIAL.options.DOUBLE_IT"),
    ];
    Logger.Info("The gold coin room GIO generated" );

    return rv;
  }

  protected override IEnumerable<DynamicVar> CanonicalVars => [
    new GoldVar(20),
  ];

  private async Task TakeGold()
  {
    if (Owner is Player player)
    {
      await PlayerCmd.GainGold(DynamicVars.Gold.IntValue, player);
    }
    SetEventFinished(L10NLookup("THE_GOLD_COIN_ROOM.pages.TAKEN.description"));
  }

  private Task DoubleIt()
  {
    var doubled = Rng.NextBool();
    if (doubled)
    {
      DynamicVars.Gold.BaseValue *= 2;
      SetEventState(L10NLookup("THE_GOLD_COIN_ROOM.pages.DOUBLED.description"), [
        new EventOption(this, TakeGold, "THE_GOLD_COIN_ROOM.pages.DOUBLED.options.TAKE"),
        new EventOption(this, DoubleIt, "THE_GOLD_COIN_ROOM.pages.DOUBLED.options.DOUBLE_IT"),
      ]);
    }
    else
    {
      SetEventFinished(L10NLookup("THE_GOLD_COIN_ROOM.pages.BUST.description"));
    }
    return Task.CompletedTask;
  }
}
