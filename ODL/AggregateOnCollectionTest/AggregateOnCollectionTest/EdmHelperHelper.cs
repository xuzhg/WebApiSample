// See https://aka.ms/new-console-template for more information
using Microsoft.OData.Edm;
using Microsoft.OData.UriParser;
using Microsoft.OData.UriParser.Aggregation;

public class EdmHelperHelper
{
    public static IEdmModel RegisterCustomFunctions(IEdmModel model)
    {
        // So far, model is not used, but in future, we want to register the custom functions along with model.
        IEdmComplexType? tagType = model.FindDeclaredType("NS.Tag") as IEdmComplexType;
        if (tagType == null)
        {
            throw new Exception("Failed to find type 'NS.Tag' in the model.");
        }

        IEdmTypeReference stringType = EdmCoreModel.Instance.GetString(true);
        IEdmCollectionTypeReference collectionStringType = new EdmCollectionTypeReference(new EdmCollectionType(stringType));

        FunctionSignatureWithReturnType functionSignatureWithReturnType = new FunctionSignatureWithReturnType(
            collectionStringType, // return type is Collection(Edm.String)
            new EdmCollectionTypeReference(new EdmCollectionType(new EdmComplexTypeReference(tagType, true))));  // only 1 argument, its type is Collection(NS.Tag)

        CustomUriFunctions.AddCustomUriFunction("Microsoft.Union", functionSignatureWithReturnType);

        functionSignatureWithReturnType = new FunctionSignatureWithReturnType(
            stringType, // return type is Edm.String)
            collectionStringType);  // only 1 argument, its type is Collection(Edm.String)
        CustomUriFunctions.AddCustomUriFunction("Microsoft.Combined", functionSignatureWithReturnType);
        return model;
    }

    public static void DoUriParse(IEdmModel model, string relativeUri)
    {
        ODataUriParser parser = new ODataUriParser(model, new Uri(relativeUri, UriKind.Relative));
        ApplyClause applyClause = parser.ParseApply();

        if (applyClause == null)
        {
            throw new Exception("Failed to parse the $apply clause.");
        }

        foreach (var transformation in applyClause.Transformations)
        {
            Console.WriteLine($"Transformation Kind: {transformation.Kind}");

            switch (transformation.Kind)
            {
                case TransformationNodeKind.Aggregate:
                    AggregateTransformationNode aggregateTransformation = (AggregateTransformationNode)transformation;
                    foreach (var aggregation in aggregateTransformation.AggregateExpressions)
                    {
                        Console.WriteLine($"  Aggregation: {aggregation.AggregateKind}");
                        switch (aggregation.AggregateKind)
                        {
                            case AggregateExpressionKind.CollectionPropertyAggregate:
                                AggregateCollectionExpression collectionPropertyAggregation = (AggregateCollectionExpression)aggregation;

                                if (collectionPropertyAggregation.Expression is CollectionPropertyAccessNode collectionPropertyAccess)
                                {
                                    Console.WriteLine($"    CollectionPropertyAccess Name: {collectionPropertyAccess.Property.Name}");
                                    Console.WriteLine($"    CollectionPropertyAccess Type: {collectionPropertyAccess.CollectionType.FullName()}");
                                }
                                else if (collectionPropertyAggregation.Expression is CollectionComplexNode collectionComplexAccess)
                                {
                                    Console.WriteLine($"    CollectionComplexNode Name: {collectionComplexAccess.Property.Name}");
                                    Console.WriteLine($"    CollectionComplexNode Type: {collectionComplexAccess.CollectionType.FullName()}");
                                }
                                else
                                {
                                    throw new NotImplementedException("You can you up.");
                                }


                                Console.WriteLine($"    MethodKind: {collectionPropertyAggregation.Method}");
                                Console.WriteLine($"    MethodName: {collectionPropertyAggregation.MethodDefinition.MethodLabel}");
                                break;
                            default:
                                throw new NotImplementedException("You can you up.");
                                //break;
                        }
                    }
                    break;
                default:
                    throw new NotImplementedException("You can you up.");
                    //break;
            }
        }
    }
}