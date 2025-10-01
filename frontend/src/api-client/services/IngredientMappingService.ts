/* generated using openapi-typescript-codegen -- do not edit */
/* istanbul ignore file */
/* tslint:disable */
/* eslint-disable */
import type { IngredientMappingCreationDTO } from '../models/IngredientMappingCreationDTO';
import type { IngredientMappingDTOLight } from '../models/IngredientMappingDTOLight';
import type { CancelablePromise } from '../core/CancelablePromise';
import { OpenAPI } from '../core/OpenAPI';
import { request as __request } from '../core/request';
export class IngredientMappingService {
    /**
     * @param requestBody
     * @returns IngredientMappingDTOLight Success
     * @throws ApiError
     */
    public static postApiIngredientMapping(
        requestBody?: IngredientMappingCreationDTO,
    ): CancelablePromise<IngredientMappingDTOLight> {
        return __request(OpenAPI, {
            method: 'POST',
            url: '/api/IngredientMapping',
            body: requestBody,
            mediaType: 'application/json',
        });
    }
    /**
     * @param ingredientId
     * @returns IngredientMappingDTOLight Success
     * @throws ApiError
     */
    public static getApiIngredientMapping(
        ingredientId?: number,
    ): CancelablePromise<IngredientMappingDTOLight> {
        return __request(OpenAPI, {
            method: 'GET',
            url: '/api/IngredientMapping',
            query: {
                'ingredientId': ingredientId,
            },
        });
    }
}
