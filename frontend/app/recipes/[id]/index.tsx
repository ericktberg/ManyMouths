import React, { useState, useEffect } from 'react';
import {
  ArrowLeft, Edit3, Star, Heart, Clock, Users, DollarSign,
  Plus, ChefHat, ShoppingCart, Play, ThumbsUp, ThumbsDown, Link2
} from 'lucide-react-native';
import {
  View, Text, ScrollView, TouchableOpacity, Image, StyleSheet,
} from 'react-native';
import { useSafeAreaInsets } from 'react-native-safe-area-context';
import { Button } from '@/ui/button';
import { Badge } from '@/ui/badge';
import { Card, CardContent } from '@/ui/card';
import { Separator } from '@/ui/separator';
import { router, useLocalSearchParams, useNavigation } from 'expo-router';
import { RecipeDetailDTO, RecipeOverviewDTO, RecipesService } from '@/src/api-client';


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
        recipeDetails={recipe}
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

interface Recipe {
  name: string;
  description: string;
  prepTime: number;
  cookTime: number;
  servings: number;
  tags: string[];
  estimatedCost: number;
  category: 'veggie' | 'beef' | 'pork' | 'chicken' | 'seafood' | 'baked' | 'sauce';
  mlPreference: 'love' | 'like' | 'dislike';
  isFavorite: boolean;
  source: 'mine' | 'family' | 'friend';
  authorName?: string;
  ingredients: Array<{
    id: string;
    name: string;
    amount: string;
    unit: string;
  }>;
  instructions: string;
  notes?: string;
  timesCooked?: number;
  lastCooked?: Date;
  averageRating?: number;
}

interface RecipeOverviewProps {
  recipeDetails: RecipeDetailDTO;
  onBack: () => void;
  onEdit: () => void;
  onRate: () => void;
  onAddToMeal: () => void;
  onGoToMeal: () => void;
  onStartCooking: () => void;
  onLinkIngredients: () => void;
  isInMeal: boolean;
}

export function RecipeOverview({
  recipeDetails,
  onBack,
  onEdit,
  onRate,
  onAddToMeal,
  onGoToMeal,
  onStartCooking,
  onLinkIngredients,
  isInMeal
}: RecipeOverviewProps) {
  const [recipeImage, setRecipeImage] = useState<string | null>(null);
  const insets = useSafeAreaInsets();

  // Mock recipe data - in a real app, this would be fetched
  const recipe: Recipe = {
    name: recipeDetails.name ?? "Name not found",
    description: recipeDetails.description ?? "Description not found",
    prepTime: recipeDetails.prepTimeMinutes ?? 15,
    cookTime: recipeDetails.cookTimeMinutes ?? 30,
    servings: recipeDetails.servings ?? 4,
    tags: ['Quick', 'Asian', 'Protein', 'Family-Friendly'],
    estimatedCost: 12.50,
    category: 'chicken',
    mlPreference: 'love',
    isFavorite: true,
    source: 'mine',
    timesCooked: 8,
    lastCooked: new Date('2024-01-15'),
    averageRating: 4.6,
    ingredients: [
      { id: '1', name: 'Chicken thighs', amount: '2', unit: 'lbs' },
      { id: '2', name: 'Hoisin sauce', amount: '1/4', unit: 'cup' },
      { id: '3', name: 'Soy sauce', amount: '2', unit: 'tbsp' },
      { id: '4', name: 'Rice wine vinegar', amount: '1', unit: 'tbsp' },
      { id: '5', name: 'Fresh ginger', amount: '1', unit: 'tbsp' },
      { id: '6', name: 'Garlic', amount: '3', unit: 'cloves' },
      { id: '7', name: 'Sesame oil', amount: '1', unit: 'tsp' },
      { id: '8', name: 'Green onions', amount: '2', unit: 'whole' },
    ],
    instructions: `1. Preheat oven to 425°F and line a baking sheet with parchment paper.

2. In a small bowl, whisk together hoisin sauce, soy sauce, rice wine vinegar, minced ginger, minced garlic, and sesame oil for [02:00].

3. Pat chicken thighs dry and season with salt and pepper. Place on the prepared baking sheet.

4. Brush half of the glaze over the chicken thighs, reserving the rest for later.

5. Roast in the preheated oven for [20:00], or until internal temperature reaches 165°F.

6. Remove from oven and brush with remaining glaze. Let rest for [05:00] before serving.

7. Garnish with sliced [green onions] and serve immediately with steamed rice and vegetables.`,
    notes: 'For extra flavor, marinate the chicken in the glaze for 30 minutes before cooking. This recipe doubles easily for larger families!'
  };

  // Load recipe image
  useEffect(() => {
    const loadImage = async () => {
      try {

      } catch (error) {
        console.error('Failed to load recipe image:', error);
      }
    };
    loadImage();
  }, []);

  const getPreferenceIcon = (preference: string) => {
    switch (preference) {
      case 'love':
        return <View className="flex-row items-center gap-1 text-lime"><ThumbsUp className="h-4 w-4" /><ThumbsUp className="h-4 w-4" /></View>;
      case 'like':
        return <ThumbsUp className="h-4 w-4 text-blueberry" />;
      case 'dislike':
        return <ThumbsDown className="h-4 w-4 text-tomato" />;
      default:
        return null;
    }
  };

  const getTagColor = (index: number) => {
    const colors = ['tomato', 'lime', 'blueberry', 'grape', 'orange'];
    return colors[index % colors.length];
  };

  const parseInstructions = (instructions: string) => {
    return instructions.split('\n').filter(line => line.trim()).map((step, index) => {
      const stepMatch = step.match(/^(\d+)\.\s*(.*)/);
      if (stepMatch) {
        return {
          number: parseInt(stepMatch[1]),
          text: stepMatch[2],
          hasTimer: /\[\d{2}:\d{2}\]/.test(step),
          hasTemperature: /\d+°[FC]/.test(step),
          hasIngredient: /\[[^\]]+\]/.test(step) && !/\[\d{2}:\d{2}\]/.test(step)
        };
      }
      return null;
    }).filter(Boolean);
  };

  const instructions = parseInstructions(recipe.instructions);
  const unlinkedIngredients = recipe.ingredients.filter(ing =>
    !['Hoisin sauce', 'Chicken thighs'].includes(ing.name)
  );

  return (
    <View className='flex justify-center align-middle'>
      <HeaderControls recipe={recipe} />
      <Text>{recipe.description}</Text>
    </View>
  )
}

interface HeaderControlsProps {
  recipe: Recipe;
}

function HeaderControls({ recipe }: HeaderControlsProps) {
  /* Header Controls
       - Back Button (integrated in view)
       - Edit button
       - Favorite button

      This is a single bar that places them in the appropriate locations
     */
  return (
    <View className='flex flex-row p-2 justify-between items-start'>
      <Button
        variant="secondary"
        size="sm"
        className="transparent backdrop-blur-sm hover:bg-orange"
      >
        <ArrowLeft className="h-4 w-4" />
      </Button>
      <View className='flex flex-row gap-2 justify-right'>
        <Button
          variant="secondary"
          size="sm"
          className="transparent backdrop-blur-sm hover:bg-orange"
        >
          <Edit3 className="h-4 w-4" />
        </Button>
        <Button
          variant="secondary"
          size="sm"
          className="transparent backdrop-blur-sm hover:bg-orange"
        >
          <Star className={`h-4 w-4 ${recipe.isFavorite ? 'fill-current' : ''}`} />
        </Button>
      </View>
    </View>
  );
}