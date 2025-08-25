/* generated using openapi-typescript-codegen -- do not edit */
/* istanbul ignore file */
/* tslint:disable */
/* eslint-disable */
import type { FoodCenterFoodRecord } from '../models/FoodCenterFoodRecord';
import type { CancelablePromise } from '../core/CancelablePromise';
import { OpenAPI } from '../core/OpenAPI';
import { request as __request } from '../core/request';
export class FoodCostService {
    /**
     * @param code
     * @returns FoodCenterFoodRecord Success
     * @throws ApiError
     */
    public static costFoodUpcCode(
        code?: string,
    ): CancelablePromise<FoodCenterFoodRecord> {
        return __request(OpenAPI, {
            method: 'GET',
            url: '/api',
            query: {
                'code': code,
            },
            errors: {
                404: `Not Found`,
            },
        });
    }
}
