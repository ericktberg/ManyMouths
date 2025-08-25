/* generated using openapi-typescript-codegen -- do not edit */
/* istanbul ignore file */
/* tslint:disable */
/* eslint-disable */
import type { CancelablePromise } from '../core/CancelablePromise';
import { OpenAPI } from '../core/OpenAPI';
import { request as __request } from '../core/request';
export class GoodService {
    /**
     * @param query
     * @returns any Success
     * @throws ApiError
     */
    public static getGoodSearch(
        query?: string,
    ): CancelablePromise<any> {
        return __request(OpenAPI, {
            method: 'GET',
            url: '/Good/search',
            query: {
                'query': query,
            },
        });
    }
}
