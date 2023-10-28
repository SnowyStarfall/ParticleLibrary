using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ReLogic.Content;
using ReLogic.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Terraria;
using Terraria.Audio;
using Terraria.GameContent.UI.Elements;
using Terraria.GameInput;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.UI;
using static ParticleLibrary.Core.CParticle;
using static Terraria.GameContent.FontAssets;

namespace ParticleLibrary.Interface
{
	internal class DebugUI : UIState
	{
		// Instance
		internal DebugUI Instance;

		//// UI Variables
		//private BasicDebugDrawer Debug;
		//private Vector2 offset;
		//private bool Dragging;
		//private bool Visible;
		//internal bool FreezeAI;
		//internal bool FreezeVelocity;

		//// Elements & Info
		//private UIPanel MainPanel;
		//private UICustomImageButton velocityButton;
		//private UICustomImageButton hitboxButton;
		//private UICustomImageButton freezeAIButton;
		//private UICustomImageButton freezeVelocityButton;
		//private UICustomImageButton emitterInfoButton;

		//private UIPanel InfoPanel;
		//private Particle TrackedParticle;
		//private Emitter TrackedEmitter;
		//private UIPanel countTab;
		//private UICustomImageButton clearParticle;
		//private UICustomImageButton velocityDropdown;
		//private UICustomImageButton positionDropdown;
		//private UICustomImageButton rotationDropdown;
		//private UICustomImageButton scaleDropdown;
		//private UICustomImageButton arrayDropdown;
		//private UICustomImageButton miscDropdown;
		//private UICustomImageButton entityDropdown;
		//private Vector2 largestInfoSize;
		//private Circle infoCircle;

		//private UIPanel DictPanel;
		//private UIPanel cullingPanel;
		//private UICustomList dictList;
		//private List<Particle> particles;
		//private List<Particle> selectedParticles;
		//private UICustomSearchBar searchBar;
		//private UICustomScrollBar scrollBar;
		//private float dictListHeight;
		//private bool dictListOrdered;

		//private enum AreaType
		//{
		//	Point,
		//	Circle,
		//	Square
		//}

		//private UIPanel SpawnPanel;
		//private Vector2 spawnPosition;
		//private AreaType area;
		//private UICustomImageButton spawnDropper;
		//private UICustomImageButton squareSetting;
		//private UICustomSlider frequency;
		//private UICustomSlider amount;
		//private UICustomSlider velocityX;
		//private UICustomSlider velocityY;
		//private UICustomSlider velocityRandomness;
		//private UICustomSearchBar ai0;
		//private UICustomSearchBar ai1;
		//private UICustomSearchBar ai2;
		//private UICustomSearchBar ai3;
		//private UICustomSearchBar ai4;
		//private UICustomSearchBar ai5;
		//private UICustomSearchBar ai6;
		//private UICustomSearchBar ai7;
		//private UICustomSearchBar colorR;
		//private UICustomSearchBar colorG;
		//private UICustomSearchBar colorB;
		//private UICustomSearchBar colorA;
		//private UICustomSearchBar scaleX;
		//private UICustomSearchBar scaleY;
		//private UIPanel layerPanel;
		//private UICustomList layerDropdown;
		//private UICustomScrollBar layerScrollbar;
		//private Layer currentLayer;
		//private DraggablePoint nX;
		//private DraggablePoint nY;
		//private Circle debugCircle;
		//private bool spawnDropperDragging;
		//private int spawnTimer;

		//public override void OnInitialize()
		//{
		//	Instance = this;

		//	// Visibility
		//	Visible = ParticleLibraryConfig.Instance.DebugUI;
		//	if (!Visible)
		//		return;

		//	// Set Debug Drawer
		//	Main.QueueMainThreadAction(() => Debug = new BasicDebugDrawer(Main.graphics.GraphicsDevice));

		//	On_Dust.UpdateDust += Dust_UpdateDust;

		//	#region Textures
		//	Asset<Texture2D> box = ModContent.Request<Texture2D>("ParticleLibrary/Debug/Box", AssetRequestMode.ImmediateLoad);
		//	Asset<Texture2D> minus = ModContent.Request<Texture2D>("ParticleLibrary/Debug/Minus", AssetRequestMode.ImmediateLoad);
		//	Asset<Texture2D> plus = ModContent.Request<Texture2D>("ParticleLibrary/Debug/Plus", AssetRequestMode.ImmediateLoad);
		//	Asset<Texture2D> x = ModContent.Request<Texture2D>("ParticleLibrary/Debug/X", AssetRequestMode.ImmediateLoad);
		//	Asset<Texture2D> square = ModContent.Request<Texture2D>("ParticleLibrary/Debug/Square", AssetRequestMode.ImmediateLoad);
		//	Asset<Texture2D> circle = ModContent.Request<Texture2D>("ParticleLibrary/Debug/Circle", AssetRequestMode.ImmediateLoad);
		//	Asset<Texture2D> highlight = ModContent.Request<Texture2D>("ParticleLibrary/Debug/BoxHighlight", AssetRequestMode.ImmediateLoad);
		//	Asset<Texture2D> squareTarget = ModContent.Request<Texture2D>("ParticleLibrary/Debug/SquareTarget", AssetRequestMode.ImmediateLoad);
		//	Asset<Texture2D> circleTarget = ModContent.Request<Texture2D>("ParticleLibrary/Debug/CircleTarget", AssetRequestMode.ImmediateLoad);
		//	#endregion

		//	#region Main Panel
		//	// Main Panel
		//	MainPanel = new UIPanel(ModContent.Request<Texture2D>("ParticleLibrary/Debug/PanelBackground", AssetRequestMode.ImmediateLoad), ModContent.Request<Texture2D>("ParticleLibrary/Debug/PanelFrame", AssetRequestMode.ImmediateLoad), 10, 2);
		//	MainPanel.Width.Set(230f, 0f);
		//	MainPanel.Height.Set(136f, 0f);
		//	MainPanel.BackgroundColor = Color.White;
		//	MainPanel.BorderColor = Color.White;

		//	velocityButton = new UICustomImageButton(box, square, highlight, isSmall: true);
		//	velocityButton.Width.Set(18f, 0f);
		//	velocityButton.Height.Set(18f, 0f);
		//	velocityButton.visible = true;

		//	hitboxButton = new UICustomImageButton(box, square, highlight, isSmall: true);
		//	hitboxButton.Width.Set(18f, 0f);
		//	hitboxButton.Height.Set(18f, 0f);
		//	hitboxButton.Top.Pixels = velocityButton.Top.Pixels + 20f;
		//	hitboxButton.visible = true;

		//	freezeAIButton = new UICustomImageButton(box, square, highlight, isSmall: true);
		//	freezeAIButton.Width.Set(18f, 0f);
		//	freezeAIButton.Height.Set(18f, 0f);
		//	freezeAIButton.Top.Pixels = hitboxButton.Top.Pixels + 20f;
		//	freezeAIButton.visible = true;

		//	freezeVelocityButton = new UICustomImageButton(box, square, highlight, isSmall: true);
		//	freezeVelocityButton.Width.Set(18f, 0f);
		//	freezeVelocityButton.Height.Set(18f, 0f);
		//	freezeVelocityButton.Top.Pixels = freezeAIButton.Top.Pixels + 20f;
		//	freezeVelocityButton.visible = true;

		//	emitterInfoButton = new UICustomImageButton(box, square, highlight, isSmall: true);
		//	emitterInfoButton.Width.Set(18f, 0f);
		//	emitterInfoButton.Height.Set(18f, 0f);
		//	emitterInfoButton.Top.Pixels = freezeVelocityButton.Top.Pixels + 20f;
		//	emitterInfoButton.visible = true;
		//	#endregion

		//	#region Info Panel
		//	// Info Panel
		//	InfoPanel = new UIPanel(ModContent.Request<Texture2D>("ParticleLibrary/Debug/PanelBackground", AssetRequestMode.ImmediateLoad), ModContent.Request<Texture2D>("ParticleLibrary/Debug/PanelFrame", AssetRequestMode.ImmediateLoad), 10, 2);
		//	InfoPanel.Width.Set(258f, 0f);
		//	InfoPanel.Height.Set(250f, 0f);
		//	InfoPanel.BackgroundColor = Color.White;
		//	InfoPanel.BorderColor = Color.White;

		//	countTab = new UIPanel(ModContent.Request<Texture2D>("ParticleLibrary/Debug/PanelBackground", AssetRequestMode.ImmediateLoad), ModContent.Request<Texture2D>("ParticleLibrary/Debug/Scrollbar", AssetRequestMode.ImmediateLoad), 8, 2);
		//	countTab.Width.Set(80f, 0f);
		//	countTab.Height.Set(30f, 0f);
		//	countTab.Left.Set(InfoPanel.Left.Pixels, 0f);
		//	countTab.Top.Set(InfoPanel.Top.Pixels - 30f, 0f);

		//	clearParticle = new UICustomImageButton(box, x, highlight, x, isSmall: true);
		//	clearParticle.Width.Set(18f, 0f);
		//	clearParticle.Height.Set(18f, 0f);
		//	clearParticle.visible = true;
		//	clearParticle.OnLeftClick += (e, l) => TrackedParticle = null;

		//	velocityDropdown = new UICustomImageButton(box, minus, highlight, plus, isSmall: true);
		//	velocityDropdown.Width.Set(18f, 0f);
		//	velocityDropdown.Height.Set(18f, 0f);
		//	velocityDropdown.visible = true;

		//	positionDropdown = new UICustomImageButton(box, minus, highlight, plus, isSmall: true);
		//	positionDropdown.Width.Set(18f, 0f);
		//	positionDropdown.Height.Set(18f, 0f);
		//	positionDropdown.visible = true;

		//	rotationDropdown = new UICustomImageButton(box, minus, highlight, plus, isSmall: true);
		//	rotationDropdown.Width.Set(18f, 0f);
		//	rotationDropdown.Height.Set(18f, 0f);
		//	rotationDropdown.visible = true;

		//	scaleDropdown = new UICustomImageButton(box, minus, highlight, plus, isSmall: true);
		//	scaleDropdown.Width.Set(18f, 0f);
		//	scaleDropdown.Height.Set(18f, 0f);
		//	scaleDropdown.visible = true;

		//	arrayDropdown = new UICustomImageButton(box, minus, highlight, plus, isSmall: true);
		//	arrayDropdown.Width.Set(18f, 0f);
		//	arrayDropdown.Height.Set(18f, 0f);
		//	arrayDropdown.visible = true;

		//	miscDropdown = new UICustomImageButton(box, minus, highlight, plus, isSmall: true);
		//	miscDropdown.Width.Set(18f, 0f);
		//	miscDropdown.Height.Set(18f, 0f);
		//	miscDropdown.visible = true;

		//	entityDropdown = new UICustomImageButton(box, minus, highlight, plus, isSmall: true);
		//	entityDropdown.Width.Set(18f, 0f);
		//	entityDropdown.Height.Set(18f, 0f);
		//	entityDropdown.visible = true;

		//	infoCircle = new(Vector2.Zero, Vector2.Zero, new Color(0f, 1f, 0f, 1f));
		//	#endregion

		//	#region Dict Panel
		//	DictPanel = new UIPanel(ModContent.Request<Texture2D>("ParticleLibrary/Debug/PanelBackground", AssetRequestMode.ImmediateLoad), ModContent.Request<Texture2D>("ParticleLibrary/Debug/PanelFrame", AssetRequestMode.ImmediateLoad), 10, 2);
		//	DictPanel.Width.Set(288f, 0f);
		//	DictPanel.Height.Set(256f, 0f);
		//	DictPanel.BackgroundColor = Color.White;
		//	DictPanel.BorderColor = Color.White;

		//	cullingPanel = new UIPanel(ModContent.Request<Texture2D>("ParticleLibrary/Debug/BlackPanelBackground", AssetRequestMode.ImmediateLoad), ModContent.Request<Texture2D>("ParticleLibrary/Debug/BlackPanel"), 2, 2);
		//	cullingPanel.Width.Set(236f, 0f);
		//	cullingPanel.Height.Set(256f - 60f, 0f);
		//	cullingPanel.BackgroundColor = Color.White;
		//	cullingPanel.BorderColor = Color.White;

