/* generated using openapi-typescript-codegen -- do not edit */
/* istanbul ignore file */
/* tslint:disable */
/* eslint-disable */
import type { Recipe } from '../models/Recipe';
import type { RecipeCreationDto } from '../models/RecipeCreationDto';
import type { RecipeDetailDTO } from '../models/RecipeDetailDTO';
import type { RecipeOverviewDTO } from '../models/RecipeOverviewDTO';
import type { CancelablePromise } from '../core/CancelablePromise';
import { OpenAPI } from '../core/OpenAPI';
import { request as __request } from '../core/request';
export class RecipesService {
    /**
     * @param requestBody
     * @returns any Success
     * @throws ApiError
     */
    public static postApiRecipes(
        requestBody?: RecipeCreationDto,
    ): CancelablePromise<any> {
        return __request(OpenAPI, {
            method: 'POST',
            url: '/api/Recipes',
            body: requestBody,
            mediaType: 'application/json',
        });
    }
    /**
     * @returns RecipeOverviewDTO Success
     * @throws ApiError
     */
    public static getApiRecipes(): CancelablePromise<Array<RecipeOverviewDTO>> {
        return __request(OpenAPI, {
            method: 'GET',
            url: '/api/Recipes',
        });
    }
    /**
     * @param recipeId
     * @returns any Success
     * @throws ApiError
     */
    public static deleteApiRecipes(
        recipeId: number,
    ): CancelablePromise<any> {
        return __request(OpenAPI, {
            method: 'DELETE',
            url: '/api/Recipes/{recipeId}',
            path: {
                'recipeId': recipeId,
            },
        });
    }
    /**
     * @param recipeId
     * @returns RecipeDetailDTO Success
     * @throws ApiError
     */
    public static getApiRecipes1(
        recipeId: number,
    ): CancelablePromise<RecipeDetailDTO> {
        return __request(OpenAPI, {
            method: 'GET',
            url: '/api/Recipes/{recipeId}',
            path: {
                'recipeId': recipeId,
            },
        });
    }
    /**
     * @param recipeId
     * @param requestBody
     * @returns any Success
     * @throws ApiError
     */
    public static putApiRecipes(
        recipeId: number,
        requestBody?: Recipe,
    ): CancelablePromise<any> {
        return __request(OpenAPI, {
            method: 'PUT',
            url: '/api/Recipes/{recipeId}',
            path: {
                'recipeId': recipeId,
            },
            body: requestBody,
            mediaType: 'application/json',
        });
    }
}
