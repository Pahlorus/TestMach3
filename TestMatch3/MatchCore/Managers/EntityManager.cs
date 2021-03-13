
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using TestMatch3;

namespace MatchCore
{
    public class EntityManager
    {
        private Random _random;
        private SettingsManager _settingsManager;
        private ResourceManager _resource;
        private List<Bit> _freeBitList;

        public EntityManager(SettingsManager settingsManager, ResourceManager resource)
        {
            _freeBitList = new List<Bit>();
            _random = new Random();
            _settingsManager = settingsManager;
            _resource = resource;
        }

        //TODO: Реализовать пул
        public Bit GetBitFromPool()
        {
            return null;
        }

        public Bit GetCell()
        {
            return null;
        }

        public void ReturnBitToPool()
        {

        }

        public Bit CreateBit(BitType type, ModifiType modifi = ModifiType.None)
        {
            string name = (type).ToString();

            BitData data = new BitData();
            RendererData rendererData = _settingsManager.GetBitRendererData();
            RendererData modifierRendererData = _settingsManager.GetBitModifierRendererData();

            rendererData.texture = _resource.GetCommonTexture(name);
            rendererData.auxTexture = _resource.GetCommonTexture(name + "Select");
            data.mainRendererData = rendererData;
            if (modifi > 0) modifierRendererData.texture = _resource.GetCommonTexture(modifi.ToString());
            data.modifiRendererData = modifierRendererData;

            Bit bit = new Bit(data);
            bit.BitType = type;
            bit.ModifiType = modifi;
            bit.Scale = 1;
            return bit;
        }

        public Bit CreateBit(ModifiType modifi = ModifiType.None)
        {
            int index = _random.Next(0, Enum.GetNames(typeof(BitType)).Length);
            BitType type = (BitType)index;
            return CreateBit(type, modifi);
        }

        public List<Bit> CreateNewBits(Cell[] column, int step, int count)
        {
            Cell firstCell = column[0];
            List<Bit> list = new List<Bit>();

            for (int i = 1; i <= count; i++)
            {
                Point pos = new Point(firstCell.Position.X, firstCell.Position.Y - step * i);
                Bit bit = CreateBit();
                bit.Position = pos;
                bit.Scale = 1;
                list.Add(bit);
            }
            return list;
        }

        public Destroyer CreateDestroyer()
        {
            RendererData rendererData = _settingsManager.GetDestroyerRendererData();
            Destroyer destroyer = new Destroyer(rendererData);
            destroyer.Scale = 1;
            return destroyer;
        }
    }
}