		//	searchBar = new UICustomSearchBar("Search...", 1f, "");
		//	searchBar.Width.Set(236f, 0f);
		//	searchBar.Height.Set(32f, 0f);

		//	scrollBar = new UICustomScrollBar(ModContent.Request<Texture2D>("ParticleLibrary/Debug/Scrollbar", AssetRequestMode.ImmediateLoad).Value, ModContent.Request<Texture2D>("ParticleLibrary/Debug/Scrollbutton", AssetRequestMode.ImmediateLoad).Value);
		//	scrollBar.Width.Set(10f, 0f);
		//	scrollBar.Height.Set(DictPanel.Height.Pixels, 0f);

		//	dictList = new UICustomList();
		//	dictList.SetScrollbar(scrollBar);
		//	#endregion

		//	#region Spawn Panel
		//	SpawnPanel = new UIPanel(ModContent.Request<Texture2D>("ParticleLibrary/Debug/PanelBackground", AssetRequestMode.ImmediateLoad), ModContent.Request<Texture2D>("ParticleLibrary/Debug/PanelFrame", AssetRequestMode.ImmediateLoad), 10, 2);
		//	SpawnPanel.Height.Set(256f, 0f);
		//	SpawnPanel.BackgroundColor = Color.White;
		//	SpawnPanel.BorderColor = Color.White;

		//	spawnDropper = new UICustomImageButton(box, circle, highlight, isSmall: true);
		//	spawnDropper.Width.Set(18f, 0f);
		//	spawnDropper.Height.Set(18f, 0f);
		//	spawnDropper.visible = true;
		//	spawnDropper.OnLeftMouseDown += (e, l) =>
		//	{
		//		if (spawnDropper.enabled)
		//			spawnDropperDragging = true;
		//	};
		//	spawnDropper.OnLeftClick += (e, l) =>
		//	{
		//		nX.visible = !nX.visible;
		//		nY.visible = !nY.visible;

		//		spawnDropper.enabled = true;
		//		spawnDropperDragging = true;

		//		if (nX.visible)
		//		{
		//			nX.SetPosition(spawnPosition, spawnPosition - new Vector2(200f, 0f));
		//			nY.SetPosition(spawnPosition, spawnPosition - new Vector2(0f, 200f));
		//		}
		//	};

		//	squareSetting = new UICustomImageButton(box, squareTarget, highlight, circleTarget, isSmall: true);
		//	squareSetting.Width.Set(18f, 0f);
		//	squareSetting.Height.Set(18f, 0f);
		//	squareSetting.visible = true;
		//	squareSetting.OnLeftClick += (e, l) =>
		//	{
		//		if (area == AreaType.Point)
		//		{
		//			squareSetting.tick = ModContent.Request<Texture2D>("ParticleLibrary/Debug/CircleTarget", AssetRequestMode.ImmediateLoad).Value;
		//			squareSetting.noTick = ModContent.Request<Texture2D>("ParticleLibrary/Debug/CircleTarget", AssetRequestMode.ImmediateLoad).Value;
		//			area = AreaType.Circle;
		//		}
		//		else if (area == AreaType.Circle)
		//		{
		//			squareSetting.tick = ModContent.Request<Texture2D>("ParticleLibrary/Debug/SquareTarget", AssetRequestMode.ImmediateLoad).Value;
		//			squareSetting.noTick = ModContent.Request<Texture2D>("ParticleLibrary/Debug/SquareTarget", AssetRequestMode.ImmediateLoad).Value;
		//			area = AreaType.Square;
		//		}
		//		else if (area == AreaType.Square)
		//		{
		//			squareSetting.tick = ModContent.Request<Texture2D>("ParticleLibrary/Debug/PointTarget", AssetRequestMode.ImmediateLoad).Value;
		//			squareSetting.noTick = ModContent.Request<Texture2D>("ParticleLibrary/Debug/PointTarget", AssetRequestMode.ImmediateLoad).Value;
		//			area = AreaType.Point;
		//		}
		//	};

		//	debugCircle = new(Vector2.Zero, Vector2.Zero, new Color(0f, 1f, 0f, 1f));

		//	nX = new DraggablePoint(Vector2.Zero, false, true, false);
		//	nY = new DraggablePoint(Vector2.Zero, false, false, true);

		//	frequency = new UICustomSlider(0, 600, 60);
		//	frequency.Width.Set(300f, 0f);
		//	frequency.Height.Set(10f, 0f);

		//	amount = new UICustomSlider(0, 100, 1);
		//	amount.Width.Set(300f, 0f);
		//	amount.Height.Set(10f, 0f);

		//	velocityX = new UICustomSlider(-64, 64, 0);
		//	velocityX.Width.Set(300f, 0f);
		//	velocityX.Height.Set(10f, 0f);

		//	velocityY = new UICustomSlider(-64, 64, 0);
		//	velocityY.Width.Set(300f, 0f);
		//	velocityY.Height.Set(10f, 0f);

		//	velocityRandomness = new UICustomSlider(0, 64, 0);
		//	velocityRandomness.Width.Set(300f, 0f);
		//	velocityRandomness.Height.Set(10f, 0f);

		//	layerPanel = new UIPanel(ModContent.Request<Texture2D>("ParticleLibrary/Debug/BlackPanelBackground", AssetRequestMode.ImmediateLoad), ModContent.Request<Texture2D>("ParticleLibrary/Debug/BlackPanel"), 2, 2);
		//	layerPanel.Width.Set(160f, 0f);
		//	layerPanel.Height.Set(400f, 0f);
		//	layerPanel.BackgroundColor = Color.White;
		//	layerPanel.BorderColor = Color.White;

		//	layerScrollbar = new UICustomScrollBar(ModContent.Request<Texture2D>("ParticleLibrary/Debug/Scrollbar", AssetRequestMode.ImmediateLoad).Value, ModContent.Request<Texture2D>("ParticleLibrary/Debug/Scrollbutton", AssetRequestMode.ImmediateLoad).Value);
		//	layerScrollbar.Width.Set(10f, 0f);
		//	layerScrollbar.Height.Set(layerPanel.Height.Pixels, 0f);

		//	layerDropdown = new();
		//	layerDropdown.SetScrollbar(layerScrollbar);

		//	Asset<Texture2D> background = ModContent.Request<Texture2D>("ParticleLibrary/Debug/BoxBackground", AssetRequestMode.ImmediateLoad);
		//	for (int i = 0; i < 18; i++)
		//	{
		//		Layer layer = (Layer)i;
		//		Color color = i % 2 == 0 ? new(0.909804f, 0.63529414f, 0.44705886f) : new(0.45882356f, 0.49411768f, 0.7803922f);

		//		UICustomSelectablePanel panel = new(background, box, 8, 2, layer, color, layer.ToString());
		//		panel.Width.Set(200f, 0f);
		//		panel.Height.Set(26f, 0f);
		//		panel.BackgroundColor = Color.White * 0.25f;
		//		panel.BorderColor = Color.White * 0.25f;
		//		panel.OnLeftClick += (e, l) =>
		//		{
		//			for (int k = 0; k < layerDropdown._items.Count; k++)
		//			{
		//				if (layerDropdown._items[k] != panel)
		//				{
		//					(layerDropdown._items[k] as UICustomSelectablePanel).selected = false;
		//					(layerDropdown._items[k] as UICustomSelectablePanel).BackgroundColor = Color.White * 0.25f;
		//					(layerDropdown._items[k] as UICustomSelectablePanel).BorderColor = Color.White * 0.25f;
		//				}
		//			}
		//			panel.selected = true;
		//			panel.BackgroundColor = Color.White;
		//			panel.BorderColor = Color.White;
		//			currentLayer = (Layer)((l as UICustomSelectablePanel)?.value);
		//		};

		//		layerPanel.Append(panel);
		//		layerDropdown.Add(panel);
		//	}
		//	currentLayer = Layer.BeforeDust;

		//	string FloatFunc(string s)
		//	{
		//		int index = s.IndexOf(".");
		//		if (index != -1)
		//		{
		//			int index2 = s.IndexOf(".", index + 1);
		//			if (index2 != -1)
		//			{
		//				string s1 = s[..(index + 1)];
		//				string s2 = s[(index + 1)..];
		//				s2 = s2.Replace(".", "");
		//				s = s1 + s2;
		//			}
		//		}

		//		bool success = double.TryParse(s, out double result);
		//		if (!success)
		//			s = string.Empty;
		//		return s;
		//	}

		//	string IntFunc(string s)
		//	{
		//		bool success = double.TryParse(s, out double result);
		//		return success ? ((int)result).ToString() : string.Empty;
		//	}

		//	ai0 = new UICustomSearchBar("f", 1f, "0.0");
		//	ai0.Width.Set(40f, 0f);
		//	ai0.Height.Set(16f, 0f);
		//	ai0.textChanged = FloatFunc;

		//	ai1 = new UICustomSearchBar("f", 1f, "0.0");
		//	ai1.Width.Set(40f, 0f);
		//	ai1.Height.Set(16f, 0f);
		//	ai1.textChanged = FloatFunc;

		//	ai2 = new UICustomSearchBar("f", 1f, "0.0");
		//	ai2.Width.Set(40f, 0f);
		//	ai2.Height.Set(16f, 0f);
		//	ai2.textChanged = FloatFunc;

		//	ai3 = new UICustomSearchBar("f", 1f, "0.0");
		//	ai3.Width.Set(40f, 0f);
		//	ai3.Height.Set(16f, 0f);
		//	ai3.textChanged = FloatFunc;

		//	ai4 = new UICustomSearchBar("f", 1f, "0.0");
		//	ai4.Width.Set(40f, 0f);
		//	ai4.Height.Set(16f, 0f);
		//	ai4.textChanged = FloatFunc;

		//	ai5 = new UICustomSearchBar("f", 1f, "0.0");
		//	ai5.Width.Set(40f, 0f);
		//	ai5.Height.Set(16f, 0f);
		//	ai5.textChanged = FloatFunc;

		//	ai6 = new UICustomSearchBar("f", 1f, "0.0");
		//	ai6.Width.Set(40f, 0f);
		//	ai6.Height.Set(16f, 0f);
		//	ai6.textChanged = FloatFunc;

		//	ai7 = new UICustomSearchBar("f", 1f, "0.0");
		//	ai7.Width.Set(40f, 0f);
		//	ai7.Height.Set(16f, 0f);
		//	ai7.textChanged = FloatFunc;

		//	colorR = new UICustomSearchBar("int", 1f, "255");
		//	colorR.Width.Set(80f, 0f);
		//	colorR.Height.Set(16f, 0f);
		//	colorR.textChanged = IntFunc;

		//	colorG = new UICustomSearchBar("int", 1f, "255");
		//	colorG.Width.Set(80f, 0f);
		//	colorG.Height.Set(16f, 0f);
		//	colorG.textChanged = IntFunc;

		//	colorB = new UICustomSearchBar("int", 1f, "255");
		//	colorB.Width.Set(80f, 0f);
		//	colorB.Height.Set(16f, 0f);
		//	colorB.textChanged = IntFunc;

		//	colorA = new UICustomSearchBar("int", 1f, "255");
		//	colorA.Width.Set(80f, 0f);
		//	colorA.Height.Set(16f, 0f);
		//	colorA.textChanged = IntFunc;

		//	scaleX = new UICustomSearchBar("f", 1f, "1.0");
		//	scaleX.Width.Set(160f, 0f);
		//	scaleX.Height.Set(16f, 0f);
		//	scaleX.textChanged = FloatFunc;

		//	scaleY = new UICustomSearchBar("f", 1f, "1.0");
		//	scaleY.Width.Set(160f, 0f);
		//	scaleY.Height.Set(16f, 0f);
		//	scaleY.textChanged = FloatFunc;
		//	#endregion

		//	#region Appending
		//	Append(countTab);

