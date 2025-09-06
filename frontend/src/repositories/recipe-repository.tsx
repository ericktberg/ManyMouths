import { RecipeDetailDTO, RecipeOverviewDTO, RecipesService, RecipeCreationDto } from "../api-client";
import { RecipeOverviewModel, RecipeDetailsModel, RecipeInputModel } from "../domain-models/recipe-models";

export class RecipeRepository {
    /** Calls the Many Mouths backend API to create a new recipe based on the provided input model.
     */
    static async createRecipe(recipeInput: RecipeInputModel): Promise<RecipeDetailsModel> {
        const dto = RecipeRepository.transformInputToDto(recipeInput);
        const resultDto = await RecipesService.postApiRecipes(dto);
        return RecipeRepository.transformToDetailModel(resultDto);
    }

    /** Calls the Many Mouths backend API, acquires a list of recipes from it,
     * then transforms them into model types that can be consumed by the many mouths fronted.
     */
    static async fetchRecipeOverviewList(): Promise<RecipeOverviewModel[]> {
        try {
            const dtos: RecipeOverviewDTO[] = await RecipesService.getApiRecipes();
            console.log("API returned overview list:", dtos);
            return dtos.map(dto => RecipeRepository.transformToOverviewModel(dto));
        } catch (e) {
            console.error("Error in fetchRecipeOverviewList:", e);
            throw e; // Let React Query handle the error state
        }
    }

    /** Calls the Many Mouths backend API, acquires the details of a specific recipe from it,
     * then transforms it into a model type that can be consumed by the Many Mouths frontend.
     */
    static async fetchRecipeDetails(recipeId: number): Promise<RecipeDetailsModel> {
        const dto: RecipeDetailDTO = await RecipesService.getApiRecipes1(recipeId);
        console.log("API returned recipe details:", dto);
        const model = RecipeRepository.transformToDetailModel(dto);
        return model;
    }

    private static transformInputToDto(input: RecipeInputModel): RecipeCreationDto {
        return {
            name: input.name,
            description: input.description,
            instructionMarkdownText: input.instructions,
            prepTimeMinutes: input.prepTimeMinutes,
            cookTimeMinutes: input.cookTimeMinutes,
            servings: input.servings,
            ingredients: input.ingredients.map(i => ({
                name: i.name,
                quantity: i.amount,
                unit: i.unit
            }))
        };
    }

    /** Transform the DTO gotten from the overview API into a model to be consumed by the Many Mouths frontend.
     */
    private static transformToOverviewModel(dto: RecipeOverviewDTO): RecipeOverviewModel {
        console.log("Transforming overview DTO:", dto);
        return {
            recipeId: dto.id || 0,
            name: dto.name || "",
            description: dto.description || "",
            prepTimeMinutes: dto.prepTimeMinutes || 0,
            cookTimeMinutes: dto.cookTimeMinutes || 0,
            servings: dto.servings || 0
        };
    }

    /** Transform the DTO gotten from the detail API into a model to be consumed by the Many Mouths frontend.
     * Note that this method calls transformToOverviewModel to avoid code duplication.
     */
    private static transformToDetailModel(dto: RecipeDetailDTO): RecipeDetailsModel {
        return {
            ...RecipeRepository.transformToOverviewModel(dto),
            instructions: dto.markdownInstructions || "",
            ingredients: []
        }
    }
}