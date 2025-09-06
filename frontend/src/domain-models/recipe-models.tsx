import { Float } from "react-native/Libraries/Types/CodegenTypes";

export interface RecipeOverviewModel
{
    recipeId: number,
    name: string,
    description: string,
    prepTimeMinutes: number,
    cookTimeMinutes: number,
    servings: number,
}

export interface RecipeDetailsModel extends RecipeOverviewModel
{
    instructions: string,
    ingredients: IngredientBaseModel[]
}

// Input models should not know anything about IDs, as those are created on the backend
// Instead, its all about the data the user inputs


export interface RecipeInputModel {
  name: string,
  description: string,
  instructions: string,
  servings: number,
  prepTimeMinutes: number,
  cookTimeMinutes: number,
  ingredients: IngredientInputModel[]
}

export interface IngredientInputModel {
  id: string;
  name: string;
  amount: Float;
  unit: string;
}