		//	MainPanel.Append(velocityButton);
		//	MainPanel.Append(hitboxButton);
		//	MainPanel.Append(freezeAIButton);
		//	MainPanel.Append(freezeVelocityButton);
		//	MainPanel.Append(emitterInfoButton);
		//	Append(MainPanel);

		//	InfoPanel.Append(clearParticle);
		//	InfoPanel.Append(velocityDropdown);
		//	InfoPanel.Append(positionDropdown);
		//	InfoPanel.Append(rotationDropdown);
		//	InfoPanel.Append(scaleDropdown);
		//	InfoPanel.Append(arrayDropdown);
		//	InfoPanel.Append(miscDropdown);
		//	InfoPanel.Append(entityDropdown);
		//	Append(InfoPanel);

		//	cullingPanel.Append(dictList);
		//	DictPanel.Append(cullingPanel);
		//	DictPanel.Append(searchBar);
		//	DictPanel.Append(scrollBar);
		//	Append(DictPanel);

		//	SpawnPanel.Append(spawnDropper);
		//	SpawnPanel.Append(squareSetting);
		//	SpawnPanel.Append(frequency);
		//	SpawnPanel.Append(amount);
		//	SpawnPanel.Append(velocityX);
		//	SpawnPanel.Append(velocityY);
		//	SpawnPanel.Append(velocityRandomness);
		//	SpawnPanel.Append(ai0);
		//	SpawnPanel.Append(ai1);
		//	SpawnPanel.Append(ai2);
		//	SpawnPanel.Append(ai3);
		//	SpawnPanel.Append(ai4);
		//	SpawnPanel.Append(ai5);
		//	SpawnPanel.Append(ai6);
		//	SpawnPanel.Append(ai7);
		//	SpawnPanel.Append(colorR);
		//	SpawnPanel.Append(colorG);
		//	SpawnPanel.Append(colorB);
		//	SpawnPanel.Append(colorA);
		//	SpawnPanel.Append(scaleX);
		//	SpawnPanel.Append(scaleY);
		//	layerPanel.Append(layerDropdown);
		//	layerPanel.Append(layerScrollbar);
		//	Append(nX);
		//	Append(nY);
		//	Append(SpawnPanel);
		//	Append(layerPanel);

		//	Recalculate();
		//	CalculatedStyle style = GetDimensions();
		//	Width.Set(style.Width, 0f);
		//	Height.Set(style.Height, 0f);
		//	#endregion
		//}

		//#region Updating
		//public override void Update(GameTime gameTime)
		//{
		//	if (!Visible)
		//		return;

		//	if (particles == null)
		//	{
		//		particles = UISystem.Instance.DebugUIElement.Instance.GetAllParticles();
		//		selectedParticles = new();
		//	}

		//	base.Update(gameTime);

		//	if (TrackedParticle?.TimeLeft <= 0)
		//	{
		//		largestInfoSize = Vector2.Zero;
		//		TrackedParticle = null;
		//	}

		//	FreezeAI = freezeAIButton.enabled;
		//	FreezeVelocity = freezeVelocityButton.enabled;

		//	DragCalculation();
		//}
		//private void Dust_UpdateDust(On_Dust.orig_UpdateDust orig)
		//{
		//	if (spawnPosition != Vector2.Zero && selectedParticles.Count > 0)
		//	{
		//		if (spawnTimer <= 0)
		//		{
		//			spawnTimer = frequency.value;
		//			for (int i = 0; i < amount.value; i++)
		//			{
		//				int index = selectedParticles.Count == 1 ? 0 : Main.rand.Next(0, selectedParticles.Count);
		//				float radians = MathHelper.ToRadians((Main.rand.NextFloat(0, 36000) / 100f));
		//				float xDist = nX.GetDistanceFromCenter();
		//				float yDist = nY.GetDistanceFromCenter();
		//				Vector2 modifier;
		//				if (area == AreaType.Square)
		//				{
		//					modifier = new(Main.rand.NextFloat(-xDist, xDist + 1f), Main.rand.NextFloat(-yDist, yDist + 1f));
		//				}
		//				else if (area == AreaType.Circle)
		//				{
		//					float sin = (float)Math.Sin(radians);
		//					float cos = (float)Math.Cos(radians);
		//					modifier = new((xDist - Main.rand.NextFloat(0f, xDist + 1f)) * sin, (yDist - Main.rand.NextFloat(0f, yDist + 1f)) * cos);
		//				}
		//				else
		//				{
		//					modifier = Vector2.Zero;
		//				}

		//				float modX = Main.rand.NextFloat(-velocityRandomness.value, velocityRandomness.value + 1);
		//				float modY = Main.rand.NextFloat(-velocityRandomness.value, velocityRandomness.value + 1);
		//				CParticleSystem.NewParticle(spawnPosition + modifier, new Vector2(velocityX.value + modX, velocityY.value + modY), selectedParticles[index], new Color(colorR.GetValue(), colorG.GetValue(), colorB.GetValue(), colorA.GetValue()), new Vector2(scaleX.GetValue(), scaleY.GetValue()), ai0.GetValue(), ai1.GetValue(), ai2.GetValue(), ai3.GetValue(), ai4.GetValue(), ai5.GetValue(), ai6.GetValue(), ai7.GetValue(), currentLayer);
		//			}
		//		}
		//		spawnTimer--;
		//	}
		//	orig();
		//}
		//#endregion

		//#region Drawing
		//public override void Draw(SpriteBatch spriteBatch)
		//{
		//	if (!Visible)
		//		return;

		//	base.Draw(spriteBatch);

		//	DrawParticleDebug(spriteBatch);
		//	DrawMainPanel(spriteBatch);
		//	DrawInfoPanel(spriteBatch);
		//	DrawDictPanel(spriteBatch);
		//	DrawCountPanel(spriteBatch);
		//	DrawSpawnPanel(spriteBatch);

		//	DrawDebug(spriteBatch);
		//}
		//internal void DrawMainPanel(SpriteBatch spriteBatch)
		//{
		//	MainPanel.Left.Set(InfoPanel.Width.Pixels, 0f);

		//	CalculatedStyle dimensions = velocityButton.GetDimensions();
		//	Vector2 position = new(dimensions.X, dimensions.Y);

		//	MainPanel.Width.Set(InfoPanel.Width.Pixels, 0f);
		//	MainPanel.Height.Set(118f, 0f);
		//	MainPanel.Left.Set(InfoPanel.Left.Pixels, 0f);
		//	MainPanel.Top.Set(InfoPanel.Height.Pixels, 0f);

		//	Recalculate();

		//	if (!velocityButton.visible)
		//		return;

		//	Texture2D panel = ModContent.Request<Texture2D>("ParticleLibrary/Debug/BlackPanel", AssetRequestMode.ImmediateLoad).Value;
		//	DrawPanel(spriteBatch, panel, Color.White * 0.5f, position, new Vector2(MainPanel.Width.Pixels - 20f, MainPanel.Height.Pixels - 20f), 2, 2);

		//	spriteBatch.DrawString(MouseText.Value, "Show Velocity", position + new Vector2(24f, 0f), Color.White, 0f, Vector2.Zero, 0.75f, SpriteEffects.None, 0f);
		//	spriteBatch.DrawString(MouseText.Value, "Show Hitbox", position + new Vector2(24f, 20f), Color.White, 0f, Vector2.Zero, 0.75f, SpriteEffects.None, 0f);
		//	spriteBatch.DrawString(MouseText.Value, "Freeze AI", position + new Vector2(24f, 40f), Color.White, 0f, Vector2.Zero, 0.75f, SpriteEffects.None, 0f);
		//	spriteBatch.DrawString(MouseText.Value, "Freeze Velocity", position + new Vector2(24f, 60f), Color.White, 0f, Vector2.Zero, 0.75f, SpriteEffects.None, 0f);
		//	spriteBatch.DrawString(MouseText.Value, "Display Emitter Info", position + new Vector2(24f, 80f), Color.White, 0f, Vector2.Zero, 0.75f, SpriteEffects.None, 0f);

		//	velocityButton.Draw(spriteBatch);
		//	hitboxButton.Draw(spriteBatch);
		//	freezeAIButton.Draw(spriteBatch);
		//	freezeVelocityButton.Draw(spriteBatch);
		//	emitterInfoButton.Draw(spriteBatch);
		//}
		//internal void DrawInfoPanel(SpriteBatch spriteBatch)
		//{
		//	Vector2 size = new(64f);
		//	if (TrackedParticle == null)
		//	{
		//		InfoPanel.Recalculate();
		//	}
		//	if (TrackedParticle?.width > 64f)
		//	{
		//		size.X = TrackedParticle.width;
		//		if (size.X > largestInfoSize.X)
		//			largestInfoSize.X = size.X;
		//	}
		//	if (TrackedParticle?.height > 64f)
		//	{
		//		size.Y = TrackedParticle.height;
		//		if (size.Y > largestInfoSize.Y)
		//		{
		//			largestInfoSize.Y = size.Y;
		//			InfoPanel.Height.Set(InfoPanel.Height.Pixels + (TrackedParticle.height - 64f), 0f);
		//		}
		//	}

		//	Texture2D panel = ModContent.Request<Texture2D>("ParticleLibrary/Debug/BlackPanel", AssetRequestMode.ImmediateLoad).Value;

		//	CalculatedStyle dimensions = InfoPanel.GetDimensions();
		//	Vector2 position = new(dimensions.X, dimensions.Y);

		//	int stringsDrawn = 0;
		//	float largestWidth = 192f;

		//	void DrawInfoString(string tag, string info, float scale = 0.75f, Color color = default)
		//	{
		//		if (color == default)
		//			color = new(0.45882356f, 0.49411768f, 0.7803922f);

		//		string text = $"{tag}:{info}";
		//		string tagText = $"{tag}: ";
		//		Vector2 textSize = MouseText.Value.MeasureString(text);
		//		Vector2 tagTextSize = MouseText.Value.MeasureString(tagText);

		//		spriteBatch.DrawString(MouseText.Value, $"{tag}: ", position + new Vector2(32f, 80f + (size.Y - 64f) + (stringsDrawn * 20f)), color, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
		//		spriteBatch.DrawString(MouseText.Value, $"{info}", position + new Vector2(tagTextSize.X * 0.75f, 0f) + new Vector2(32f, 80f + (size.Y - 64f) + (stringsDrawn * 20f)), Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0f);
		//		stringsDrawn++;

		//		InfoPanel.Height.Set(90f + (stringsDrawn * 20f), 0f);
		//		if (textSize.X > largestWidth)
		//			largestWidth = textSize.X;
		//	}

		//	int panelModifier = 8;
		//	if (velocityDropdown.enabled)
		//		panelModifier++;
		//	if (positionDropdown.enabled)
		//		panelModifier += 2;
		//	if (rotationDropdown.enabled)
		//		panelModifier += 2;
		//	if (scaleDropdown.enabled)
		//		panelModifier += 3;
		//	if (arrayDropdown.enabled)
		//		panelModifier += 4;
		//	if (miscDropdown.enabled)
		//		panelModifier += 6;
		//	if (entityDropdown.enabled)
		//		panelModifier += 7;

		//	DrawPanel(spriteBatch, panel, Color.White * 0.5f, position + new Vector2(10f, 80f + (size.Y - 64f)), new Vector2(InfoPanel.Width.Pixels - 20f, panelModifier * 20f), 2, 2);
		//	DrawInfoString("Time Left", TrackedParticle?.TimeLeft.ToString(), color: new(0.909804f, 0.63529414f, 0.44705886f)); // Default

		//	// Button - Velocity
		//	velocityDropdown.Top.Set(80f + (size.Y - 64f) + (stringsDrawn * 20f) - (velocityDropdown.Height.Pixels / 2f), 0f);
		//	DrawInfoString("Velocity", $"({TrackedParticle?.velocity.X:N1}, {TrackedParticle?.velocity.Y:N1})", color: new(0.909804f, 0.63529414f, 0.44705886f)); // Default
		//	if (velocityDropdown.enabled)
		//	{
		//		DrawInfoString("Velocity Acceleration", $"({TrackedParticle?.VelocityAcceleration.X:N1}, {TrackedParticle?.VelocityAcceleration.Y:N1})");
		//	}
		//	//

