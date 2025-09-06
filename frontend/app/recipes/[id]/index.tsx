import { Text } from 'react-native';
import { router, useLocalSearchParams, useNavigation } from 'expo-router';
import { RecipeOverview } from '@/components/forms/recipe-details-page';
import { useQuery } from '@tanstack/react-query';
import { RecipeRepository } from '@/src/repositories/recipe-repository';
import { useEffect } from "react";


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
    navigation.setOptions({ title: s.data?.name || 'Recipe Details' });
  }, [navigation, s.data?.name]);

  if (s.isLoading) {
    return <Text>Loading...</Text>;
  }
  else if (s.error) {
    return <Text>Error: {s.error.message}</Text>;
  }

  const recipe = s.data;
  if (!recipe) {
    return <Text>No recipe found.</Text>;
  }
  else {
    function OnAddToMeal() {
      // Add to the meal api - not yet written
    }

    return (
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
        }
        }
        onLinkIngredients={function (): void {
          throw new Error('Function not implemented.');
        }}
        isInMeal={false} />
    );
  }
}
