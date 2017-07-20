// https://github.com/apache/metamodel/blob/master/core/src/main/java/org/apache/metamodel/query/builder/JoinFromBuilder.java
using org.apache.metamodel.schema;

namespace org.apache.metamodel.query.builder
{
    public interface JoinFromBuilder // : SatisfiedFromBuilder
    {
        SatisfiedFromBuilder on(Column left, Column right); // throws IllegalArgumentException;
        SatisfiedFromBuilder on(string left, string right); // throws IllegalArgumentException;
    } // JoinFromBuilder interface
} // org.apache.metamodel.query.builder namspace