		//	// Button - Position
		//	positionDropdown.Top.Set(80f + (size.Y - 64f) + (stringsDrawn * 20f) - (positionDropdown.Height.Pixels / 2f), 0f);
		//	DrawInfoString("Real Pos", $"({TrackedParticle?.position.X:N1}, {TrackedParticle?.position.Y:N1})", color: new(0.909804f, 0.63529414f, 0.44705886f)); // Default
		//	if (positionDropdown.enabled)
		//	{
		//		DrawInfoString("Visual Pos", $"({TrackedParticle?.VisualPosition.X:N1}, {TrackedParticle?.VisualPosition.Y:N1})");
		//		DrawInfoString("Anchor Pos", $"({TrackedParticle?.AnchorPosition.X:N1}, {TrackedParticle?.AnchorPosition.Y:N1})");
		//	}
		//	//

		//	// Button - Rotation
		//	rotationDropdown.Top.Set(80f + (size.Y - 64f) + (stringsDrawn * 20f) - (rotationDropdown.Height.Pixels / 2f), 0f);
		//	DrawInfoString("Rotation", TrackedParticle?.Rotation.ToString(), color: new(0.909804f, 0.63529414f, 0.44705886f)); // Default
		//	if (rotationDropdown.enabled)
		//	{
		//		DrawInfoString("Rotation Velocity", TrackedParticle?.RotationVelocity.ToString());
		//		DrawInfoString("Rotation Acceleration", TrackedParticle?.RotationAcceleration.ToString());
		//	}
		//	//

		//	// Button - Scale
		//	scaleDropdown.Top.Set(80f + (size.Y - 64f) + (stringsDrawn * 20f) - (scaleDropdown.Height.Pixels / 2f), 0f);
		//	DrawInfoString("Scale", $"{TrackedParticle?.Scale:N1}", color: new(0.909804f, 0.63529414f, 0.44705886f)); // Default
		//	if (scaleDropdown.enabled)
		//	{
		//		DrawInfoString("Vector Scale", $"({TrackedParticle?.Scale2D.X:N1}, {TrackedParticle?.Scale2D.Y:N1})");
		//		DrawInfoString("Scale Velocity", $"({TrackedParticle?.ScaleVelocity.X:N1}, {TrackedParticle?.ScaleVelocity.Y:N1})");
		//		DrawInfoString("Scale Acceleration", $"({TrackedParticle?.ScaleAcceleration.X:N1}, {TrackedParticle?.ScaleAcceleration.Y:N1})");
		//	}
		//	//

		//	// Button - Arrays
		//	arrayDropdown.Top.Set(80f + (size.Y - 64f) + (stringsDrawn * 20f) - (arrayDropdown.Height.Pixels / 2f), 0f);
		//	DrawInfoString("AI", TrackedParticle?.Rotation.ToString(), color: new(0.909804f, 0.63529414f, 0.44705886f)); // Default
		//	if (arrayDropdown.enabled)
		//	{
		//		DrawInfoString("Old Pos", TrackedParticle?.RotationVelocity.ToString());
		//		DrawInfoString("Old Rot", TrackedParticle?.RotationAcceleration.ToString());
		//		DrawInfoString("Old Cen", TrackedParticle?.RotationAcceleration.ToString());
		//		DrawInfoString("Old Vel", TrackedParticle?.RotationAcceleration.ToString());
		//	}
		//	//

		//	// Button - Misc
		//	miscDropdown.Top.Set(80f + (size.Y - 64f) + (stringsDrawn * 20f) - (miscDropdown.Height.Pixels / 2f), 0f);
		//	DrawInfoString("Layer", TrackedParticle?.Layer.ToString(), color: new(0.909804f, 0.63529414f, 0.44705886f)); // Default
		//	if (miscDropdown.enabled)
		//	{
		//		DrawInfoString("Color", TrackedParticle?.Color.ToString());
		//		DrawInfoString("Frame", TrackedParticle?.Frame.ToString());
		//		DrawInfoString("Texture", TrackedParticle?.Texture != null && TrackedParticle?.Texture != ParticleLibrary.EmptyPixel ? "Loaded" : TrackedParticle?.Texture == ParticleLibrary.EmptyPixel ? "Loaded with empty pixel" : "null");
		//		DrawInfoString("Tile Collide", TrackedParticle?.TileCollide.ToString());
		//		DrawInfoString("Opacity", TrackedParticle?.Opacity.ToString("N1"));
		//		DrawInfoString("Gravity", TrackedParticle?.Gravity.ToString());
		//	}
		//	//

		//	// Button - Entity
		//	entityDropdown.Top.Set(80f + (size.Y - 64f) + (stringsDrawn * 20f) - (entityDropdown.Height.Pixels / 2f), 0f);
		//	DrawInfoString("Entity", "", color: new(0.909804f, 0.63529414f, 0.44705886f)); // Default
		//	if (entityDropdown.enabled)
		//	{
		//		DrawInfoString("Width", TrackedParticle?.width.ToString());
		//		DrawInfoString("Height", TrackedParticle?.height.ToString());
		//		DrawInfoString("Direction", TrackedParticle?.direction != null && TrackedParticle?.Texture != ParticleLibrary.EmptyPixel ? "Loaded" : TrackedParticle?.Texture == ParticleLibrary.EmptyPixel ? "Loaded with empty pixel" : "null");
		//		DrawInfoString("Wet", TrackedParticle?.wet.ToString());
		//		DrawInfoString("Honey Wet", TrackedParticle?.honeyWet.ToString());
		//		DrawInfoString("Lava Wet", TrackedParticle?.lavaWet.ToString());
		//		DrawInfoString("Wet Count", TrackedParticle?.wetCount.ToString());
		//	}
		//	//

		//	if (TrackedParticle?.width > largestWidth)
		//		largestWidth = TrackedParticle.width;
		//	if (TrackedParticle?.height > 64f)
		//		InfoPanel.Height.Set(InfoPanel.Height.Pixels + (TrackedParticle.height - 64f), 0f);

		//	if (largestWidth > largestInfoSize.X)
		//	{
		//		largestInfoSize.X = largestWidth;
		//		InfoPanel.Width.Set(largestWidth, 0f);
		//	}

		//	DrawPanel(spriteBatch, panel, Color.White * 0.5f, position + new Vector2((InfoPanel.Width.Pixels / 2f) - (size.X / 2f), 10f), size, 2, 2);

		//	if (TrackedParticle?.PreDraw(spriteBatch, position + new Vector2(InfoPanel.Width.Pixels / 2f, 42f), Color.White) == true)
		//		TrackedParticle.Draw(spriteBatch, position + new Vector2(InfoPanel.Width.Pixels / 2f, 42f), Color.White);
		//	TrackedParticle?.PostDraw(spriteBatch, position + new Vector2(InfoPanel.Width.Pixels / 2f, 42f), Color.White);

		//	clearParticle.Left.Set(InfoPanel.Width.Pixels - 20f - 18f, 0f);

		//	clearParticle.Draw(spriteBatch);
		//	velocityDropdown.Draw(spriteBatch);
		//	positionDropdown.Draw(spriteBatch);
		//	rotationDropdown.Draw(spriteBatch);
		//	scaleDropdown.Draw(spriteBatch);
		//	arrayDropdown.Draw(spriteBatch);
		//	miscDropdown.Draw(spriteBatch);
		//	entityDropdown.Draw(spriteBatch);

		//	Recalculate();

		//	if (emitterInfoButton.enabled)
		//	{
		//		for (int i = 0; i < EmitterManager.emitters.Count; i++)
		//		{
		//			Emitter emitter = EmitterManager.emitters[i];

		//			Vector2 offset = Main.MouseWorld - emitter.Center;

		//			if (offset.X > -24f && offset.X < 24f && offset.Y > -24f && offset.Y < 24f && !MainPanel.ContainsPoint(Main.MouseScreen) && !InfoPanel.ContainsPoint(Main.MouseScreen) && !DictPanel.ContainsPoint(Main.MouseScreen))
		//			{
		//				DrawHollowSquare(emitter.Center - new Vector2(24f) - Main.screenPosition, new Vector2(48f), new Color(0f, 1f, 0f, 1f), 1f);

		//				if (PlayerInput.Triggers.JustPressed.MouseLeft)
		//				{
		//					TrackedEmitter = emitter;
		//				}
		//			}

		//			Debug.Begin();
		//			Debug.DrawLine(emitter.position + new Vector2(5f, 0f) - Main.screenPosition, emitter.position + new Vector2(10f, 0f) - Main.screenPosition, 1f, new Color(0f, 1f, 0f, 1f));
		//			Debug.DrawLine(emitter.position + new Vector2(0f, 5f) - Main.screenPosition, emitter.position + new Vector2(0f, 10f) - Main.screenPosition, 1f, new Color(0f, 1f, 0f, 1f));
		//			Debug.DrawLine(emitter.position + new Vector2(-5f, 0f) - Main.screenPosition, emitter.position + new Vector2(-10f, 0f) - Main.screenPosition, 1f, new Color(0f, 1f, 0f, 1f));
		//			Debug.DrawLine(emitter.position + new Vector2(0f, -5f) - Main.screenPosition, emitter.position + new Vector2(0f, -10f) - Main.screenPosition, 1f, new Color(0f, 1f, 0f, 1f));
		//			Debug.End();
		//		}

		//		if (TrackedEmitter != null)
		//		{
		//			infoCircle.Position = TrackedEmitter.position - Main.screenPosition;
		//			infoCircle.Radius = new Vector2(5f, 5f);
		//			infoCircle.Draw();
		//			Vector2 pos = TrackedEmitter.position - Main.screenPosition;

		//			DrawPanel(spriteBatch, panel, Color.White * 0.5f, pos, new Vector2(100f, 100f), 2, 2);
		//			spriteBatch.DrawString(MouseText.Value, "Mod: " + TrackedEmitter.Assembly, pos + new Vector2(4f), Color.White, 0f, Vector2.Zero, 0.75f, SpriteEffects.None, 0f);
		//			spriteBatch.DrawString(MouseText.Value, "Type: " + TrackedEmitter.Type, pos + new Vector2(4f) + new Vector2(0f, 20f), Color.White, 0f, Vector2.Zero, 0.75f, SpriteEffects.None, 0f);
		//			spriteBatch.DrawString(MouseText.Value, "Data: " + TrackedEmitter.Data, pos + new Vector2(4f) + new Vector2(0f, 40f), Color.White, 0f, Vector2.Zero, 0.75f, SpriteEffects.None, 0f);
		//			spriteBatch.DrawString(MouseText.Value, "Save: " + TrackedEmitter.Save, pos + new Vector2(4f) + new Vector2(0f, 60f), Color.White, 0f, Vector2.Zero, 0.75f, SpriteEffects.None, 0f);
		//			spriteBatch.DrawString(MouseText.Value, "Cull: " + TrackedEmitter.CullDistance, pos + new Vector2(4f) + new Vector2(0f, 80f), Color.White, 0f, Vector2.Zero, 0.75f, SpriteEffects.None, 0f);
		//		}
		//	}
		//}
		//internal void DrawDictPanel(SpriteBatch spriteBatch)
		//{
		//	DictPanel.Left.Set(InfoPanel.Width.Pixels, 0f);
		//	DictPanel.Height.Set(InfoPanel.Height.Pixels + MainPanel.Height.Pixels, 0f);

		//	scrollBar.Left.Set(244f, 0f);
		//	scrollBar.Top.Set(6f, 0f);
		//	scrollBar.Height.Set(DictPanel.Height.Pixels - 30f, 0f);
		//	scrollBar.Width.Set(20f, 0f);

		//	cullingPanel.Top.Set(40f, 0f);
		//	cullingPanel.Height.Set(DictPanel.Height.Pixels - 60f, 0f);
		//	cullingPanel.BackgroundColor = Color.White * 0.25f;
		//	cullingPanel.BorderColor = Color.White * 0.25f;

		//	dictList.Height.Set(cullingPanel.Height.Pixels - 4f, 0f);
		//	dictList.Width.Set(cullingPanel.Width.Pixels - 4f, 0f);
		//	dictList.ListPadding = 4f;
		//	dictListHeight = dictList.GetTotalHeight();

