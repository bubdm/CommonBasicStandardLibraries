namespace CommonBasicStandardLibraries.DatabaseHelpers.EntityInterfaces
{
    public interface IJoinedEntity : ISimpleDapperEntity //hopefully i don't need more than 3 joined.  if so, rethink
    {
        void AddRelationships<E>(E entity)
            where E: class;
    }
    public interface IJoin3Entity<D1, D2> : ISimpleDapperEntity
        where D1: class
        where D2: class
    {
        void AddRelationships(D1? firstEntity, D2? secondEntity);
    }
}
