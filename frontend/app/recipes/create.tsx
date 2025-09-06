import { RecipeCreation, RecipeInput } from '@/components/forms/recipe-creation-form';
import { RecipesService, RecipeCreationDto } from '@/src/api-client';
import { router, Stack } from 'expo-router';
import React from 'react';
import { Alert } from 'react-native';

function OnBack() {
    if (router.canGoBack()) {
        router.back();
    }
    else {
        router.navigate("recipes");
    }
}

async function OnSave(recipe: RecipeInput) {
    try {
        const response = await RecipesService.postApiRecipes({
            instructionMarkdownText: recipe.instructions,
            servings: recipe.servings,
            cookTimeMinutes: recipe.cookTime,
            prepTimeMinutes: recipe.prepTime,
            name: recipe.name,
            description: recipe.description,
            ingredients: recipe.ingredients.map((i) => ({
                name: i.name,
                quantity: parseInt(i.amount) || 1,
                unit: i.unit
            }))
        })

        // Assuming the response includes the new recipe's ID
        const newRecipeId = response.id;

        // Navigate to the recipe detail page
        router.push(`/recipes/${newRecipeId}`);
    } catch (error) {
        console.error("Failed to save recipe:", error);

        // Show a user-friendly message
        Alert.alert(
            "Save failed",
            "We couldn't save your recipe. Please try again later."
        );
    }
}

export default function CreateRecipePage() {
    return (
        <>
            <Stack.Screen
                options={{
                    title: 'Create New Recipe',
                    headerStyle: { backgroundColor: '#f8f9fa' },
                    headerTitleStyle: { fontWeight: 'bold' },
                    headerBackTitle: 'Back',
                    headerShown: false
                }}
            />
            {/* Your page content */}
            <RecipeCreation onBack={OnBack} onSave={OnSave} />
        </>
    );
}