		//	Recalculate();

		//	CalculatedStyle dimensions = DictPanel.GetDimensions();
		//	Vector2 position = new(dimensions.X, dimensions.Y);

		//	if (!dictListOrdered)
		//	{
		//		dictList._items = dictList._items.OrderBy(each => 100 - StringSimilarity((each as UICustomSelectablePanel)?.value.GetType().Assembly.GetName().Name + ": " + (each as UICustomSelectablePanel)?.value.GetType().Name, searchBar.contents)).ToList();
		//		dictListOrdered = true;
		//	}

		//	for (int i = 0; i < particles.Count; i++)
		//	{
		//		dictList.DrawSelfCustom(spriteBatch);
		//	}
		//}
		//internal void DrawCountPanel(SpriteBatch spriteBatch)
		//{
		//	countTab.Width.Set(100f, 0f);
		//	countTab.Height.Set(30f, 0f);
		//	countTab.Left.Set(InfoPanel.Left.Pixels, 0f);
		//	countTab.Top.Set(InfoPanel.Top.Pixels - 20f, 0f);
		//	countTab.BackgroundColor = Color.White;
		//	countTab.BorderColor = Color.White;

		//	CalculatedStyle dimensions = GetDimensions();
		//	Vector2 position = new(dimensions.X, dimensions.Y);

		//	spriteBatch.DrawString(MouseText.Value, CParticleSystem._particles.Count.ToString(), position - new Vector2(-10f, 15f), Color.White, 0f, Vector2.Zero, 0.75f, SpriteEffects.None, 0f);
		//}
		//internal void DrawSpawnPanel(SpriteBatch spriteBatch)
		//{
		//	SpawnPanel.Width.Set(InfoPanel.Width.Pixels + DictPanel.Width.Pixels, 0f);
		//	SpawnPanel.Height.Set(320f, 0f);
		//	SpawnPanel.Left.Set(InfoPanel.Left.Pixels, 0f);
		//	SpawnPanel.Top.Set(InfoPanel.Top.Pixels + InfoPanel.Height.Pixels + MainPanel.Height.Pixels, 0f);

		//	CalculatedStyle dimensions = SpawnPanel.GetDimensions();
		//	Vector2 position = new(dimensions.X, dimensions.Y);

		//	spawnDropper.Left.Set(SpawnPanel.Width.Pixels - 52f, 0f);
		//	spawnDropper.Top.Set(4f, 0f);
		//	spawnDropper.Width.Set(32f, 0f);
		//	spawnDropper.Height.Set(32f, 0f);

		//	squareSetting.Left.Set(spawnDropper.Left.Pixels, 0f);
		//	squareSetting.Top.Set(spawnDropper.Top.Pixels + spawnDropper.Height.Pixels + 6f, 0f);
		//	squareSetting.Width.Set(32f, 0f);
		//	squareSetting.Height.Set(32f, 0f);

		//	layerPanel.Left.Set(SpawnPanel.Left.Pixels + 70f + 10f, 0f);
		//	layerPanel.Top.Set(SpawnPanel.Top.Pixels + 160f + 10f, 0f);
		//	layerPanel.Width.Set(220f, 0f);
		//	layerPanel.Height.Set(140f, 0f);
		//	layerPanel.BackgroundColor = Color.White * 0.25f;
		//	layerPanel.BorderColor = Color.White * 0.25f;

		//	layerDropdown.Height.Set(layerPanel.Height.Pixels - 4f, 0f);
		//	layerDropdown.Width.Set(layerPanel.Width.Pixels - 44f, 0f);
		//	layerDropdown.ListPadding = 0;

		//	layerScrollbar.Left.Set(200f - 20f, 0f);
		//	layerScrollbar.Top.Set(0f, 0f);
		//	layerScrollbar.Height.Set(layerPanel.Height.Pixels, 0f);
		//	layerScrollbar.Width.Set(20f, 0f);

		//	frequency.Left.Set(70f, 0f);
		//	frequency.Top.Set(6f, 0f);

		//	amount.Left.Set(70f, 0f);
		//	amount.Top.Set(26f, 0f);

		//	velocityX.Left.Set(70f, 0f);
		//	velocityX.Top.Set(46f, 0f);

		//	velocityY.Left.Set(70f, 0f);
		//	velocityY.Top.Set(66f, 0f);

		//	velocityRandomness.Left.Set(70f, 0f);
		//	velocityRandomness.Top.Set(86f, 0f);

		//	Texture2D panel = ModContent.Request<Texture2D>("ParticleLibrary/Debug/BlackPanel", AssetRequestMode.ImmediateLoad).Value;
		//	DrawPanel(spriteBatch, panel, Color.White * 0.5f, position + new Vector2(10f), new Vector2(dimensions.Width, dimensions.Height) - new Vector2(20f), 2, 2);

		//	spawnDropper.Draw(spriteBatch);
		//	squareSetting.Draw(spriteBatch);
		//	frequency.Draw(spriteBatch);
		//	amount.Draw(spriteBatch);
		//	velocityX.Draw(spriteBatch);
		//	velocityY.Draw(spriteBatch);
		//	velocityRandomness.Draw(spriteBatch);

		//	if (spawnDropper.enabled && spawnDropperDragging)
		//	{
		//		if (!PlayerInput.Triggers.Current.MouseLeft)
		//		{
		//			spawnDropperDragging = false;
		//			spawnPosition = Main.MouseWorld;
		//			nX.SetPosition(spawnPosition, spawnPosition - new Vector2(200f, 0f));
		//			nY.SetPosition(spawnPosition, spawnPosition - new Vector2(0f, 200f));
		//		}
		//		else
		//		{
		//			CalculatedStyle spawnDropperDimensions = spawnDropper.GetDimensions();
		//			Debug.Begin();
		//			Debug.DrawLine(new Vector2(spawnDropperDimensions.X + (spawnDropper.Width.Pixels / 2f), spawnDropperDimensions.Y + (spawnDropper.Height.Pixels / 2f)), Main.MouseScreen, 1f, new Color(1f, 1f, 0f, 1f));
		//			Debug.End();
		//		}
		//	}
		//	if (spawnDropper.enabled && spawnPosition != default)
		//	{
		//		if (area == AreaType.Circle)
		//		{
		//			nX.visible = true;
		//			nY.visible = true;
		//			debugCircle.Position = spawnPosition - Main.screenPosition;
		//			debugCircle.Radius = new Vector2(nX.GetDistanceFromCenter(), nY.GetDistanceFromCenter());
		//			debugCircle.Draw();
		//		}
		//		else if (area == AreaType.Square)
		//		{
		//			nX.visible = true;
		//			nY.visible = true;
		//			DrawHollowSquare(spawnPosition - new Vector2(nX.GetDistanceFromCenter(), nY.GetDistanceFromCenter()) - Main.screenPosition, new Vector2(nX.GetDistanceFromCenter(), nY.GetDistanceFromCenter()) * 2f, new Color(0f, 1f, 0f, 1f), 1f);
		//		}
		//		else
		//		{
		//			nX.visible = false;
		//			nY.visible = false;
		//			debugCircle.Position = spawnPosition - Main.screenPosition;
		//			debugCircle.Radius = new Vector2(5f, 5f);
		//			debugCircle.Draw();
		//			Vector2 pos = spawnPosition - Main.screenPosition;
		//			Debug.Begin();
		//			Debug.DrawLine(pos + new Vector2(5f, 0f), pos + new Vector2(10f, 0f), 1f, new Color(0f, 1f, 0f, 1f));
		//			Debug.DrawLine(pos + new Vector2(0f, 5f), pos + new Vector2(0f, 10f), 1f, new Color(0f, 1f, 0f, 1f));
		//			Debug.DrawLine(pos + new Vector2(-5f, 0f), pos + new Vector2(-10f, 0f), 1f, new Color(0f, 1f, 0f, 1f));
		//			Debug.DrawLine(pos + new Vector2(0f, -5f), pos + new Vector2(0f, -10f), 1f, new Color(0f, 1f, 0f, 1f));
		//			Debug.End();
		//		}
		//	}

		//	spriteBatch.DrawString(MouseText.Value, "Frequency: ", new Vector2(position.X + frequency.Left.Pixels - 60f, position.Y + frequency.Top.Pixels + 6f), Color.White, 0f, Vector2.Zero, 0.75f, SpriteEffects.None, 0f);
		//	spriteBatch.DrawString(MouseText.Value, frequency.value.ToString(), new Vector2(position.X + frequency.Left.Pixels + frequency.Width.Pixels + 10f, position.Y + frequency.Top.Pixels + 6f), Color.White, 0f, Vector2.Zero, 0.75f, SpriteEffects.None, 0f);
		//	spriteBatch.DrawString(MouseText.Value, "Amount: ", new Vector2(position.X + amount.Left.Pixels - 60f, position.Y + amount.Top.Pixels + 6f), Color.White, 0f, Vector2.Zero, 0.75f, SpriteEffects.None, 0f);
		//	spriteBatch.DrawString(MouseText.Value, amount.value.ToString(), new Vector2(position.X + amount.Left.Pixels + amount.Width.Pixels + 10f, position.Y + amount.Top.Pixels + 6f), Color.White, 0f, Vector2.Zero, 0.75f, SpriteEffects.None, 0f);
		//	spriteBatch.DrawString(MouseText.Value, "Velocity X: ", new Vector2(position.X + velocityX.Left.Pixels - 60f, position.Y + velocityX.Top.Pixels + 6f), Color.White, 0f, Vector2.Zero, 0.75f, SpriteEffects.None, 0f);
		//	spriteBatch.DrawString(MouseText.Value, velocityX.value.ToString(), new Vector2(position.X + velocityX.Left.Pixels + velocityX.Width.Pixels + 10f, position.Y + velocityX.Top.Pixels + 6f), Color.White, 0f, Vector2.Zero, 0.75f, SpriteEffects.None, 0f);
		//	spriteBatch.DrawString(MouseText.Value, "Velocity Y: ", new Vector2(position.X + velocityY.Left.Pixels - 60f, position.Y + velocityY.Top.Pixels + 6f), Color.White, 0f, Vector2.Zero, 0.75f, SpriteEffects.None, 0f);
		//	spriteBatch.DrawString(MouseText.Value, velocityY.value.ToString(), new Vector2(position.X + velocityY.Left.Pixels + velocityY.Width.Pixels + 10f, position.Y + velocityY.Top.Pixels + 6f), Color.White, 0f, Vector2.Zero, 0.75f, SpriteEffects.None, 0f);
		//	spriteBatch.DrawString(MouseText.Value, "Velocity R: ", new Vector2(position.X + velocityRandomness.Left.Pixels - 60f, position.Y + velocityRandomness.Top.Pixels + 6f), Color.White, 0f, Vector2.Zero, 0.75f, SpriteEffects.None, 0f);
		//	spriteBatch.DrawString(MouseText.Value, velocityRandomness.value.ToString(), new Vector2(position.X + velocityRandomness.Left.Pixels + velocityRandomness.Width.Pixels + 10f, position.Y + velocityRandomness.Top.Pixels + 6f), Color.White, 0f, Vector2.Zero, 0.75f, SpriteEffects.None, 0f);

		//	spriteBatch.DrawString(MouseText.Value, "AI: ", new Vector2(position.X + velocityRandomness.Left.Pixels - 60f, position.Y + velocityRandomness.Top.Pixels + 6f + 20f), Color.White, 0f, Vector2.Zero, 0.75f, SpriteEffects.None, 0f);
		//	ai0.Left.Set(70f, 0f);
		//	ai0.Top.Set(100f, 0f);
		//	ai1.Left.Set(114f, 0f);
		//	ai1.Top.Set(100f, 0f);
		//	ai2.Left.Set(158f, 0f);
		//	ai2.Top.Set(100f, 0f);
		//	ai3.Left.Set(202f, 0f);
		//	ai3.Top.Set(100f, 0f);
		//	ai4.Left.Set(246f, 0f);
		//	ai4.Top.Set(100f, 0f);
		//	ai5.Left.Set(290f, 0f);
		//	ai5.Top.Set(100f, 0f);
		//	ai6.Left.Set(334f, 0f);
		//	ai6.Top.Set(100f, 0f);
		//	ai7.Left.Set(378f, 0f);
		//	ai7.Top.Set(100f, 0f);

