using Microsoft.Xna.Framework;
using System.Collections.Generic;
using Terraria.UI;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace ParticleLibrary.UI.Elements.Base
{
    public class ListBox : Panel
    {
        public ObservableCollection<Button> Items { get; }
        public IReadOnlyCollection<Button> SelectedItems { get => _selectedItems.AsReadOnly(); }
        private readonly List<Button> _selectedItems;

        public StyleDimension ItemHeight { get; set; }
        public StyleDimension ItemPadding { get; set; }

        private float _scroll;

        // TODO: Scrolling

        public ListBox(Color fill, Color outline, float outlineThickness = 1, float cornerRadius = 0, bool soft = false) : base(fill, outline, outlineThickness, cornerRadius, soft)
        {
            Items = new();
            Items.CollectionChanged += Items_CollectionChanged;

            _selectedItems = new();

            OverflowHidden = true;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        private void CalculateItemPositions()
        {
            CalculatedStyle inner = GetInnerDimensions();

            float height = ItemHeight.GetValue(inner.Height);
            float padding = ItemPadding.GetValue(inner.Height);
            float totalHeight = Items.Count * (height + padding);
            float exposableHeight = inner.Height / totalHeight;

            Vector2 position = inner.Position();

            foreach (var item in Items)
            {
                item.Top.Set(position.Y, 0f);
                item.Height = ItemHeight;
                item.PaddingBottom = padding;
                position.Y += height;
            }

            //foreach (var item in Items)
            //{
            //	// If we're too high up
            //	if (position.Y <= inner.X - (_scroll * totalHeight))
            //		continue;

            //	item.Top.Set(position.Y, 0f);
            //	item.Height = ItemHeight;
            //	item.PaddingBottom = padding;
            //	position.Y += height;

            //	// If we're too far down
            //	if (position.Y >= inner.Height)
            //		break;
            //}
        }

        private void Items_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            foreach (Button item in e.OldItems)
            {
                item.OnClick -= Item_OnClick;
            }

            foreach (Button item in e.NewItems)
            {
                item.OnClick += Item_OnClick;
            }
        }

        private void Item_OnClick(Button button)
        {
            if (_selectedItems.Contains(button))
            {
                _selectedItems.Remove(button);
                return;
            }

            _selectedItems.Add(button);
        }
    }
}
