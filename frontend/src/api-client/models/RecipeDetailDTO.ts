/* generated using openapi-typescript-codegen -- do not edit */
/* istanbul ignore file */
/* tslint:disable */
/* eslint-disable */
import type { GoodDTOLight } from './GoodDTOLight';
import type { RecipeIngredientDTO } from './RecipeIngredientDTO';
export type RecipeDetailDTO = {
    id?: number;
    name?: string | null;
    ingredients?: Array<RecipeIngredientDTO> | null;
    ingredientMappings?: Record<string, GoodDTOLight> | null;
};

