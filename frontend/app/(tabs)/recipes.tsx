import { RecipeList } from "@/components/recipelist";
import { router } from "expo-router";
import { useState } from "react";
import { View } from "react-native";

const handleRecipeClick = (recipeId?: number) => {
    router.push(`recipes/${recipeId}`);
};

const handleCreateRecipe = () => {
};

const handleMapIngredients = (recipeId?: number) => {
};

export default function Index() {

    return <RecipeList 
        onCreateRecipe={handleCreateRecipe}
        onRecipeClick={handleRecipeClick}
        onMapIngredients={handleMapIngredients} />
}