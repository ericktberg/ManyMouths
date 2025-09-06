import { RecipeDetailDTO, RecipeOverviewDTO, RecipesService } from "../api-client";

class RecipeRepository {
    /** Calls the Many Mouths backend API, acquires a list of recipes from it,
     * then transforms them into model types that can be consumed by the many mouths fronted.
     */
    static async fetchRecipeOverviewList(): Promise<RecipeOverviewModel[]> {
        const dtos: RecipeOverviewDTO[] = await RecipesService.getApiRecipes();
        return dtos.map(this.transformToOverviewModel);
    }

    /** Calls the Many Mouths backend API, acquires the details of a specific recipe from it,
     * then transforms it into a model type that can be consumed by the Many Mouths frontend.
     */
    static async fetchRecipeDetails(recipeId: number): Promise<RecipeDetailsModel> {
        const dto: RecipeDetailDTO = await RecipesService.getApiRecipes1(recipeId);
        const model = this.transformToDetailModel(dto);
        return model;
    }

    /** Transform the DTO gotten from the overview API into a model to be consumed by the Many Mouths frontend.
     */
    private static transformToOverviewModel(dto: RecipeOverviewDTO): RecipeOverviewModel {
        return {
            name: dto.name || "",
            description: dto.description || "",
            prepTime: dto.prepTimeMinutes || 0,
            cookTime: dto.cookTimeMinutes || 0,
            servings: dto.servings || 0
        };
    }

    /** Transform the DTO gotten from the detail API into a model to be consumed by the Many Mouths frontend.
     * Note that this method calls transformToOverviewModel to avoid code duplication.
     */
    private static transformToDetailModel(dto: RecipeDetailDTO): RecipeDetailsModel {
        return {
            ...this.transformToOverviewModel(dto),
            instructions: dto.instructions,
            ingredients: []
        }
    }
}