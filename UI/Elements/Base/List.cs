using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Terraria.GameContent.UI.Elements;
using Terraria.UI;

namespace ParticleLibrary.UI.Elements.Base
{
	public class List : Panel
	{
		public ObservableCollection<UIElement> Items { get; }

		public IReadOnlyCollection<UIElement> SelectedItems { get => _selectedItems.AsReadOnly(); }
		private readonly List<UIElement> _selectedItems;

		public StyleDimension ItemHeight { get; set; }
		public StyleDimension ItemPadding { get; set; }

		private readonly InnerList _innerList;
		private readonly ScrollBar _scrollBar;

		public List(Color fill, Color outline, float outlineThickness = 1, float cornerRadius = 0, bool soft = false) : base(fill, outline, outlineThickness, cornerRadius, soft)
		{
			Items = new();
			Items.CollectionChanged += Items_CollectionChanged;

			ItemHeight = new StyleDimension(24f, 0f);
			ItemPadding = new StyleDimension(4f, 0f);

			_selectedItems = new();

			_innerList = new();
			_innerList.SetPadding(4f);
			_innerList.Width.Set(0f, 1f);
			_innerList.Height.Set(0f, 1f);
			Append(_innerList);

			_scrollBar = new(ParticleLibraryConfig.CurrentTheme.Low, ParticleLibraryConfig.CurrentTheme.LowAccent);
			_scrollBar.Width.Set(32f, 0f);
			_scrollBar.Height.Set(0f, 1f);
			Append(_scrollBar);

			OverflowHidden = true;
		}

		private class InnerList : UIElement
		{
			//public override bool ContainsPoint(Vector2 point) => true;

			protected override void DrawChildren(SpriteBatch spriteBatch)
			{
				Rectangle parentDim = Parent.GetDimensions().ToRectangle();

				foreach (UIElement element in Elements)
				{
					Rectangle elementDim = element.GetDimensions().ToRectangle();

					if (parentDim.Contains(elementDim) || elementDim.Intersects(parentDim))
						element.Draw(spriteBatch);
				}
			}

			//public override Rectangle GetViewCullingArea() => Parent.GetDimensions().ToRectangle();
		}

		public virtual void Add(UIElement item)
		{
			Items.Add(item);
			_innerList.Append(item);
			//UpdateOrder();
			_innerList.Recalculate();
		}

		public virtual bool Remove(UIElement item)
		{
			_innerList.RemoveChild(item);
			//UpdateOrder();
			return Items.Remove(item);
		}

		public virtual void Clear()
		{
			_innerList.RemoveAllChildren();
			Items.Clear();
		}

		public override void Recalculate()
		{
			base.Recalculate();

			_scrollBar.Left.Set(-_scrollBar.Width.GetValue(_scrollBar.Parent.Width.Pixels), 1f);

			//UpdateScrollbar();
		}

		public override void ScrollWheel(UIScrollWheelEvent evt)
		{
			base.ScrollWheel(evt);

			if (evt.Target != this)
			{
				return;
			}

			//if (_scrollbar != null)
			//	_scrollbar.ViewPosition -= evt.ScrollWheelValue;
		}

		public override void RecalculateChildren()
		{
			base.RecalculateChildren();

			CalculatedStyle inner = GetInnerDimensions();

			float height = ItemHeight.GetValue(inner.Height);
			float padding = ItemPadding.GetValue(inner.Height);
			float totalHeight = Items.Count * (height + padding);
			float exposableHeight = inner.Height / totalHeight;

			float top = 0f;

			foreach (var item in Items)
			{
				item.Top.Set(top, 0f);
				item.Width.Set(0f, 1f);
				item.Height = ItemHeight;
				item.Recalculate();
				top += height + padding;
			}

			//float num = 0f;
			//for (int i = 0; i < _items.Count; i++)
			//{
			//	float num2 = ((_items.Count == 1) ? 0f : ListPadding);
			//	_items[i].Top.Set(num, 0f);
			//	_items[i].Recalculate();
			//	num += _items[i].GetOuterDimensions().Height + num2;
			//}

			//_innerListHeight = num;
		}

		private void Items_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (e.OldItems is not null)
			{
				foreach (UIElement item in e.OldItems)
				{
					item.OnLeftClick -= Item_OnClick;
				}
			}

			if (e.NewItems is not null)
			{
				foreach (UIElement item in e.NewItems)
				{
					item.OnLeftClick += Item_OnClick;
				}
			}
		}

		private void Item_OnClick(UIMouseEvent evt, UIElement listeningElement)
		{
			if (!Items.Contains(evt.Target))
				return;

			if (_selectedItems.Contains(evt.Target))
			{
				_selectedItems.Remove(evt.Target);
				return;
			}

			_selectedItems.Add(evt.Target);
		}
	}
}
