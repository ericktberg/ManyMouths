/* generated using openapi-typescript-codegen -- do not edit */
/* istanbul ignore file */
/* tslint:disable */
/* eslint-disable */
import type { IngredientMappingCreationDTO } from '../models/IngredientMappingCreationDTO';
import type { CancelablePromise } from '../core/CancelablePromise';
import { OpenAPI } from '../core/OpenAPI';
import { request as __request } from '../core/request';
export class IngredientMappingService {
    /**
     * @param requestBody
     * @returns any Success
     * @throws ApiError
     */
    public static postApiIngredientMapping(
        requestBody?: IngredientMappingCreationDTO,
    ): CancelablePromise<any> {
        return __request(OpenAPI, {
            method: 'POST',
            url: '/api/IngredientMapping',
            body: requestBody,
            mediaType: 'application/json',
        });
    }
    /**
     * @param ingredientId
     * @returns any Success
     * @throws ApiError
     */
    public static getApiIngredientMapping(
        ingredientId?: number,
    ): CancelablePromise<any> {
        return __request(OpenAPI, {
            method: 'GET',
            url: '/api/IngredientMapping',
            query: {
                'ingredientId': ingredientId,
            },
        });
    }
}
