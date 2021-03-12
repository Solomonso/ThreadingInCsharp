using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace ThreadingInCsharp.Game.interfaces
{
 public interface IInventoryItem
    {
        int GetPrice();

        int GetCount();

         Texture2D GetTexture();

         void SetCount();

         void SetPrice(int price);

         void Buy();

         string GetName();

         void Sell();

        int GetSellingPrice();
    }
}
