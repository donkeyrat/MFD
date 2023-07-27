using UnityEngine;

namespace ModdingForDummies.TABSSimp
{
    public abstract class ModdingClass<T> where T : ModdingClass<T>
    {
        public abstract string Name { get; set; }

        public abstract T Clone();
        public abstract void Separate();
        public abstract void ColorInternal(int index, Color color, float glow = 0f);

        public void Color(Color color, float glow = 0f) => ColorInternal(0, color, glow);

        public void Color(string color, float glow = 0f) => Color(0, Utilities.HexColor(color), glow);

        public void Color(int index, Color color, float glow = 0f) => ColorInternal(index, color, glow);

        public void Color(int index, string color, float glow = 0f) => Color(index, Utilities.HexColor(color), glow);


        public void Colors(params Color[] colors)
        {
            for (var i = 0; i < colors.Length; i++) Color(i, colors[i]);
        }

        public void Colors(params string[] colors)
        {
            for (var i = 0; i < colors.Length; i++) Color(i, Utilities.HexColor(colors[i]));
        }

        public void Colors(params (Color color, float glow)[] colors)
        {
            for (var i = 0; i < colors.Length; i++) Color(i, colors[i].color, colors[i].glow);
        }

        public void Colors(params (string color, float glow)[] colors)
        {
            for (var i = 0; i < colors.Length; i++) Color(i, Utilities.HexColor(colors[i].color), colors[i].glow);
        }
    }
}