		//	spriteBatch.DrawString(MouseText.Value, "Color: ", new Vector2(position.X + velocityRandomness.Left.Pixels - 60f, position.Y + velocityRandomness.Top.Pixels + 6f + 40f), Color.White, 0f, Vector2.Zero, 0.75f, SpriteEffects.None, 0f);
		//	colorR.Left.Set(70f, 0f);
		//	colorR.Top.Set(120f, 0f);
		//	colorG.Left.Set(154f, 0f);
		//	colorG.Top.Set(120f, 0f);
		//	colorB.Left.Set(238f, 0f);
		//	colorB.Top.Set(120f, 0f);
		//	colorA.Left.Set(322f, 0f);
		//	colorA.Top.Set(120f, 0f);

		//	spriteBatch.DrawString(MouseText.Value, "Scale: ", new Vector2(position.X + velocityRandomness.Left.Pixels - 60f, position.Y + velocityRandomness.Top.Pixels + 6f + 60f), Color.White, 0f, Vector2.Zero, 0.75f, SpriteEffects.None, 0f);
		//	scaleX.Left.Set(70f, 0f);
		//	scaleX.Top.Set(140f, 0f);
		//	scaleY.Left.Set(234f, 0f);
		//	scaleY.Top.Set(140f, 0f);

		//	spriteBatch.DrawString(MouseText.Value, "Layer: ", new Vector2(position.X + velocityRandomness.Left.Pixels - 60f, position.Y + velocityRandomness.Top.Pixels + 6f + 90f), Color.White, 0f, Vector2.Zero, 0.75f, SpriteEffects.None, 0f);

		//	layerPanel.Draw(spriteBatch);
		//	layerDropdown.DrawSelfCustom(spriteBatch);
		//}
		//#endregion

		//#region Dragging
		//internal void DragCalculation()
		//{
		//	if (MainPanel.ContainsPoint(Main.MouseScreen))
		//	{
		//		Main.LocalPlayer.mouseInterface = true;
		//	}
		//	if (InfoPanel.ContainsPoint(Main.MouseScreen))
		//	{
		//		Main.LocalPlayer.mouseInterface = true;
		//	}
		//	if (DictPanel.ContainsPoint(Main.MouseScreen))
		//	{
		//		Main.LocalPlayer.mouseInterface = true;
		//	}

		//	if (Dragging)
		//	{
		//		Left.Set(Main.mouseX - offset.X, 0f);
		//		Top.Set(Main.mouseY - offset.Y, 0f);
		//		Recalculate();
		//	}

		//	var parentSpace = new Rectangle(0, 0, Main.screenWidth, Main.screenHeight);

		//	Left.Pixels = Utils.Clamp(Left.Pixels, 0, parentSpace.Right - InfoPanel.Width.Pixels);
		//	Top.Pixels = Utils.Clamp(Top.Pixels, 0, parentSpace.Bottom - InfoPanel.Height.Pixels - MainPanel.Height.Pixels);

		//	Recalculate();
		//}
		//public override void LeftMouseDown(UIMouseEvent evt)
		//{
		//	if (!Visible)
		//		return;

		//	if ((InfoPanel.ContainsPoint(Main.MouseScreen) || MainPanel.ContainsPoint(Main.MouseScreen)) && !scrollBar.ContainsPoint(Main.MouseScreen))
		//	{
		//		base.LeftMouseDown(evt);
		//		DragStart(evt);
		//	}
		//}
		//public override void LeftMouseUp(UIMouseEvent evt)
		//{
		//	if (!Visible)
		//		return;

		//	if ((InfoPanel.ContainsPoint(Main.MouseScreen) || MainPanel.ContainsPoint(Main.MouseScreen)) && !scrollBar.ContainsPoint(Main.MouseScreen))
		//	{
		//		base.LeftMouseUp(evt);
		//		DragEnd(evt);
		//	}
		//}
		//private void DragStart(UIMouseEvent evt)
		//{
		//	offset = new Vector2(evt.MousePosition.X - Left.Pixels, evt.MousePosition.Y - Top.Pixels);
		//	Dragging = true;
		//}
		//private void DragEnd(UIMouseEvent evt)
		//{
		//	Vector2 end = evt.MousePosition;
		//	Dragging = false;

		//	Left.Set(end.X - offset.X, 0f);
		//	Top.Set(end.Y - offset.Y, 0f);

		//	Recalculate();
		//}
		//#endregion

		//#region Debugging
		//internal void DrawDebug(SpriteBatch spriteBatch)
		//{
		//	DrawHollowSquare(new Vector2(Left.Pixels, Top.Pixels), new Vector2(Width.Pixels, Height.Pixels), Color.Green, 1f);

		//	foreach (var child in Children)
		//	{
		//		DrawHollowSquare(new Vector2(child.Left.Pixels, child.Top.Pixels), new Vector2(child.Width.Pixels, child.Height.Pixels), Color.Green, 1f);
		//	}
		//}
		//internal void DrawParticleDebug(SpriteBatch spriteBatch)
		//{
		//	for (int i = 0; i < CParticleSystem._particles.Count; i++)
		//	{
		//		Particle particle = CParticleSystem._particles[i];

		//		if (velocityButton.enabled)
		//		{
		//			Vector2 modifier = new(particle.velocity.X * 16f, particle.velocity.Y * 16f);
		//			DrawArrow(particle.Center - Main.screenPosition, particle.Center + modifier - Main.screenPosition, new Color(0f, 1f, 0f, 0f), 2f);
		//			spriteBatch.DrawString(MouseText.Value, $"({particle.velocity.X:N1}, {particle.velocity.Y:N1})", particle.Center + (modifier * 0.5f) - Main.screenPosition, Color.White, 0f, Vector2.Zero, 0.75f, SpriteEffects.None, 0f);
		//		}

		//		if (hitboxButton.enabled)
		//		{
		//			Debug.DrawSquare(particle.position - Main.screenPosition, new Vector2(particle.width, particle.height), Color.Red * 0.5f);
		//		}

		//		Vector2 offset = Main.MouseWorld - particle.Center;

		//		if (offset.X > -24f && offset.X < 24f && offset.Y > -24f && offset.Y < 24f && !MainPanel.ContainsPoint(Main.MouseScreen) && !InfoPanel.ContainsPoint(Main.MouseScreen) && !DictPanel.ContainsPoint(Main.MouseScreen))
		//		{
		//			DrawHollowSquare(particle.Center - new Vector2(24f) - Main.screenPosition, new Vector2(48f), new Color(1f, 1f, 0f, 1f), 1f);

		//			if (PlayerInput.Triggers.JustPressed.MouseLeft)
		//			{
		//				TrackedParticle = particle;
		//			}
		//		}
		//	}
		//}
		//internal void DrawArrow(Vector2 start, Vector2 end, Color color, float width)
		//{
		//	Debug.Begin();
		//	Debug.DrawLine(start, end, width, color);
		//	Debug.DrawLine(end, end + new Vector2(16f, 0f).RotatedBy(start.AngleTo(end)).RotatedBy(MathHelper.Pi + MathHelper.PiOver4), width, color);
		//	Debug.DrawLine(end, end + new Vector2(16f, 0f).RotatedBy(start.AngleTo(end)).RotatedBy(MathHelper.Pi - MathHelper.PiOver4), width, color);
		//	Debug.End();
		//}
		//internal void DrawHollowSquare(Vector2 position, Vector2 size, Color color, float width)
		//{
		//	Debug.Begin();
		//	Debug.DrawLine(position, position + new Vector2(size.X, 0f), width, color);
		//	Debug.DrawLine(position + new Vector2(size.X, 0f), position + size, width, color);
		//	Debug.DrawLine(position + size, position + new Vector2(0f, size.Y), width, color);
		//	Debug.DrawLine(position + new Vector2(0f, size.Y), position, width, color);
		//	Debug.End();
		//}
		//internal Texture2D DrawCircle(int radius)
		//{
		//	Texture2D texture = new(Main.graphics.GraphicsDevice, radius, radius);
		//	Color[] colorData = new Color[radius * radius];

		//	float halfRadius = radius / 2f;
		//	float halfRadiusSquared = halfRadius * halfRadius;

		//	for (int x = 0; x < radius; x++)
		//	{
		//		for (int y = 0; y < radius; y++)
		//		{
		//			int index = (x * radius) + y;
		//			Vector2 pos = new(x - halfRadius, y - halfRadius);
		//			if (pos.LengthSquared() <= halfRadiusSquared)
		//			{
		//				colorData[index] = Color.White;
		//			}
		//			else
		//			{
		//				colorData[index] = Color.Transparent;
		//			}
		//		}
		//	}

		//	texture.SetData(colorData);
		//	return texture;
		//}
		//#endregion

		//#region Panel
		//internal void DrawPanel(SpriteBatch spriteBatch, Texture2D texture, Color color, Vector2 position, Vector2 size, int cornerSize, int barSize, Rectangle clippingRectangle = default)
		//{
		//	spriteBatch.End();
		//	spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.NonPremultiplied, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.UIScaleMatrix);

		//	CullMode cull = spriteBatch.GraphicsDevice.RasterizerState.CullMode;
		//	Rectangle rectangle = spriteBatch.GraphicsDevice.ScissorRectangle;

		//	if (clippingRectangle != default)
		//	{
		//		spriteBatch.GraphicsDevice.RasterizerState.CullMode = CullMode.None;
		//		spriteBatch.GraphicsDevice.RasterizerState.ScissorTestEnable = true;
		//		spriteBatch.GraphicsDevice.ScissorRectangle = clippingRectangle;
		//	}

		//	Point point = new((int)position.X, (int)position.Y);
		//	Point point2 = new(point.X + (int)size.X - cornerSize, point.Y + (int)size.Y - cornerSize);
		//	int width = point2.X - point.X - cornerSize;
		//	int height = point2.Y - point.Y - cornerSize;
		//	spriteBatch.Draw(texture, new Rectangle(point.X, point.Y, cornerSize, cornerSize), new Rectangle(0, 0, cornerSize, cornerSize), color);
		//	spriteBatch.Draw(texture, new Rectangle(point2.X, point.Y, cornerSize, cornerSize), new Rectangle(cornerSize + barSize, 0, cornerSize, cornerSize), color);
		//	spriteBatch.Draw(texture, new Rectangle(point.X, point2.Y, cornerSize, cornerSize), new Rectangle(0, cornerSize + barSize, cornerSize, cornerSize), color);
		//	spriteBatch.Draw(texture, new Rectangle(point2.X, point2.Y, cornerSize, cornerSize), new Rectangle(cornerSize + barSize, cornerSize + barSize, cornerSize, cornerSize), color);
		//	spriteBatch.Draw(texture, new Rectangle(point.X + cornerSize, point.Y, width, cornerSize), new Rectangle(cornerSize, 0, barSize, cornerSize), color);
		//	spriteBatch.Draw(texture, new Rectangle(point.X + cornerSize, point2.Y, width, cornerSize), new Rectangle(cornerSize, cornerSize + barSize, barSize, cornerSize), color);
		//	spriteBatch.Draw(texture, new Rectangle(point.X, point.Y + cornerSize, cornerSize, height), new Rectangle(0, cornerSize, cornerSize, barSize), color);
		//	spriteBatch.Draw(texture, new Rectangle(point2.X, point.Y + cornerSize, cornerSize, height), new Rectangle(cornerSize + barSize, cornerSize, cornerSize, barSize), color);
		//	spriteBatch.Draw(texture, new Rectangle(point.X + cornerSize, point.Y + cornerSize, width, height), new Rectangle(cornerSize, cornerSize, barSize, barSize), color);

		//	spriteBatch.End();
		//	spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.None, RasterizerState.CullCounterClockwise, null, Main.UIScaleMatrix);

