// recipes/[id].tsx
import { View, Text } from 'react-native';
import { useLocalSearchParams, useNavigation } from 'expo-router';
import { useEffect, useState } from 'react';
import { RecipeDetailDTO, RecipesService } from '@/src/api-client';

export default function RecipeDetailsPage() {
  const { id } = useLocalSearchParams();
  // Change the state type to be either a 'Recipe' or 'null'
  const [recipe, setRecipe] = useState<RecipeDetailDTO | null>(null);
  const [error, setError] = useState<string>("");
  const [isLoading, setIsLoading] = useState(true);

  const navigation = useNavigation();

  useEffect(() => {
    const fetchRecipe = async () => {
      // The id from the URL params can be a string, or an array of strings,
      // so we need to ensure we're working with a string.
      const recipeId = parseInt(Array.isArray(id) ? id[0] : id);

      try {
        const selectedRecipe = await RecipesService.getApiRecipes1(recipeId);

        // If a recipe is found, set it in the state
        if (selectedRecipe) {
          navigation.setOptions({ title: selectedRecipe.name });
          setRecipe(selectedRecipe);
        } else {
          // If no recipe is found, set the state back to null
          setRecipe(null);
        }
      }
      catch (e) {
        setError("Could not find recipe");
      }
      finally {
        setIsLoading(false);
      }
    }

    fetchRecipe();
  }, [id]);

  if (!recipe) {
    return <Text>Recipe not found.</Text>;
  }

  return (
    <View style={{ flex: 1, padding: 20 }}>
      <Text style={{ fontSize: 24, fontWeight: 'bold' }}>{recipe.name}</Text>
      <Text style={{ marginTop: 10 }}>{recipe.name}</Text>
    </View>
  );
}