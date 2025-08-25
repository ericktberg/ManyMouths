/* generated using openapi-typescript-codegen -- do not edit */
/* istanbul ignore file */
/* tslint:disable */
/* eslint-disable */
import type { ReceiptDto } from '../models/ReceiptDto';
import type { CancelablePromise } from '../core/CancelablePromise';
import { OpenAPI } from '../core/OpenAPI';
import { request as __request } from '../core/request';
export class ReceiptService {
    /**
     * @param requestBody
     * @returns any Success
     * @throws ApiError
     */
    public static postApiReceipt(
        requestBody?: ReceiptDto,
    ): CancelablePromise<any> {
        return __request(OpenAPI, {
            method: 'POST',
            url: '/api/Receipt',
            body: requestBody,
            mediaType: 'application/json',
        });
    }
    /**
     * @param foodName
     * @returns any Success
     * @throws ApiError
     */
    public static getApiReceiptSearch(
        foodName?: string,
    ): CancelablePromise<any> {
        return __request(OpenAPI, {
            method: 'GET',
            url: '/api/Receipt/search',
            query: {
                'foodName': foodName,
            },
        });
    }
}
