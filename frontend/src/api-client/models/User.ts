/* generated using openapi-typescript-codegen -- do not edit */
/* istanbul ignore file */
/* tslint:disable */
/* eslint-disable */
import type { RecipeOwner } from './RecipeOwner';
import type { SelectedIngredientMapping } from './SelectedIngredientMapping';
export type User = {
    userId?: number;
    ownedRecipes?: Array<RecipeOwner> | null;
    selectedIngredientMappings?: Array<SelectedIngredientMapping> | null;
};

