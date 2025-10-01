/* generated using openapi-typescript-codegen -- do not edit */
/* istanbul ignore file */
/* tslint:disable */
/* eslint-disable */
import type { AddPriceDTO } from '../models/AddPriceDTO';
import type { CreateGoodDTO } from '../models/CreateGoodDTO';
import type { GoodDTOLight } from '../models/GoodDTOLight';
import type { GoodTransactionDTO } from '../models/GoodTransactionDTO';
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
    /**
     * @param requestBody
     * @returns GoodDTOLight Success
     * @throws ApiError
     */
    public static postGood(
        requestBody?: CreateGoodDTO,
    ): CancelablePromise<GoodDTOLight> {
        return __request(OpenAPI, {
            method: 'POST',
            url: '/Good',
            body: requestBody,
            mediaType: 'application/json',
        });
    }
    /**
     * @param goodId
     * @returns GoodTransactionDTO Success
     * @throws ApiError
     */
    public static getGoodLatestPrice(
        goodId: number,
    ): CancelablePromise<GoodTransactionDTO> {
        return __request(OpenAPI, {
            method: 'GET',
            url: '/Good/{goodId}/latest-price',
            path: {
                'goodId': goodId,
            },
        });
    }
    /**
     * @param goodId
     * @param requestBody
     * @returns GoodTransactionDTO Success
     * @throws ApiError
     */
    public static postGoodPrice(
        goodId: number,
        requestBody?: AddPriceDTO,
    ): CancelablePromise<GoodTransactionDTO> {
        return __request(OpenAPI, {
            method: 'POST',
            url: '/Good/{goodId}/price',
            path: {
                'goodId': goodId,
            },
            body: requestBody,
            mediaType: 'application/json',
        });
    }
    /**
     * @returns any Success
     * @throws ApiError
     */
    public static getGoodAllowedUnits(): CancelablePromise<any> {
        return __request(OpenAPI, {
            method: 'GET',
            url: '/Good/allowed-units',
        });
    }
}
