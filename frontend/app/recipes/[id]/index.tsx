import React, { useEffect } from 'react';
import { Text, View } from 'react-native';
import { router, useLocalSearchParams, useNavigation } from 'expo-router';
import { RecipeOverview } from '@/components/forms/recipe-details-page';
import { useQuery } from '@tanstack/react-query';
import { RecipeRepository } from '@/src/repositories/recipe-repository';
import { IngredientBaseModel } from '@/src/domain-models/ingredient-models';


export default function RecipeDetailsPage() {
  const { id } = useLocalSearchParams();
  const recipeId = parseInt(Array.isArray(id) ? id[0] : id);

  // Change the state type to be either a 'Recipe' or 'null'
  const s = useQuery({
    queryKey: ['recipe', recipeId],
    queryFn: () => RecipeRepository.fetchRecipeDetails(recipeId),
  })

  const navigation = useNavigation();

  useEffect(() => {
    navigation.setOptions({
      title: s.data?.name || 'Recipe Details',
      headerShown: false,
    });
  }, [navigation, s.data?.name]);

  if (s.isLoading) {
    return (
      <View className="flex-1 justify-center items-center">
        <Text>Loading...</Text>
      </View>
    );
  } else if (s.error) {
    return (
      <View className="flex-1 justify-center items-center">
        <Text>Error: {s.error.message}</Text>
      </View>
    );
  }

  const recipe = s.data;
  if (!recipe) {
    return (
      <View className="flex-1 justify-center items-center">
        <Text>No recipe found.</Text>
      </View>
    );
  } else {
    function OnAddToMeal() {
      // Add to the meal api - not yet written
    }

    function handleLinkIngredientClick(ingredient: IngredientBaseModel) {
      router.push({ pathname: `/(modals)/ingredients/${ingredient.id}/link-ingredients` });
    }

    // Example: function to handle linking an ingredient (API call)
    async function handleLinkIngredient() {
      // Call your API/mutation here
      // await RecipeRepository.linkIngredientToRecipe(recipeId, ingredientId, linkData);
      // Optionally refetch or update state
    }

    // Example: function to fetch extra ingredient info
    async function fetchIngredientInfo() {
      // return await RecipeRepository.fetchIngredientDetails(ingredientId);
    }

    return (
      <View className="flex-1 items-center">
        <View className="items-center justify-center w-full max-w-[500px]">
          <RecipeOverview
            recipe={recipe}
            onAddToMeal={OnAddToMeal}
            onBack={router.back}
            onEdit={function (): void {
              throw new Error('Function not implemented.');
            }}
            onRate={function (): void {
              throw new Error('Function not implemented.');
            }}
            onGoToMeal={function (): void {
              throw new Error('Function not implemented.');
            }}
            onStartCooking={function (): void {
              throw new Error('Function not implemented.');
            }}
            onLinkIngredients={function (): void {
              throw new Error('Function not implemented.');
            }}
            isInMeal={false}
            handleLinkIngredientClick={handleLinkIngredientClick}
            onLinkIngredient={handleLinkIngredient}
            fetchIngredientInfo={fetchIngredientInfo}
          />
        </View>
      </View>
    );
  }
}
