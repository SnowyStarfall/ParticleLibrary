namespace ParticleLibrary.Core
{
	public enum Layer
	{
		/// <summary>
		/// Will not draw.
		/// </summary>
		None,
		/// <summary>
		/// Background.
		/// </summary>
		BeforeBackground,
		/// <summary>
		/// Walls.
		/// </summary>
		BeforeWalls,
		/// <summary>
		/// Trees, flowers, rocks, etc.
		/// </summary>
		BeforeNonSolidTiles,
		/// <summary>
		/// Worm enemies.
		/// </summary>
		BeforeNPCsBehindTiles,
		/// <summary>
		/// Tiles.
		/// </summary>
		BeforeTiles,
		/// <summary>
		/// Player details drawn behind NPCs.
		/// </summary>
		BeforePlayersBehindNPCs,
		/// <summary>
		/// NPCs.
		/// </summary>
		BeforeNPCs,
		/// <summary>
		/// Projectiles.
		/// </summary>
		BeforeProjectiles,
		/// <summary>
		/// Players.
		/// </summary>
		BeforePlayers,
		/// <summary>
		/// Items dropped in world.
		/// </summary>
		BeforeItems,
		/// <summary>
		/// Rain.
		/// </summary>
		BeforeRain,
		/// <summary>
		/// Gore.
		/// </summary>
		BeforeGore,
		/// <summary>
		/// Dust.
		/// </summary>
		BeforeDust,
		/// <summary>
		/// Water. Adjust draw position by new Vector2(Main.offScreenRange, Main.offScreenRange).
		/// </summary>
		BeforeWater,
		/// <summary>
		/// Before UI.
		/// </summary>
		BeforeUI,
		/// <summary>
		/// After UI.
		/// </summary>
		AfterUI,
		/// <summary>
		/// Before Main Menu.
		/// </summary>
		BeforeMainMenu,
		/// <summary>
		/// After Main Menu.
		/// </summary>
		AfterMainMenu,
	}
}
