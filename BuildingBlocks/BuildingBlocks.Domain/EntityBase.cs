using System.ComponentModel.DataAnnotations;

namespace BuildingBlocks.Domain
{
    public abstract class EntityBase : EntityBase<long>;

    public abstract class EntityBase<TKey> : IEntity<TKey>
    {
        private TKey? _id;

        protected EntityBase()
        {
            // Intentionally left blank; tracking properties moved to concrete entities.
        }

        protected EntityBase(TKey keyValue)
        {
            _id = keyValue;
        }

        [Key]
        public virtual TKey Id
        {
            get => _id!;
            set => _id = value;
        }

        object? IEntity.Id
        {
            get => Id;
        }
    }
}
