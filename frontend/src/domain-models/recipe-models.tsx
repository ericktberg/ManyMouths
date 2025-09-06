
interface RecipeOverviewModel
{
    name: string,
    description: string,
    prepTime: number,
    cookTime: number,
    servings: number,
}

interface RecipeDetailsModel extends RecipeOverviewModel
{
    instructions: string,
    ingredients: IngredientBaseModel[]
}