using Assets.Code.Serialization.Data;
using Code.Common.Extensions;
using Entitas;
using System;
using System.Linq;


namespace Assets.Code.Serialization.Extensions
{
    public static class SerializeEntityExtensions
    {
        public static EntitySnapshot AsSerializedEntity(this IEntity entity)
        {
            var components = entity.GetComponents();
            return new EntitySnapshot
            {
                Components = components
                .Where(c => c is ISerializeComponent)
                .Cast<ISerializeComponent>()
                .ToList()
            };
        }

        public static IEntity FillEntityWith(this IEntity entity, EntitySnapshot snapshot)
        {
            foreach (ISerializeComponent component in snapshot.Components)
            {
                var lookupIndex = Array.IndexOf(GameComponentsLookup.componentTypes, component.GetType());
                entity.With(x => x.ReplaceComponent(lookupIndex, component), when: lookupIndex >= 0);
            }

            return entity;
        }
    }
}