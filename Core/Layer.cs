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
		/// Walls. <b>After Background.</b>
		/// </summary>
		BeforeWalls,
		/// <summary>
		/// Trees, flowers, rocks, etc. <b>After Walls.</b>
		/// </summary>
		BeforeNonSolidTiles,
		/// <summary>
		/// Worm enemies. <b>After mon-solid Tiles.</b>
		/// </summary>
		BeforeNPCsBehindTiles,
		/// <summary>
		/// Tiles. <b>After NPCs drawn behind Tiles.</b>
		/// </summary>
		BeforeSolidTiles,
		/// <summary>
		/// Player details drawn behind NPCs. <b>After Solid Tiles.</b>
		/// </summary>
		BeforePlayersBehindNPCs,
		/// <summary>
		/// NPCs. <b>After Player details drawn behind NPCs.</b>
		/// </summary>
		BeforeNPCs,
		/// <summary>
		/// Projectiles. <b>After NPCs.</b>
		/// </summary>
		BeforeProjectiles,
		/// <summary>
		/// Players. <b>After Projectiles.</b>
		/// </summary>
		BeforePlayers,
		/// <summary>
		/// Items dropped in world. <b>After Players.</b>
		/// </summary>
		BeforeItems,
		/// <summary>
		/// Rain. <b>After Items.</b>
		/// </summary>
		BeforeRain,
		/// <summary>
		/// Gore. <b>After Rain.</b>
		/// </summary>
		BeforeGore,
		/// <summary>
		/// Dust. <b>After Gore.</b>
		/// </summary>
		BeforeDust,
		/// <summary>
		/// Water <b>After Dust.</b>  Adjust draw position by new Vector2(Main.offScreenRange, Main.offScreenRange).
		/// </summary>
		BeforeWater,
		///// <summary>
		///// After Water. Adjust draw position by new Vector2(Main.offScreenRange, Main.offScreenRange).
		///// </summary>
		//AfterWater,
		/// <summary>
		/// Before UI.
		/// </summary>
		BeforeInterface,
		/// <summary>
		/// After UI.
		/// </summary>
		AfterInterface,
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
