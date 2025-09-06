import { RecipeList } from "@/components/forms/recipe-list";
import { router } from "expo-router";
import { useState } from "react";
import { View, Text } from "react-native";
import { useQuery } from "@tanstack/react-query";
import { RecipeRepository } from "@/src/repositories/recipe-repository";

const handleRecipeClick = (recipeId?: number) => {
    router.push(`recipes/${recipeId}`);
};

const handleCreateRecipe = () => {
    router.push('recipes/create');
};

export default function Index() {
    const recipesQuery = useQuery({
        queryKey: ['recipes'],
        queryFn: RecipeRepository.fetchRecipeOverviewList
    })

    if (recipesQuery.isLoading) return <Text>Loading...</Text>;
    if (recipesQuery.error) return <Text>Error loading recipes</Text>;

    return <RecipeList 
        onCreateRecipeClick={handleCreateRecipe}
        onRecipeClick={handleRecipeClick}
        recipes={recipesQuery.data || []} />
}