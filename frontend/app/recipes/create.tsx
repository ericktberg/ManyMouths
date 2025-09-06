import { RecipeCreationForm } from '@/components/forms/recipe-creation-form';
import { RecipeInputModel } from '@/src/domain-models/recipe-models';
import { RecipeRepository } from '@/src/repositories/recipe-repository';
import { useMutation, useQueryClient } from '@tanstack/react-query';
import { router, Stack } from 'expo-router';
import React from 'react';
import { Alert } from 'react-native';

export default function CreateRecipePage() {
    const queryClient = useQueryClient();

    const createRecipeMutation = useMutation({
        mutationFn: async (recipe: RecipeInputModel) => await RecipeRepository.createRecipe(recipe),
        onSuccess: (data) => {
            // Invalidate the recipes list so it refetches
            queryClient.invalidateQueries({ queryKey: ['recipes'] });

            // Navigate to the new recipe's detail page
            const newRecipeId = data?.recipeId;
            if (newRecipeId) {
                router.push(`/recipes/${newRecipeId}`);
            } else {
                router.push('/recipes');
            }
        },
        onError: (error) => {
            Alert.alert(
                "Save failed",
                "We couldn't save your recipe. Please try again later."
            );
        }
    });

    function OnBack() {
        if (router.canGoBack()) {
            router.back();
        }
        else {
            router.navigate("recipes");
        }
    }

    function OnSave(recipe: RecipeInputModel) {
        createRecipeMutation.mutate(recipe);
    }

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
            <RecipeCreationForm onBack={OnBack} onSave={OnSave} />
        </>
    );
}