		//	if (clippingRectangle != default)
		//	{
		//		spriteBatch.GraphicsDevice.RasterizerState.CullMode = cull;
		//		spriteBatch.GraphicsDevice.RasterizerState.ScissorTestEnable = false;
		//		spriteBatch.GraphicsDevice.ScissorRectangle = rectangle;
		//	}
		//}
		//#endregion

		//#region Dictionary
		//public List<Particle> GetAllParticles()
		//{
		//	Type baseType = typeof(Particle);
		//	List<Type> list = new();
		//	List<Particle> result = new();

		//	Asset<Texture2D> box = ModContent.Request<Texture2D>("ParticleLibrary/Debug/Box", AssetRequestMode.ImmediateLoad);
		//	Asset<Texture2D> background = ModContent.Request<Texture2D>("ParticleLibrary/Debug/BoxBackground", AssetRequestMode.ImmediateLoad);

		//	for (int i = 0; i < ModLoader.Mods.Length; i++)
		//	{
		//		Mod mod = ModLoader.Mods[i];
		//		Assembly assembly = mod.Code;

		//		list.AddRange(assembly.GetTypes().Where(t => t.IsSubclassOf(baseType)));
		//	}

		//	for (int i = 0; i < list.Count; i++)
		//	{
		//		Type type = list[i];

		//		Particle particle = Activator.CreateInstance(type) as Particle;

		//		Color color = i % 2 == 0 ? new(0.909804f, 0.63529414f, 0.44705886f) : new(0.45882356f, 0.49411768f, 0.7803922f);

		//		UICustomSelectablePanel panel = new(background, box, 8, 2, particle, color, particle.GetType().Assembly.GetName().Name + ": " + particle.GetType().Name);
		//		panel.Width.Set(234f, 0f);
		//		panel.Height.Set(26f, 0f);
		//		panel.BackgroundColor = Color.White * 0.25f;
		//		panel.BorderColor = Color.White * 0.25f;
		//		panel.selected = false;

		//		DictPanel.Append(panel);
		//		dictList.Add(panel);

		//		result.Add(particle);
		//	}

		//	return result;
		//}
		//internal static int LevenshteinDistance(string s, string t)
		//{
		//	int length = s.Length;
		//	int length2 = t.Length;
		//	int[,] array = new int[length + 1, length2 + 1];
		//	if (length == 0)
		//	{
		//		return length2;
		//	}

		//	if (length2 == 0)
		//	{
		//		return length;
		//	}

		//	int num = 0;
		//	while (num <= length)
		//	{
		//		array[num, 0] = num++;
		//	}

		//	int num2 = 0;
		//	while (num2 <= length2)
		//	{
		//		array[0, num2] = num2++;
		//	}

		//	for (int i = 1; i <= length; i++)
		//	{
		//		for (int j = 1; j <= length2; j++)
		//		{
		//			int num3 = ((t[j - 1] != s[i - 1]) ? 2 : 0);
		//			array[i, j] = Math.Min(Math.Min(array[i - 1, j] + 2, array[i, j - 1] + 2), array[i - 1, j - 1] + num3);
		//		}
		//	}

		//	return array[length, length2];
		//}
		//internal static int StringSimilarity(string x, string y)
		//{
		//	if (x == null || y == null)
		//		return 0;
		//	double maxLength = Math.Max(x.Length, y.Length);
		//	if (maxLength > 0)
		//	{
		//		int percentage = (int)((maxLength - LevenshteinDistance(x.ToLower(), y.ToLower())) / maxLength * 100);

		//		string x1 = x.Replace(" ", "").Split(":")[0];
		//		string x2 = x.Replace(" ", "").Split(":")[1];

		//		if (x1.Contains(y, StringComparison.OrdinalIgnoreCase))
		//			percentage += 50;
		//		if (x2.Contains(y, StringComparison.OrdinalIgnoreCase))
		//			percentage += 10;

		//		return percentage;
		//	}
		//	return (int)(1.0 * 100);
		//}
		//#endregion

		//#region Classes
		//internal class UICustomImageButton : UIColoredImageButton
		//{
		//	public Texture2D texture;
		//	public Texture2D tick;
		//	public Texture2D noTick;
		//	public Texture2D highlight;

		//	public bool hovered;
		//	public bool enabled;
		//	public bool visible;

		//	public UICustomImageButton(Asset<Texture2D> texture, Asset<Texture2D> tick, Asset<Texture2D> highlight, Asset<Texture2D> noTick = null, bool isSmall = false) : base(texture, isSmall)
		//	{
		//		this.texture = texture.Value;
		//		this.tick = tick.Value;
		//		this.highlight = highlight.Value;
		//		this.noTick = noTick?.Value;

		//		OnLeftClick += OnButtonClick;
		//	}

		//	protected override void DrawSelf(SpriteBatch spriteBatch)
		//	{
		//		if (!visible)
		//			return;

		//		CalculatedStyle dimensions = GetDimensions();
		//		Vector2 position = new(dimensions.X, dimensions.Y);

		//		UISystem.Instance.DebugUIElement.Instance.DrawPanel(spriteBatch, texture, Color.White, position, new Vector2(Width.Pixels, Height.Pixels), 8, 2);
		//		if (enabled)
		//			spriteBatch.Draw(tick, position + new Vector2(Width.Pixels / 2f, Height.Pixels / 2f), null, Color.White, 0f, tick.Size() * 0.5f, 1f, SpriteEffects.None, 0f);
		//		if (!enabled && noTick != null)
		//			spriteBatch.Draw(noTick, position + new Vector2(Width.Pixels / 2f, Height.Pixels / 2f), null, Color.White, 0f, noTick.Size() * 0.5f, 1f, SpriteEffects.None, 0f);
		//		if (hovered)
		//			UISystem.Instance.DebugUIElement.Instance.DrawHollowSquare(position - new Vector2(2f), new Vector2(Width.Pixels + 4f, Height.Pixels + 4f), new Color(1f, 1f, 0f, 1f), 2f);
		//	}

		//	public void SetState(bool state)
		//	{
		//		this.enabled = state;
		//	}

		//	private void OnButtonClick(UIMouseEvent evt, UIElement listeningElement)
		//	{
		//		enabled = !enabled;
		//		SoundEngine.PlaySound(SoundID.MenuTick);
		//	}

		//	public override void MouseOver(UIMouseEvent evt)
		//	{
		//		base.MouseOver(evt);
		//		SoundEngine.PlaySound(SoundID.MenuTick);
		//		hovered = true;
		//	}

		//	public override void MouseOut(UIMouseEvent evt)
		//	{
		//		base.MouseOut(evt);
		//		hovered = false;
		//	}
		//}
		//internal class UICustomScrollBar : UIScrollbar
		//{
		//	public Texture2D bar;
		//	public Texture2D handle;

		//	public Rectangle handleRect;
		//	public float offset;
		//	public float mouseOffset;
		//	public bool dragging;

		//	public UICustomScrollBar(Texture2D bar, Texture2D handle)
		//	{
		//		this.bar = bar;
		//		this.handle = handle;
		//	}

		//	protected override void DrawSelf(SpriteBatch spriteBatch)
		//	{
		//		if (UISystem.Instance.DebugUIElement.Instance.dictList.ContainsPoint(Main.MouseScreen))
		//			PlayerInput.LockVanillaMouseScroll("ParticleLibrary/UICustomScrollBar");

		//		CalculatedStyle dimensions = GetDimensions();
		//		Vector2 position = new(dimensions.X, dimensions.Y);

		//		MethodInfo method = GetType().BaseType.GetMethod("GetHandleRectangle", BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public);
		//		handleRect = (Rectangle)Convert.ChangeType(method.Invoke(this, null), typeof(Rectangle));

		//		if (!PlayerInput.Triggers.Current.MouseLeft)
		//			dragging = false;

		//		if (dragging)
		//		{
		//			float screenOffset = Main.MouseScreen.Y - offset;

		//			ViewPosition = UISystem.Instance.DebugUIElement.Instance.dictListHeight * MathHelper.Lerp(0f, 1f, screenOffset / Height.Pixels);

		//			if (!PlayerInput.Triggers.Current.MouseLeft)
		//				offset = 0f;
		//		}

		//		UISystem.Instance.DebugUIElement.Instance.DrawPanel(spriteBatch, bar, Color.White, position, new Vector2(dimensions.Width, dimensions.Height), 8, 2);
		//		UISystem.Instance.DebugUIElement.Instance.DrawPanel(spriteBatch, handle, Color.White, new Vector2(handleRect.X, handleRect.Y), new Vector2(handleRect.Width, handleRect.Height), 4, 2);
		//	}

		//	public override void LeftMouseDown(UIMouseEvent evt)
		//	{
		//		CalculatedStyle dimensions = GetDimensions();
		//		Vector2 position = new(dimensions.X, dimensions.Y);

		//		if (handleRect.Contains((int)Main.MouseScreen.X, (int)Main.MouseScreen.Y))
		//		{
		//			dragging = true;

		//			float handleTop = handleRect.Y + 4f;
		//			mouseOffset = Main.MouseScreen.Y >= handleTop && Main.MouseScreen.Y <= handleTop + (handleRect.Height - 4f) ? Main.MouseScreen.Y - handleTop : 0f;

		//			offset = position.Y + mouseOffset;
		//		}
		//	}

		//	public override void LeftMouseUp(UIMouseEvent evt)
		//	{
		//		if (handleRect.Contains((int)Main.MouseScreen.X, (int)Main.MouseScreen.Y))
		//		{
		//			dragging = false;
		//		}
		//	}

		//	public override void MouseOver(UIMouseEvent evt)
		//	{
		//		PlayerInput.LockVanillaMouseScroll("ParticleLibrary/UICustomScrollBar");
		//	}
		//}
		//internal class UICustomSelectablePanel : UIPanel
		//{
		//	public object value;
		//	public Color selectedColor;
		//	public bool hovered;
		//	public bool selected;
		//	public string text;

		//	public UICustomSelectablePanel(Asset<Texture2D> background, Asset<Texture2D> border, int cornerSize, int barSize, object value, Color selectedColor, string text) : base(background, border, cornerSize, barSize)
		//	{
		//		this.value = value;
		//		this.selectedColor = selectedColor;
		//		this.text = text;
		//	}

		//	protected override void DrawSelf(SpriteBatch spriteBatch)
		//	{
		//		base.DrawSelf(spriteBatch);

		//		CalculatedStyle dimensions = GetDimensions();
		//		Vector2 position = new(dimensions.X, dimensions.Y);

		//		if (hovered)
		//			UISystem.Instance.DebugUIElement.Instance.DrawHollowSquare(position - new Vector2(2f), new Vector2(Width.Pixels + 4f, Height.Pixels + 4f), new Color(1f, 1f, 0f, 1f) * 0.25f, 2f);

		//		spriteBatch.DrawString(MouseText.Value, text, position + new Vector2(6f), Color.White, 0f, Vector2.Zero, 0.75f, SpriteEffects.None, 0f);
		//	}

		//	public override void LeftMouseDown(UIMouseEvent evt)
		//	{
		//		base.LeftMouseDown(evt);

		//		selected = !selected;

		//		UISystem.Instance.DebugUIElement.Instance.dictListOrdered = false;

		//		if (!selected && value is Particle)
		//		{
		//			BackgroundColor = Color.White * 0.25f;
		//			BorderColor = Color.White * 0.25f;
		//			UISystem.Instance.DebugUIElement.Instance.selectedParticles.Remove(value as Particle);
		//		}
		//		else if (selected && value is Particle)
		//		{
		//			BackgroundColor = Color.White;
		//			BorderColor = Color.White;
		//			UISystem.Instance.DebugUIElement.Instance.selectedParticles.Add(value as Particle);
		//		}
		//	}

		//	public override void MouseOver(UIMouseEvent evt)
		//	{
		//		base.MouseOver(evt);
		//		SoundEngine.PlaySound(SoundID.MenuTick);
		//		hovered = true;
		//	}

		//	public override void MouseOut(UIMouseEvent evt)
		//	{
		//		base.MouseOut(evt);
		//		hovered = false;
		//	}
		//}
		//internal class UICustomList : UIList
		//{
		//	public void DrawSelfCustom(SpriteBatch spriteBatch)
		//	{
		//		DrawSelf(spriteBatch);

