/* generated using openapi-typescript-codegen -- do not edit */
/* istanbul ignore file */
/* tslint:disable */
/* eslint-disable */
import type { CodeTypes } from './CodeTypes';
import type { GoodTransaction } from './GoodTransaction';
import type { IngredientMapping } from './IngredientMapping';
export type Good = {
    codeType?: CodeTypes;
    friendlyName?: string | null;
    id?: number;
    storeCode?: number | null;
    readonly goodTransactions?: Array<GoodTransaction> | null;
    readonly ingredientMappings?: Array<IngredientMapping> | null;
};

