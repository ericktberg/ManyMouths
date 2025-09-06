/* generated using openapi-typescript-codegen -- do not edit */
/* istanbul ignore file */
/* tslint:disable */
/* eslint-disable */
import type { RecipeIngredientDTO } from './RecipeIngredientDTO';
export type RecipeDTO = {
    readonly id?: number;
    readonly name?: string | null;
    readonly description?: string | null;
    readonly prepTimeMinutes?: number;
    readonly cookTimeMinutes?: number;
    readonly servings?: number;
    ingredients?: Array<RecipeIngredientDTO> | null;
};

