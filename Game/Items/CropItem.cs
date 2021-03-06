using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ThreadingInCsharp.Game.interfaces;

namespace ThreadingInCsharp.Game.Items
{
    public class CropItem : Entity, IInventoryItem
    {
        int price;
        int count;
        string name;
        bool selected = false;
        int sellingPrice;
        public CropItem(Texture2D texture, Vector2 position, int price, int count, string name, int sellingPrice) : base(texture, position, 1)
        {
            this.price = price;
            this.count = count;
            this.name = name;
            this.sellingPrice = sellingPrice;
        }

        public virtual int GetPrice()
        {
            return this.price;
        }

        public virtual int GetCount()
        {
            return this.count;
        }

        public virtual Texture2D GetTexture()
        {
            return Texture;
        }

        public virtual bool BuyItem()
        {
            if (99 > GetPrice())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void SetCount()
        {
            this.count += 1;
        }

        public void SetPrice(int price)
        {
            this.price = price;
        }

        public void Buy()
        {

        }

        public string GetName()
        {
            return this.name;
        }

        public bool IsSelected()
        {
            return this.selected;
        }

        public void Select(bool select)
        {
            this.selected = select;
        }

        public void Sell()
        {
            this.count -= 1;
        }

        public int GetSellingPrice()
        {
            return this.sellingPrice;
        }
    }
}
