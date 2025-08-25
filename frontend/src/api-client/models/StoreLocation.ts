/* generated using openapi-typescript-codegen -- do not edit */
/* istanbul ignore file */
/* tslint:disable */
/* eslint-disable */
import type { GoodTransaction } from './GoodTransaction';
import type { StoreChain } from './StoreChain';
export type StoreLocation = {
    storeLocationId?: number;
    storeChainId?: number;
    locationAddress?: string | null;
    locationNumber?: number;
    chain?: StoreChain;
    readonly goodTransactions?: Array<GoodTransaction> | null;
};

