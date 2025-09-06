/* generated using openapi-typescript-codegen -- do not edit */
/* istanbul ignore file */
/* tslint:disable */
/* eslint-disable */
import type { RecipeIngredientDTO } from './RecipeIngredientDTO';
export type RecipeDetailDTO = {
    id?: number;
    name?: string | null;
    description?: string | null;
    prepTimeMinutes?: number;
    cookTimeMinutes?: number;
    servings?: number;
    markdownInstructions?: string | null;
    ingredients?: Array<RecipeIngredientDTO> | null;
};

