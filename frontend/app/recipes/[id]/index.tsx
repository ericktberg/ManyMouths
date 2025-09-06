import React, { useState, useEffect } from 'react';
import {
  ArrowLeft, Edit3, Star, Heart, Clock, Users, DollarSign,
  Plus, ChefHat, ShoppingCart, Play, ThumbsUp, ThumbsDown, Link2
} from 'lucide-react-native';
import {
  View, Text, ScrollView, TouchableOpacity, Image, StyleSheet,
} from 'react-native';
import { useSafeAreaInsets } from 'react-native-safe-area-context';
import { Button } from '@/components/ui/button';
import { Badge } from '@/components/ui/badge';
import { Card, CardContent } from '@/components/ui/card';
import { Separator } from '@/components/ui/separator';
import { router, useLocalSearchParams, useNavigation } from 'expo-router';
import { RecipeDetailDTO, RecipeOverviewDTO, RecipesService } from '@/src/api-client';
import { RecipeOverview } from '@/components/forms/recipe-details-page';


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

  if (recipe === null) {
    return <Text>Recipe not found.</Text>;
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