		//		if (ContainsPoint(Main.MouseScreen))
		//			PlayerInput.LockVanillaMouseScroll("ModLoader/UIScrollbar");
		//	}

		//	public override void ScrollWheel(UIScrollWheelEvent evt)
		//	{
		//		if (_scrollbar != null)
		//			_scrollbar.ViewPosition -= 20 * Math.Sign(evt.ScrollWheelValue);
		//	}
		//}
		//internal class DraggablePoint : UIColoredImageButton
		//{
		//	public Texture2D pixel;
		//	public Vector2 center;
		//	public Vector2 position;
		//	public Vector2 origin;
		//	public bool locked;
		//	public bool dragging;
		//	public bool horizontal;
		//	public bool vertical;
		//	public bool hovered;
		//	public bool visible;

		//	public DraggablePoint(Vector2 center, bool locked, bool horizontal, bool vertical, Asset<Texture2D> texture = null, bool isSmall = false) : base(texture, isSmall)
		//	{
		//		pixel = ModContent.Request<Texture2D>("ParticleLibrary/Debug/WhitePixel", AssetRequestMode.ImmediateLoad).Value;
		//		this.center = center;
		//		this.locked = locked;
		//		this.horizontal = horizontal;
		//		this.vertical = vertical;

		//		Width.Set(12f, 0f);
		//		Height.Set(12f, 0f);
		//	}

		//	public override void Draw(SpriteBatch spriteBatch)
		//	{
		//		if (visible)
		//		{
		//			if (dragging && !locked)
		//			{
		//				dragging = PlayerInput.Triggers.Current.MouseLeft;

		//				Vector2 offset = origin - Main.MouseWorld;

		//				if (horizontal)
		//				{
		//					this.position.X -= offset.X;
		//					offset.X = 0f;
		//				}
		//				if (vertical)
		//				{
		//					this.position.Y -= offset.Y;
		//					offset.Y = 0f;
		//				}

		//				origin = Main.MouseWorld;

		//				if (!dragging)
		//				{
		//					origin = Vector2.Zero;
		//				}
		//			}

		//			CalculatedStyle dimensions = GetDimensions();
		//			Vector2 position = new(dimensions.X, dimensions.Y);
		//			Vector2 modifier = new(position.X - Left.Pixels, position.Y - Top.Pixels);

		//			Left.Set(this.position.X - Main.screenPosition.X - modifier.X - 6f, 0f);
		//			Top.Set(this.position.Y - Main.screenPosition.Y - modifier.Y - 6f, 0f);

		//			Recalculate();

		//			spriteBatch.Draw(pixel, position, new Rectangle(0, 0, (int)Width.Pixels, (int)Height.Pixels), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
		//		}
		//	}

		//	public override void LeftMouseDown(UIMouseEvent evt)
		//	{
		//		if (visible)
		//		{
		//			base.LeftMouseDown(evt);
		//			dragging = true;
		//			origin = Main.MouseWorld;
		//		}
		//	}

		//	public override void MouseOver(UIMouseEvent evt)
		//	{
		//		if (visible)
		//			hovered = true;
		//	}

		//	public override void MouseOut(UIMouseEvent evt)
		//	{
		//		if (visible)
		//			hovered = false;
		//	}

		//	public float GetDistanceFromCenter()
		//	{
		//		return position.Distance(center);
		//	}

		//	public void SetPosition(Vector2 center, Vector2 position)
		//	{
		//		this.center = center;
		//		this.position = position;
		//	}
		//}
		//internal class UICustomSlider : UIColoredSliderSimple
		//{
		//	public Texture2D slider;
		//	public Texture2D handle;
		//	public Rectangle handleRect;
		//	public Vector2 origin;
		//	public Vector2 modifier;
		//	public int min;
		//	public int max;
		//	public int value;
		//	public int defaultValue;
		//	public bool dragging;
		//	public bool defaultValueSet;
		//	public bool defaultUntouched;

		//	public UICustomSlider(int min, int max, int defaultValue)
		//	{
		//		slider = ModContent.Request<Texture2D>("ParticleLibrary/Debug/Scrollbar", AssetRequestMode.ImmediateLoad).Value;
		//		handle = ModContent.Request<Texture2D>("ParticleLibrary/Debug/Scrollbutton", AssetRequestMode.ImmediateLoad).Value;
		//		this.min = min;
		//		this.max = max;
		//		this.defaultValue = defaultValue;
		//		defaultUntouched = true;
		//		value = defaultValue;
		//		FillPercent = 1f / MathHelper.Distance(min, max) * defaultValue;
		//	}

		//	protected override void DrawSelf(SpriteBatch spriteBatch)
		//	{
		//		if (!defaultValueSet)
		//		{
		//			value = defaultValue;
		//			FillPercent = 1f / MathHelper.Distance(min, max) * defaultValue;
		//			CalculatedStyle d = GetDimensions();
		//			modifier.X = (int)((d.Width - handleRect.Width) * FillPercent);
		//			defaultValueSet = true;
		//		}

		//		if (dragging)
		//		{
		//			dragging = PlayerInput.Triggers.Current.MouseLeft;

		//			Vector2 offset = origin - Main.MouseScreen;
		//			modifier.X = -(int)offset.X;
		//		}

		//		base.DrawSelf(spriteBatch);

		//		CalculatedStyle dimensions = GetDimensions();
		//		Vector2 position = new(dimensions.X, dimensions.Y);

		//		UISystem.Instance.DebugUIElement.Instance.DrawPanel(spriteBatch, slider, Color.White, position, new Vector2(Width.Pixels, Height.Pixels), 8, 2);

		//		Vector2 handlePosition = new(dimensions.X + 5f, dimensions.Y - 2f);
		//		handleRect = new Rectangle((int)MathHelper.Clamp(handlePosition.X + modifier.X, position.X, position.X + dimensions.Width - handleRect.Width), (int)handlePosition.Y, (int)Height.Pixels + 4, (int)Height.Pixels + 4);

		//		UISystem.Instance.DebugUIElement.Instance.DrawPanel(spriteBatch, handle, Color.White, new Vector2(handleRect.X, handleRect.Y), new Vector2(Height.Pixels + 4f, Height.Pixels + 4f), 4, 2);

		//		FillPercent = MathHelper.Lerp(0f, 1f, (handleRect.X - position.X) / (dimensions.Width - handleRect.Width));

		//		if (defaultUntouched)
		//			value = defaultValue;
		//		else
		//			value = (int)MathHelper.Lerp(min, max, FillPercent);
		//	}

		//	public override void LeftMouseDown(UIMouseEvent evt)
		//	{
		//		base.LeftMouseDown(evt);

		//		if (handleRect.Contains(new Point((int)Main.MouseScreen.X, (int)Main.MouseScreen.Y)))
		//		{
		//			dragging = true;
		//			defaultUntouched = false;
		//			CalculatedStyle dimensions = GetDimensions();
		//			origin = new(dimensions.X + 10f, dimensions.Y);
		//		}
		//	}
		//}
		//internal class UICustomSearchBar : UIElement
		//{
		//	public UITextBox textBox;
		//	public string emptyText;
		//	public string contents;
		//	public float textScale;
		//	public bool isWritingText;
		//	public Func<string, string> textChanged;

		//	public UICustomSearchBar(string emptyContentText, float scale, string defaultContents)
		//	{
		//		emptyText = emptyContentText;
		//		contents = defaultContents;
		//		textScale = scale;
		//		UITextBox uITextBox = new("", scale)
		//		{
		//			HAlign = 0f,
		//			VAlign = 0.5f,
		//			BackgroundColor = Color.Transparent,
		//			BorderColor = Color.Transparent,
		//			Width = new StyleDimension(0f, 1f),
		//			Height = new StyleDimension(0f, 1f),
		//			TextHAlign = 0f,
		//			ShowInputTicker = false
		//		};
		//		uITextBox.SetTextMaxLength(500);
		//		Append(uITextBox);
		//		textBox = uITextBox;
		//		uITextBox.SetText(defaultContents);
		//	}

		//	public override void Update(GameTime gameTime)
		//	{
		//		if (Main.keyState.IsKeyDown(Keys.Escape))
		//		{
		//			isWritingText = false;
		//			textBox.ShowInputTicker = isWritingText;
		//			Main.clrInput();
		//		}

		//		if (PlayerInput.Triggers.JustPressed.MouseLeft && !ContainsPoint(Main.MouseScreen))
		//		{
		//			isWritingText = false;
		//			textBox.ShowInputTicker = isWritingText;
		//			Main.clrInput();
		//		}

		//		if (isWritingText)
		//		{
		//			PlayerInput.WritingText = true;
		//			textBox.ShowInputTicker = true;
		//			Main.CurrentInputTextTakerOverride = this;
		//		}

		//		base.Update(gameTime);
		//	}

		//	protected override void DrawSelf(SpriteBatch spriteBatch)
		//	{
		//		base.DrawSelf(spriteBatch);

		//		CalculatedStyle dimensions = GetDimensions();
		//		Vector2 pos = new(dimensions.X, dimensions.Y);

		//		Texture2D panel = ModContent.Request<Texture2D>("ParticleLibrary/Debug/BlackPanel", AssetRequestMode.ImmediateLoad).Value;
		//		UISystem.Instance.DebugUIElement.Instance.DrawPanel(spriteBatch, panel, Color.White * 0.5f, pos, new Vector2(Width.Pixels, Height.Pixels), 2, 2);

		//		if (ContainsPoint(Main.MouseScreen))
		//			UISystem.Instance.DebugUIElement.Instance.DrawHollowSquare(pos - new Vector2(2f), new Vector2(Width.Pixels + 4f, Height.Pixels + 4f), new Color(1f, 1f, 0f, 1f) * 0.25f, 2f);

		//		if (!isWritingText)
		//		{
		//			return;
		//		}

		//		PlayerInput.WritingText = true;
		//		Main.instance.HandleIME();
		//		Vector2 position = new(Main.screenWidth / 2, textBox.GetDimensions().ToRectangle().Bottom + 32);
		//		Main.instance.DrawWindowsIMEPanel(position, 0.5f);
		//		string inputText = Main.GetInputText(contents);
		//		if (Main.inputTextEnter)
		//		{
		//			ToggleTakingText();
		//		}
		//		else if (Main.inputTextEscape)
		//		{
		//			ToggleTakingText();
		//		}

		//		SetContents(inputText);
		//		position = new Vector2(Main.screenWidth / 2, textBox.GetDimensions().ToRectangle().Bottom + 32);
		//		Main.instance.DrawWindowsIMEPanel(position, 0.5f);
		//	}

		//	public override void LeftMouseDown(UIMouseEvent evt)
		//	{
		//		base.LeftMouseDown(evt);

		//		isWritingText = true;
		//	}

		//	public override void MouseOver(UIMouseEvent evt)
		//	{
		//		base.MouseOver(evt);
		//		SoundEngine.PlaySound(SoundID.MenuTick);
		//	}

		//	public void SetContents(string contents)
		//	{
		//		if (textChanged == null)
		//			UISystem.Instance.DebugUIElement.Instance.dictListOrdered = false;

		//		if (textChanged != null)
		//			contents = textChanged(contents);

		//		if (this.contents != contents)
		//		{
		//			this.contents = contents;
		//			if (string.IsNullOrEmpty(this.contents))
		//			{
		//				textBox.TextColor = Color.Gray;
		//				textBox.SetText(emptyText, textScale, large: false);
		//			}
		//			else
		//			{
		//				textBox.TextColor = Color.White;
		//				textBox.SetText(this.contents);
		//				this.contents = textBox.Text;
		//			}
		//		}
		//	}

		//	public void ToggleTakingText()
		//	{
		//		isWritingText = !isWritingText;
		//		textBox.ShowInputTicker = isWritingText;
		//		Main.clrInput();
		//	}

		//	public float GetValue()
		//	{
		//		double.TryParse(contents, out double result);
		//		return (float)result;
		//	}
		//}
		//#endregion
	}
}
