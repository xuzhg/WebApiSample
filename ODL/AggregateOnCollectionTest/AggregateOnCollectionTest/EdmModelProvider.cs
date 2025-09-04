// See https://aka.ms/new-console-template for more information
using Microsoft.OData.Edm;
using System.Xml.Schema;

public class EdmModelProvider
{
    public static IEdmModel GetEdmModel()
    {
        EdmModel model = new EdmModel();

        EdmComplexType tag = new EdmComplexType("NS", "Tag");
        tag.AddStructuralProperty("Name", EdmPrimitiveTypeKind.String);
        tag.AddStructuralProperty("Count", EdmPrimitiveTypeKind.Int32, false);
        tag.AddStructuralProperty("Description", EdmPrimitiveTypeKind.String, true);

        model.AddElement(tag);

        EdmEntityType article = new EdmEntityType("NS", "Article");
        article.AddKeys(article.AddStructuralProperty("Id", EdmPrimitiveTypeKind.Int32, false));
        article.AddStructuralProperty("Title", EdmPrimitiveTypeKind.String);
        article.AddStructuralProperty("Emails", new EdmCollectionTypeReference(new EdmCollectionType(EdmCoreModel.Instance.GetString(true))));
        article.AddStructuralProperty("Tags", new EdmCollectionTypeReference(new EdmCollectionType(new EdmComplexTypeReference(tag, true))));
        model.AddElement(article);

        EdmEntityContainer container = new EdmEntityContainer("NS", "DefaultContainer");
        model.AddElement(container);
        container.AddEntitySet("Articles", article);
        return model;
    }
}
