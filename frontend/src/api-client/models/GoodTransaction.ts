/* generated using openapi-typescript-codegen -- do not edit */
/* istanbul ignore file */
/* tslint:disable */
/* eslint-disable */
import type { Good } from './Good';
import type { StoreLocation } from './StoreLocation';
export type GoodTransaction = {
    goodId?: number;
    price?: number;
    storeLocationId?: number;
    id?: number;
    unit?: string | null;
    good?: Good;
    storeLocation?: StoreLocation;
};

