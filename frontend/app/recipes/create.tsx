import { RecipeCreation } from '@/components/forms/recipe-creation-form';
import { router, Stack } from 'expo-router';
import React from 'react';

function OnBack() {
    if (router.canGoBack()) {
        router.back();
    }
    else {
        router.navigate("recipes");
    }
}

function OnSave() {

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
