// https://github.com/apache/metamodel/blob/b0cfe3aed447769f752743ac1753ebed90adaad2/core/src/main/java/org/apache/metamodel/query/QueryItem.java
namespace org.apache.metamodel.query
{
    /**
     * Interface for items in a query. All QueryItems reside within a QueryClause.
     * 
     * @see AbstractQueryClause
     */
    public interface QueryItem //: Serializable
    {
        QueryItem setQuery(Query query);
        Query     getQuery();
        string    toSql();
        string    toSql(bool includeSchemaInColumnPaths);
        string    toString();
    } // QueryItem interface
} // org.apache.metamodel.query namespace
