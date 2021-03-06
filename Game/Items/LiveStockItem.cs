using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ThreadingInCsharp.Game.interfaces;

namespace ThreadingInCsharp.Game.Items
{
    public class LiveStockItem : Entity, IInventoryItem
    {
        int count;
        int price;
        string name;

        public LiveStockItem(Texture2D texture, Vector2 position, int price, int count, string name) : base(texture, position, 1)
        {
            this.price = price;
            this.count = count;
            this.name = name;
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
            SetCount();
        }

        public string GetName()
        {
            return this.name;
        }

        public void Sell()
        {
            this.count -= 1;
        }

        public int GetSellingPrice()
        {
            return 0;
        }
    }
}
