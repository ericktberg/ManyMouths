/* generated using openapi-typescript-codegen -- do not edit */
/* istanbul ignore file */
/* tslint:disable */
/* eslint-disable */
import type { RecipeOwner } from './RecipeOwner';
import type { RecipeQuant } from './RecipeQuant';
export type Recipe = {
    id?: number;
    name?: string | null;
    readonly ingredientQuantities?: Array<RecipeQuant> | null;
    readonly recipeOwners?: Array<RecipeOwner> | null;
};

