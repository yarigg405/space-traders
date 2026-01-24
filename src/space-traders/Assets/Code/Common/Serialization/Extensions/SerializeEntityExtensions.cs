using Assets.Code.Common.Extensions;
using Assets.Code.Common.Serialization.Data;
using Entitas;
using System;
using System.Linq;


namespace Assets.Code.Common.Serialization.Extensions
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

        public static EntitySnapshot AsSerializedEntity(this IEntity entity, int[] components)
        {
            var entitySnapshot = new EntitySnapshot();
            entitySnapshot.Components = new(components.Length);

            foreach (var componentIndex in components)
            {
                var component = entity.GetComponent(componentIndex) as ISerializeComponent;
                entitySnapshot.Components.Add(component);
            }

            return entitySnapshot;
        }

        public static IEntity FillEntityWith(this IEntity entity, EntitySnapshot snapshot)
        {
            foreach (ISerializeComponent component in snapshot.Components)
            {
                var lookupIndex = ComponentIndexByType.IndexByType(component.GetType());
                entity.With(x => x.ReplaceComponent(lookupIndex, component), when: lookupIndex >= 0);
            }

            return entity;
        }
    }
}