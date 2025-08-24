// recipes/[id].tsx
import { View, Text } from 'react-native';
import { useLocalSearchParams, useNavigation } from 'expo-router';
import { useEffect, useState } from 'react';
import { RecipeList, sampleRecipes, Recipe } from '@/components/recipelist';

export default function RecipeDetailsPage() {
  const { id } = useLocalSearchParams();
  // Change the state type to be either a 'Recipe' or 'null'
  const [recipe, setRecipe] = useState<Recipe | null>(null);
  const navigation = useNavigation();

  useEffect(() => {
    // The id from the URL params can be a string, or an array of strings,
    // so we need to ensure we're working with a string.
    const recipeId = Array.isArray(id) ? id[0] : id;

    // Find the recipe that matches the ID from the URL
    const selectedRecipe = sampleRecipes.find(r => r.id === recipeId);

    // If a recipe is found, set it in the state
    if (selectedRecipe) {
      navigation.setOptions({ title: selectedRecipe.name });
      setRecipe(selectedRecipe);
    } else {
      // If no recipe is found, set the state back to null
      setRecipe(null);
    }
  }, [id]);

  if (!recipe) {
    return <Text>Recipe not found.</Text>;
  }

  return (
    <View style={{ flex: 1, padding: 20 }}>
      <Text style={{ fontSize: 24, fontWeight: 'bold' }}>{recipe.name}</Text>
      <Text style={{ marginTop: 10 }}>{recipe.description}</Text>
    </View>
  );
}