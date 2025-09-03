import { RecipeList } from "@/components/forms/recipe-list";
import { router } from "expo-router";
import { useState } from "react";
import { View } from "react-native";

const handleRecipeClick = (recipeId?: number) => {
    router.push(`recipes/${recipeId}`);
};

const handleCreateRecipe = () => {
    router.push('recipes/create');
};

const handleMapIngredients = (recipeId?: number) => {
};

export default function Index() {

    return <RecipeList 
        onCreateRecipe={handleCreateRecipe}
        onRecipeClick={handleRecipeClick}
        onMapIngredients={handleMapIngredients} />
}